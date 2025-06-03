using System.Threading.Tasks;
using UnityEngine;

public class PlaybackManager : MonoBehaviour
{
    private SceneData currentScene;
    private int Index = 0;
    private CharacterData currentSpeaker;
    public UIControllerInPlay uIControllerInPlay;
    public KeyboardInputListener keyboardInputListener;
    public MicInputListener micInputListener;
    public CaptionPanel captionPanel;
    public MessagePanel messagePanel;
    public PanelController panelController;


    public void SetCurrentScene(SceneData scene)
    {
        currentScene = scene;
    }

    public void StartPlay()
    {
        uIControllerInPlay.OpenPlayPanel();
        keyboardInputListener.StartListening();
    }

    public void EndPlay()
    {
        uIControllerInPlay.ClosePlayPanel();
        keyboardInputListener.EndListening();
        Camera mainCam = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();
        if (mainCam != null)
        {
            mainCam.enabled = true;

            AudioListener listener = mainCam.GetComponent<AudioListener>();
            if (listener != null)
                listener.enabled = true;
        }
        AvatarLoaderOnPlay.DestroyAllAvatars();
        panelController.OpenReadingSetupPanel();
        FeedbackMemoryStore.Clear();
    }

    public async Task PlayCurrentLine()
    {
        if (Index < currentScene.dialogues.Count)
        {
            int currentLineId = currentScene.dialogues[Index].lineId;
            captionPanel.UpdatePanel(currentLineId);

            if (currentSpeaker != null)
            {
                AvatarBehaviour avatarBehaviour = currentSpeaker.avatar.GetComponent<AvatarBehaviour>();
                avatarBehaviour.MoveBackPosition();
            }

            currentSpeaker = GetAvatarByLineId(currentLineId);
            Debug.Log($"currentSpeaker : {currentSpeaker}");

            if (currentSpeaker.isUser)
            {
                uIControllerInPlay.setLineId(currentLineId);
                uIControllerInPlay.DisplayMicIMG();
                uIControllerInPlay.DisplayMicBtn();
            }
            else
            {
                uIControllerInPlay.UndisplayMicIMG();
                uIControllerInPlay.UndisplayMicBtn();
                await CommandSayLine(currentSpeaker, currentLineId);
            }
        }
    }

    public async Task PlayPreviousLine()
    {
        if (Index > 0)
        {
            Index--;
            await PlayCurrentLine();
        }
        else
        {
            messagePanel.OpenTemporaryPanel("이전 대사가 없습니다.");
        }
    }

    public async Task PlayNextLine()
    {
        if (Index < currentScene.dialogues.Count - 1)
        {
            Index++;
            await PlayCurrentLine();
        }
        else
        {
            messagePanel.OpenTemporaryPanel("다음 대사가 없습니다.");
        }
    }

    private CharacterData GetAvatarByLineId(int lineId)
    {
        CharacterData character = CharacterMemoryStore.GetCharacterByLine(lineId);
        return character;
    }

    private async Task CommandSayLine(CharacterData speaker, int lineId)
    {
        AvatarBehaviour avatarBehaviour = speaker.avatar.GetComponent<AvatarBehaviour>();
        await avatarBehaviour.SayLine(lineId);
    }

    public void ExitPlay()
    {

    }
}
