using System.Threading.Tasks;
using UnityEngine;

public class AvatarBehaviour : MonoBehaviour
{
    private TTSService ttsService;
    private AudioSource avatarAudio;
    private GameObject parentAvatar;
    private Vector3 homePosition;

    public void Init(GameObject avatar, AudioSource audioSource, TTSService tts, Vector3 position)
    {
        Debug.Log("init");
        avatarAudio = audioSource;
        ttsService = tts;
        parentAvatar = avatar;
        homePosition = position;
    }

    public async Task SayLine(int lineId)
    {
        AvatarLoaderOnPlay.MoveAvatarToSpeakingPosition(parentAvatar);
        await ttsService.PlayTTS(lineId, avatarAudio);
        Debug.Log("💬 SayLine 완료됨");
    }

    public void MoveBackPosition()
    {
        parentAvatar.transform.position = homePosition;
    }

    public void MoveToSpeakingPosition()
    {
        // AvatarLoaderOnPlay에 있는 함수 여기로 옮겨야 함
    }
}
