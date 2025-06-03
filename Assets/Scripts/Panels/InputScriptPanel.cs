using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputScriptPanel : MonoBehaviour
{
    public Button backToMainBtn;
    public Button saveScriptBtn;
    public Button addSceneBtn;
    public TMP_InputField scriptTitleInput;
    public SceneListContainer sceneListContainer;
    public MessagePanel messagePanel;

    public PanelController panelController;

    void Start()
    {
        backToMainBtn.onClick.AddListener(OnClickBackToMainBtn);
        saveScriptBtn.onClick.AddListener(OnClickSaveScriptBtn);
    }

    public void OnClickBackToMainBtn()
    {
        panelController.OpenMainPanel();
    }

    public void OnClickSaveScriptBtn()
    {
        string scriptTitle = scriptTitleInput.text;
        if (!string.IsNullOrEmpty(scriptTitle))
        {
            ScriptManager.SaveScript(scriptTitle);
            Debug.Log("Script가 저장되었습니다.");
            panelController.OpenMainPanel();
        }
        else
        {
            messagePanel.OpenTemporaryPanel("Script를 저장하려면 제목을 입력해야 합니다.");
        }
    }
}
