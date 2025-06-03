using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentSetupPanel : MonoBehaviour
{
    public Button backToMainBtn;
    public Button modeBtn;
    public TMP_Text modeBtnText;
    public TMP_InputField avatarUrlInput;
    public Button loadAvatarBtn;
    public Button saveAvatarBtn;
    public PanelController panelController;
    public MessagePanel messagePanel;
    public TMP_InputField imagePathInput;
    public Button loadImageBtn;
    public Button applyImageBtn;
    public BackgroundImageLoader backgroundImageLoader;

    public GameObject characterSettingGroup;
    public GameObject backgroundSettingGroup;

    private Mode currentMode = Mode.CharacterSetting;

    private enum Mode
    {
        CharacterSetting,
        BackgroundSetting
    }

    public CharacterData selectedCharacter;

    public void Start()
    {
        backToMainBtn.onClick.AddListener(OnClickBackToMainBtn);
        loadAvatarBtn.onClick.AddListener(onClickLoadAvatarBtn);
        saveAvatarBtn.onClick.AddListener(onClickSaveAvatarBtn);
        modeBtn.onClick.AddListener(onClickModeBtn);
        loadImageBtn.onClick.AddListener(onClickLoadImageBtn);
        applyImageBtn.onClick.AddListener(OnClickApplyImageBtn);
        SwitchMode(Mode.CharacterSetting);
    }

    public void OnClickBackToMainBtn()
    {
        panelController.OpenMainPanel();
    }

    private void onClickLoadAvatarBtn()
    {
        string url = avatarUrlInput.text;

        if (!string.IsNullOrEmpty(url))
        {
            LoadAvatorHandler.LoadAvatar(url);
        }
        else messagePanel.OpenTemporaryPanel("url을 입력해주세요");
    }

    private void onClickSaveAvatarBtn()
    {
        string url = avatarUrlInput.text;
        if (!string.IsNullOrEmpty(url) &&
           url.StartsWith("https://models.readyplayer.me/") &&
           url.EndsWith(".glb"))
        {
            selectedCharacter.avatarUrl = url;
            Debug.Log("캐릭터 외형이 저장 되었습니다.");
        }
        else Debug.LogWarning("잘못된 URL");
    }

    private void onClickModeBtn()
    {
        Mode nextMode = currentMode == Mode.BackgroundSetting
            ? Mode.CharacterSetting
            : Mode.BackgroundSetting;

        Debug.Log($"[{currentMode} -> {nextMode}]");

        SwitchMode(nextMode);
        modeBtnText.text = nextMode == Mode.CharacterSetting ? "배경 설정" : "캐릭터외형설정";
    }

    private void onClickLoadImageBtn()
    {
        imagePathInput.text = backgroundImageLoader.LoadImage();
    }

    private void OnClickApplyImageBtn()
    {
        if (!string.IsNullOrEmpty(imagePathInput.text))
        {
            bool isSuccess = backgroundImageLoader.ApplyImage(imagePathInput.text);
            if (isSuccess)
            {
                messagePanel.OpenTemporaryPanel("배경이 적용되었습니다.");
            }
            else
            {
                messagePanel.OpenTemporaryPanel("배경 적용에 실패했습니다.");
            }
        }
        else
        {
            messagePanel.OpenTemporaryPanel("배경을 불러와 주세요");
            Debug.Log(imagePathInput.text);
        }
    }

    private void SwitchMode(Mode mode)
    {
        currentMode = mode;
        if (currentMode == Mode.CharacterSetting)
        {
            characterSettingGroup.SetActive(true);
            backgroundSettingGroup.SetActive(false);
        }
        else
        {
            characterSettingGroup.SetActive(false);
            backgroundSettingGroup.SetActive(true);
        }

    }
}
