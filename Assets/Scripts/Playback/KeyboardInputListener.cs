using UnityEngine;

public class KeyboardInputListener : MonoBehaviour
{
    public UIControllerInPlay uIControllerInPlay;
    public PlaybackManager playbackManager;
    public GameObject feedbackPanel;
    public FeedbackContainer feedbackContainer;

    private bool isListening = false;

    public void StartListening()
    {
        isListening = true;
    }

    public void EndListening()
    {
        isListening = false;
    }

    void Update()
    {
        if (isListening)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                uIControllerInPlay.SwitchCaptionPanel();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                uIControllerInPlay.SwitchFeedbackPanel();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // async 메서드는 직접 호출 불가
                _ = playbackManager.PlayPreviousLine();  //fire-and-forget
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // async 메서드는 직접 호출 불가
                _ = playbackManager.PlayNextLine();  //fire-and-forget
            }
        }
    }
}
