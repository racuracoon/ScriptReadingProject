using UnityEngine;
using UnityEngine.UI;

public class MainPanels : MonoBehaviour
{
    public Button goToInputScriptBtn;
    public Button EnvironmentSetupBtn;
    public Button goToReadingSetupBtn;
    public PanelController panelController;

    public void Start()
    {
        goToInputScriptBtn.onClick.AddListener(OnClickGoToInputScriptBtn);
        EnvironmentSetupBtn.onClick.AddListener(OnClickGoToEnvironmentSetupBtn);
        goToReadingSetupBtn.onClick.AddListener(OnClickGoToReadingSetupBtn);
    }
    
    public void OnClickGoToInputScriptBtn()
    {
        panelController.OpenInputScriptPanel();
    }

    public void OnClickGoToEnvironmentSetupBtn()
    {
        panelController.OpenSettingPanel();
    }

    public void OnClickGoToReadingSetupBtn()
    {
        panelController.OpenReadingSetupPanel();
    }
}
