using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System;

public class TTSService : MonoBehaviour
{
    [Header("OpenAI 설정")]
    private string apiKey;
    public string voice = "nova";
    public AudioSource audioSource;

    void Awake()
    {
        apiKey = GetEnvValue("OPENAI_API_KEY");
    }

    private string GetEnvValue(string key)
    {
        string envPath = Path.Combine(Application.dataPath, ".env");
        if (!File.Exists(envPath)) return null;

        var lines = File.ReadAllLines(envPath);
        foreach (var line in lines)
        {
            if (line.StartsWith(key + "="))
            {
                return line.Substring(key.Length + 1).Trim();
            }
        }
        return null;
    }

    public async Task RequestTTSAsync(SceneData scene)
    {
        foreach (Dialogue dialogue in scene.dialogues)
        {
            int lineId = dialogue.lineId;
            string text = dialogue.line;

            if (string.IsNullOrWhiteSpace(text)) continue;

            string url = "https://api.openai.com/v1/audio/speech";
            string json = $"{{\"model\":\"tts-1\",\"input\":\"{EscapeJson(text)}\",\"voice\":\"{voice}\",\"response_format\":\"wav\"}}";

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

                var operation = request.SendWebRequest();

                // UnityWebRequest를 await로 감싸기
                while (!operation.isDone)
                    await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"❌ TTS 실패 (lineId: {lineId}): {request.error}");
                    continue;
                }

                byte[] wavData = request.downloadHandler.data;
                Debug.Log($"📥 응답 수신 완료 (lineId: {lineId}, WAV {wavData.Length} bytes)");

                AudioClip clip = WavUtility.ToAudioClip(wavData, 0, $"TTS_{lineId}");
                if (clip == null)
                {
                    Debug.LogError($"❌ AudioClip 변환 실패 (lineId: {lineId})");
                    continue;
                }

                TTSClipStore.Store(lineId, clip);
                Debug.Log($"✅ Clip 저장 완료: {lineId} (length: {clip.length:F2}s)");
            }
        }
    }

    public async Task PlayTTS(int lineId, AudioSource targetAudioSource, Action onComplete = null)
    {
        AudioClip clip = TTSClipStore.Get(lineId);

        if (clip == null)
        {
            Debug.LogWarning($"⚠️ lineId {lineId}에 대한 Clip 없음");
            return;
        }

        if (targetAudioSource == null)
        {
            Debug.LogWarning("⚠️ AudioSource 없음");
            return;
        }

        targetAudioSource.clip = clip;
        targetAudioSource.Play();

        Debug.Log($"🎤 아바타 TTS 재생 시작 (lineId: {lineId}, duration: {clip.length:F2}s)");

        await WaitForAudioEndAsync(targetAudioSource);

        Debug.Log("✅ TTS 재생 완료");
        onComplete?.Invoke();
    }

    private async Task WaitForAudioEndAsync(AudioSource source)
    {
        // AudioSource 재생이 끝날 때까지 대기
        while (source != null && source.isPlaying)
        {
            await Task.Yield();  // 프레임 단위로 대기
        }
    }


    private string EscapeJson(string str)
    {
        return str.Replace("\\", "\\\\")
                  .Replace("\"", "\\\"")
                  .Replace("\n", "\\n")
                  .Replace("\r", "\\r");
    }
}