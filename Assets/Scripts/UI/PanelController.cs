using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject addScriptPanel;
    public GameObject addScenePanel;
    public GameObject updateScenePanel;
    public GameObject settingPanel;
    public Button addScriptBtn;
    public Button addSceneBtn;
    public Button settingBtn;
    public Button backToMain_AddScriptBtn;
    public Button backToMain_SettingBtn;
    public Button backToAddScript_AddSceneBtn;
    public Button backToAddScript_UpdateBtn;
    public SceneViewer sceneViewer;
    public DialogueBoxManager dialogueBoxManager;
    public CharacterLIstContiner characterLIstContiner;

    public void Start()
    {
        OpenMainPanel();
        addScriptBtn.onClick.AddListener(OpenAddScriptPanel);
        backToMain_AddScriptBtn.onClick.AddListener(OpenMainPanel);
        addSceneBtn.onClick.AddListener(OpenAddScenePanel);
        backToAddScript_AddSceneBtn.onClick.AddListener(OpenAddScriptPanel);
        backToAddScript_UpdateBtn.onClick.AddListener(OpenAddScriptPanel);
        backToMain_SettingBtn.onClick.AddListener(OpenMainPanel);
        settingBtn.onClick.AddListener(OpenSettingPanel);
    }
    public void OpenMainPanel()
    {
        CloseAll();
        mainPanel.SetActive(true);
    }
    public void OpenAddScriptPanel()
    {
        CloseAll();
        addScriptPanel.SetActive(true);
        sceneViewer.DisplayScenes();
    }
    public void OpenAddScenePanel()
    {
        CloseAll();
        addScenePanel.SetActive(true);
    }
    public void OpenUpdateScenePanel(SceneData sceneToEdit)
    {
        CloseAll();
        updateScenePanel.SetActive(true);

        dialogueBoxManager.LoadScene(sceneToEdit);
    }
    public void OpenSettingPanel()
    {
        CloseAll();
        settingPanel.SetActive(true);
        characterLIstContiner.CreateCharacterBtn();
    }
    void CloseAll()
    {
        mainPanel.SetActive(false);
        addScriptPanel.SetActive(false);
        addScenePanel.SetActive(false);
        updateScenePanel.SetActive(false);
        settingPanel.SetActive(false);
    }

}
