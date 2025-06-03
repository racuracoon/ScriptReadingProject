using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerInPlay : MonoBehaviour
{
    public GameObject playPanel;
    public Button playBtn;
    public GameObject controllGuidePanel;
    public GameObject captionPanel;
    public GameObject feedbackPanel;
    public Image micIMG;
    public PlaybackManager playbackManager;
    public MicInputListener micInputListener;
    public Button micBtn;
    public TMP_Text micBtn_text;
    public Button exitBtn;

    private bool isOpenCaption = false;
    private bool isOnMic = false;
    private bool isOpenFeedback = false;
    private int lineId;

    void Start()
    {
        playPanel.SetActive(false);
        captionPanel.SetActive(false);
        feedbackPanel.SetActive(false);
        micIMG.gameObject.SetActive(false);
        micBtn.gameObject.SetActive(false);
        controllGuidePanel.SetActive(false);
        playBtn.gameObject.SetActive(true);
        exitBtn.onClick.AddListener(OnClickExitBtn);
        playBtn.onClick.AddListener(OnClickPlayBtn);
        micBtn.onClick.AddListener(OnClickMicBtn);
    }

    public void OpenPlayPanel()
    {
        playPanel.SetActive(true);
    }

    public void ClosePlayPanel()
    {
        playPanel.SetActive(false);
        playBtn.gameObject.SetActive(true);
        controllGuidePanel.SetActive(false);
        captionPanel.SetActive(false);
        isOpenCaption = false;
        micIMG.gameObject.SetActive(false);
        playPanel.SetActive(false);
    }

    private async void OnClickPlayBtn()
    {
        Debug.Log("플레이");
        playBtn.gameObject.SetActive(false);
        controllGuidePanel.SetActive(true);
        captionPanel.gameObject.SetActive(true);

        await playbackManager.PlayCurrentLine();
    }

    private void OnClickMicBtn()
    {
        if (isOnMic == false)
        {
            micInputListener.StartMic(lineId);
            isOnMic = true;
            micBtn_text.text = "말하기 종료";
        }
        else
        {
            micInputListener.StopMic();
            isOnMic = false;
            micBtn_text.text = "말하기 시작";
        }
    }

    private void OnClickExitBtn()
    {
        playbackManager.EndPlay();
    }

    public void SwitchCaptionPanel()
    {
        if (isOpenCaption)
        {
            captionPanel.SetActive(false);
            isOpenCaption = false;
        }
        else
        {
            captionPanel.SetActive(true);
            isOpenCaption = true;
        }
    }

    public void SwitchFeedbackPanel()
    {
        if (isOpenFeedback)
        {
            feedbackPanel.SetActive(false);
            isOpenFeedback = false;
        }
        else
        {
            feedbackPanel.SetActive(true);
            isOpenFeedback = true;
        }
    }

    public void DisplayMicIMG()
    {
        micIMG.gameObject.SetActive(true);
    }

    public void UndisplayMicIMG()
    {
        micIMG.gameObject.SetActive(false);
    }

    public void DisplayMicBtn()
    {
        micBtn.gameObject.SetActive(true);
    }

    public void UndisplayMicBtn()
    {
        micBtn.gameObject.SetActive(false);
    }

    public void setLineId(int currentLineId)
    {
        lineId = currentLineId;
    }
}
