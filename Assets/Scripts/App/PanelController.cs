using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject inputScriptPanel;
    public GameObject inputScenePanel;
    public GameObject environmentSetupPanel;
    public GameObject readingSetupPanel;
    public Button addScriptBtn;
    public Button addSceneBtn;
    public Button settingBtn;
    public Button startBtn;
    public CharacterLIstContiner characterLIstContiner;
    public SceneListContainer sceneListContainer;
    public SelectSceneContainer selectSceneContainer;

    public void Start()
    {
        OpenMainPanel();
        addScriptBtn.onClick.AddListener(OpenInputScriptPanel);
        addSceneBtn.onClick.AddListener(OpenInputScenePanel);
        settingBtn.onClick.AddListener(OpenSettingPanel);
        startBtn.onClick.AddListener(OpenReadingSetupPanel);
    }
    public void OpenMainPanel()
    {
        CloseAll();
        mainPanel.SetActive(true);
    }
    public void OpenInputScriptPanel()
    {
        CloseAll();
        inputScriptPanel.SetActive(true);
        sceneListContainer.LoadSceneTexts();
    }
    public void OpenInputScenePanel()
    {
        CloseAll();
        inputScenePanel.SetActive(true);
    }

    public void OpenSettingPanel()
    {
        CloseAll();
        environmentSetupPanel.SetActive(true);
        characterLIstContiner.CreateCharacterBtn();
    }
    public void OpenReadingSetupPanel()
    {
        CloseAll();
        readingSetupPanel.SetActive(true);
        selectSceneContainer.LoadSceneBtn();
    }
    public void CloseAll()
    {
        mainPanel.SetActive(false);
        inputScriptPanel.SetActive(false);
        inputScenePanel.SetActive(false);
        environmentSetupPanel.SetActive(false);
        readingSetupPanel.SetActive(false);
    }

}
