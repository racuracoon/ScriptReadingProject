using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicInputListener : MonoBehaviour
{
    public AzureFeedbackService azureFeedbackService;

    public string selectedDevice = null; // 사용할 마이크 장치 (null이면 기본)
    public int sampleRate = 44100;       // 샘플링 레이트
    public int bufferLength = 10;        // 녹음 버퍼 길이 (초)

    private AudioSource audioSource;
    private bool isRecording = false;
    private int lineId;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.mute = true; // 자기 목소리 스피커에 안 나가게

        if (Microphone.devices.Length == 0)
        {
            Debug.LogWarning("마이크 장치가 없습니다.");
            return;
        }

        selectedDevice = selectedDevice ?? Microphone.devices[0];
        Debug.Log($"선택된 마이크: {selectedDevice}");
    }

    public void StartMic(int currentLineId)
    {
        if (isRecording) return;
        lineId = currentLineId;
        Debug.Log($"라인 아이디:{lineId}");
        audioSource.clip = Microphone.Start(selectedDevice, true, bufferLength, sampleRate);
        while (Microphone.GetPosition(selectedDevice) <= 0) { } // 마이크 준비될 때까지 대기

        audioSource.Play();
        isRecording = true;
        Debug.Log("마이크 입력 시작");
    }

    public void StopMic()
    {
        if (!isRecording) return;

        audioSource.Stop();
        Microphone.End(selectedDevice);
        isRecording = false;
        Debug.Log("마이크 입력 중단");

        azureFeedbackService.RequestFeedback(audioSource.clip, lineId);
    }

    public bool IsMicConnected()
    {
        return Microphone.devices.Length > 0;
    }

    public AudioClip GetCurrentClip()
    {
        return audioSource.clip;
    }
}
