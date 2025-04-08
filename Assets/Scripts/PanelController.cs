using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject addScriptPanel;
    public GameObject addScenePanel;
    public Button addScriptBtn;
    public Button addSceneBtn;
    public Button backToMain_AddScriptBtn;
    public Button backToAddScript_AddSceneBtn;

    public void Start()
    {
        OpenMainPanel();
        addScriptBtn.onClick.AddListener(OpenAddScriptPanel);
        backToMain_AddScriptBtn.onClick.AddListener(OpenMainPanel);
        addSceneBtn.onClick.AddListener(OpenAddScenePanel);
        backToAddScript_AddSceneBtn.onClick.AddListener(OpenAddScriptPanel);
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
    }
    public void OpenAddScenePanel()
    {
        CloseAll();
        addScenePanel.SetActive(true);
    }
    void CloseAll()
    {
        mainPanel.SetActive(false);
        addScriptPanel.SetActive(false);
        addScenePanel.SetActive(false);

    }

}
