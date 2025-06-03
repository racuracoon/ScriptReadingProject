using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

public class AzureFeedbackService : MonoBehaviour
{
    public string serverUrl = "http://localhost:5000/analyze-speech";
    public FeedbackContainer feedbackContainer;
    public MessagePanel messagePanel;
    public UIControllerInPlay uIControllerInPlay;

    public void RequestFeedback(AudioClip clip, int lineId)
    {
        if (clip == null)
        {
            Debug.LogError("❌ AudioClip이 비어 있습니다.");
            return;
        }

        byte[] wavData = WavUtility.FromAudioClip(clip, out string _);
        StartCoroutine(SendRequest(clip, wavData, lineId));
    }

    IEnumerator SendRequest(AudioClip clip, byte[] wavBytes, int lineId)
    {
        Dialogue dialogue = ScriptMemoryStore.GetDialougeByLineId(lineId);
        string line = dialogue.line;

        WWWForm form = new WWWForm();
        form.AddBinaryData("audio", wavBytes, "recorded.wav", "audio/wav");
        form.AddField("reference_text", line);

        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, form))
        {
            Debug.Log("📡 응답 대기중...");
            messagePanel.OpenPanel("피드백 생성중 ...");
            www.timeout = 30;
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ 서버 응답 실패: " + www.error);
                messagePanel.OpenTemporaryPanel("피드백 서버 응답 실패");
                yield break;
            }

            JObject data = JObject.Parse(www.downloadHandler.text);

            var p = data["pronunciation"];
            float accuracy = (float)p["AccuracyScore"];
            float fluency = (float)p["FluencyScore"];
            float completeness = (float)p["CompletenessScore"];
            float pron = (float)p["PronScore"];

            List<float> scores = new List<float> { accuracy, fluency, completeness };

            var omitted = data["pronunciation_details"]?["omitted_words"];
            var unclear = data["pronunciation_details"]?["unclear_pronunciations"];

            List<string> omittedWords = omitted?.Select(w => w.ToString()).ToList() ?? new List<string>();
            List<string> unclearWords = unclear?.Select(w => w.ToString()).ToList() ?? new List<string>();

            string feedback = GenerateFeedback(accuracy, completeness, fluency, omittedWords, unclearWords);

            FeedbackMemoryStore.Add(clip, lineId, scores, feedback);
            feedbackContainer.LoadFeedbackList();
            messagePanel.ClosePanel();
            uIControllerInPlay.SwitchFeedbackPanel();
        }
    }

    string GenerateFeedback(float acc, float comp, float flu, List<string> omittedWords, List<string> unclearWords)
    {
        bool accHigh = acc >= 90f;
        bool compHigh = comp >= 90f;
        bool fluHigh = flu >= 90f;

        string omitted = string.Join(", ", omittedWords);
        string unclear = string.Join(", ", unclearWords);

        bool hasOmitted = omittedWords.Count > 0;
        bool hasUnclear = unclearWords.Count > 0;

        if (accHigh && compHigh && fluHigh)
        {
            return "완벽합니다!!";
        }
        else if (accHigh && compHigh && !fluHigh)
        {
            return "발음은 좋았지만, 말할 때 약간 끊김이 있었어요. 더 자연스럽게 말하면 완벽해요!";
        }
        else if (accHigh && !compHigh && fluHigh)
        {
            return hasOmitted
                ? $"'{omitted}'를 빼먹었어요. 하지만 발음이 좋고 매우 유창했어요!"
                : "문장을 다 말하지는 않았지만, 발음과 유창성은 매우 좋았어요!";
        }
        else if (accHigh && !compHigh && !fluHigh)
        {
            return hasOmitted
                ? $"'{omitted}'를 빼먹었고, 말할 때도 약간 끊김이 있었어요. 그래도 발음은 정확했어요!"
                : "문장을 조금 덜 말했고, 유창성도 부족했지만 발음은 괜찮았어요!";
        }
        else if (!accHigh && compHigh && fluHigh)
        {
            return hasUnclear
                ? $"'{unclear}'의 발음이 부정확했어요. 하지만 완성도도 높고, 말도 매우 유창했어요!"
                : "발음이 조금 아쉬웠지만, 문장도 완성했고 유창성도 좋았어요!";
        }
        else if (!accHigh && compHigh && !fluHigh)
        {
            return hasUnclear
                ? $"'{unclear}'의 발음이 부정확했고, 말할 때도 끊김이 있었어요. 하지만 문장은 잘 완성했어요!"
                : "발음과 유창성은 조금 아쉬웠지만, 문장은 잘 말했어요!";
        }
        else if (!accHigh && !compHigh && fluHigh)
        {
            if (hasOmitted && hasUnclear)
            {
                return $"'{omitted}'를 빼먹었고, '{unclear}'의 발음도 부정확했어요. 그래도 유창하게 말한 건 좋아요!";
            }
            else if (hasOmitted)
            {
                return $"'{omitted}'를 빼먹었지만, 유창하게 말한 건 좋아요!";
            }
            else if (hasUnclear)
            {
                return $"'{unclear}'의 발음이 부정확했지만, 유창하게 말한 건 좋아요!";
            }
            else
            {
                return "발음과 문장 완성도는 조금 부족했지만, 말은 유창했어요!";
            }
        }
        else // all low
        {
            if (hasOmitted && hasUnclear)
            {
                return $"'{omitted}'를 빼먹었고, '{unclear}'의 발음도 부정확했으며 말할 때도 끊김이 있었어요. 조금 더 연습하면 좋아질 수 있어요!";
            }
            else if (hasOmitted)
            {
                return $"'{omitted}'를 빼먹었고 말할 때도 끊김이 있었어요. 연습을 조금 더 해보면 좋겠어요!";
            }
            else if (hasUnclear)
            {
                return $"'{unclear}'의 발음이 부정확했고 말할 때도 끊김이 있었어요. 연습하면 충분히 좋아질 수 있어요!";
            }
            else
            {
                return "발음, 유창성, 완성도 모두 조금 아쉬웠어요. 하지만 계속 연습하면 금방 좋아질 수 있어요!";
            }
        }
    }

}
