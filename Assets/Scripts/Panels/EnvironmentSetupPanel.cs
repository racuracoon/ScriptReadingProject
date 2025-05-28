using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentSetupPanel : MonoBehaviour
{
    public Button backToMainBtn;
    public Button saveBtn;
    public TMP_InputField avatarUrlInput;
    public Button loadAvatarBtn;
    public Button SaveAvatarBtn;
    public PanelController panelController;
    
    public CharacterData selectedCharacter;

    public void Start()
    {
        backToMainBtn.onClick.AddListener(OnClickBackToMainBtn);
        loadAvatarBtn.onClick.AddListener(onClickLoadAvatarBtn);
        SaveAvatarBtn.onClick.AddListener(onClickSaveAvatarBtn);
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
        else Debug.LogWarning("url을 입력하세요");
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

}
