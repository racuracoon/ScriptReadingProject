using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackContainer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public RectTransform feedbackContainer;
    public GameObject feedbackPrefab;

    public void LoadFeedbackList()
    {
        ClearContainer();
        float currentY = 0f;
        float prefabHieght = 600f;
        List<FeedbackData> feedbackList = FeedbackMemoryStore.GetFeedbackList();

        if (feedbackPrefab == null)
        {
            Debug.LogError("❌ feedbackPrefab이 null입니다. 인스펙터에서 연결했는지 확인하세요.");
            return;
        }

        foreach (FeedbackData feedback in feedbackList)
        {
            GameObject item = Instantiate(feedbackPrefab, feedbackContainer);
            RectTransform boxRT = item.GetComponent<RectTransform>();
            string line = ScriptMemoryStore.GetDialougeByLineId(feedback.lineId).line;

            boxRT.anchorMin = new Vector2(0.5f, 1f);
            boxRT.anchorMax = new Vector2(0.5f, 1f);
            boxRT.pivot = new Vector2(0.5f, 1f);
    
            boxRT.anchoredPosition = new Vector2(0f, -currentY);
            currentY += prefabHieght;

            feedbackContainer.sizeDelta = new Vector2(
                feedbackContainer.sizeDelta.x,
                currentY
            );

            FeedbackPrefab feedbackItem = item.GetComponent<FeedbackPrefab>();
            if (feedbackItem != null)
            {
                feedbackItem.line.text = line;
                feedbackItem.playAudioBtn.onClick.AddListener(() => OnClickPlayClipBtn(feedback.lineId));
                feedbackItem.accuracy.text = feedback.scores[0].ToString();
                feedbackItem.fluency.text = feedback.scores[1].ToString();
                feedbackItem.completeness.text = feedback.scores[2].ToString();
                feedbackItem.comment.text = feedback.commet;
            }

        }
    }

    public void ClearContainer()
    {
        foreach (Transform child in feedbackContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnClickPlayClipBtn(int lineId)
    {
        AudioClip clip = MicClipStore.Get(lineId);

        audioSource.Stop();         // 이전 클립 중지
        audioSource.clip = clip;    // 클립 할당
        audioSource.Play();         // 재생
    }
}
