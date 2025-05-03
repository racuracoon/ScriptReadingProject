using UnityEngine;
using TMPro;

public class ScriptSaveBtn : MonoBehaviour
{
    public TMP_InputField scriptTitleInput;
    public PanelController panelController;
    

    public void onClickSaveScript()
    {
        string sceneTitle = scriptTitleInput.text.Trim();
        if (ScriptMemoryStore.currentScript != null && ScriptMemoryStore.currentScript.scenes != null)
        {
            ScriptMemoryStore.currentScript.title = sceneTitle;
            panelController.OpenMainPanel();
            Debug.Log("스크립트가 성공적으로 저장됨");
            Debug.Log($"제목 : {ScriptMemoryStore.currentScript.title}");
        }
        else{
            Debug.Log("저장 실패");
        }
    }
}
