using UnityEngine;
using TMPro;

public class SceneSaveBtn : MonoBehaviour
{
    public DialogueBoxManager dialogueBoxManager;
    public TMP_InputField sceneTitleInput;
    public PanelController panelController;

    public void OnClickSaveScene()
    {
        string sceneTitle = sceneTitleInput.text.Trim();
        if (string.IsNullOrEmpty(sceneTitle))
        {
            Debug.Log("씬 제목이 비어 있습니다.");
            return;
        }

        foreach (Transform child in dialogueBoxManager.dialogueContainer)
        {
            DialogueInputBox box = child.GetComponent<DialogueInputBox>();
            if (box != null)
            {
                string character = box.characterInput.text.Trim();
                string line = box.dialogueInput.text.Trim();

                if (string.IsNullOrEmpty(character) || string.IsNullOrEmpty(line))
                {
                    Debug.LogWarning("캐릭터 또는 대사가 비어 있습니다.");
                    return;
                }
            }
        }

        var dialogues = DialogueDataHandler.ExtractDialogues(dialogueBoxManager.dialogueContainer);
        var scene = SceneDataHandler.CreateScene(sceneTitle, dialogues);
        ScriptDataHandler.AddScene(scene);

        Debug.Log($"씬 저장됨: {scene.title}, 대사 수: {scene.dialogues.Count}");
        Debug.Log("현재 스크립트에 저장된 씬 목록:");

        if (ScriptMemoryStore.currentScript != null && ScriptMemoryStore.currentScript.scenes != null)
        {
            foreach (SceneData s in ScriptMemoryStore.currentScript.scenes)
            {
                Debug.Log($"씬 {scene.sceneNumber}: {scene.title}");

                foreach (var dialogue in scene.dialogues)
                {
                    Debug.Log($"{dialogue.character} : {dialogue.line}");
                }
            }
        }
        else
        {
            Debug.Log("⚠ 저장된 스크립트나 씬이 없습니다.");
        }
        panelController.OpenAddScriptPanel();
    }
}
