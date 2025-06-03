using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class InputScenePanel : MonoBehaviour
{
    public Button backToInputScriptBtn;
    public Button addDialogueBtn;
    public Button saveSceneBtn;
    public TMP_InputField sceneTitleInput;
    public DialogueListContainer dialogueListContainer;
    public SceneManager sceneManager;
    public PanelController panelController;
    public MessagePanel messagePanel;

    private SceneData updatingScene = null;

    public void Start()
    {
        backToInputScriptBtn.onClick.AddListener(OnClickBackToInputScriptBtn);
        addDialogueBtn.onClick.AddListener(OnClickAddDialogueBtn);
        saveSceneBtn.onClick.AddListener(OnClickSaveSceneBtn);
    }

    public void OnClickBackToInputScriptBtn()
    {
        panelController.OpenInputScriptPanel();
    }

    public void OnClickAddDialogueBtn()
    {
        dialogueListContainer.AddDialogueBox();
    }

    public void OnClickSaveSceneBtn()
    {
        string title = sceneTitleInput.text;


        if (!string.IsNullOrEmpty(title))
        {
            bool isSave;
            if (updatingScene == null)
            {
                isSave = sceneManager.SaveScene(title);
            }
            else
            {
                isSave = sceneManager.UpdateScene(updatingScene, title);
                updatingScene = null;
            }
            if (isSave)
            {
                Debug.Log("씬이 저장 되었습니다.");
                ClearInput();
                panelController.OpenInputScriptPanel();
            }

        }
        else
        {
            messagePanel.OpenTemporaryPanel("저장 하려면 씬 제목을 입력해 주세요");
        }
    }

    public void SetUpdatingScene(SceneData scene)
    {
        updatingScene = scene;
        sceneTitleInput.text = scene.title;
    }

    public void ClearInput()
    {
        sceneTitleInput.text = "";
        dialogueListContainer.ClearAllDialogues();
    }
}
