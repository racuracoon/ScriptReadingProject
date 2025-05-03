using UnityEngine;
using TMPro;

public class SceneUpdateBtn : MonoBehaviour
{
    public DialogueBoxManager dialogueBoxManager;
    public TMP_InputField sceneTitleInput;
    public PanelController panelController;
    public SceneViewer sceneViewer;
    public SceneData editingScene;

    public void getScene(SceneData scene)
    {
        editingScene = scene;
    }

    public void OnClickUpdateScene()
    {
        var manager = UnityEngine.Object.FindFirstObjectByType<DialogueBoxManager>();
        string sceneTitle = sceneTitleInput.text.Trim();
        if (string.IsNullOrEmpty(sceneTitle))
        {
            Debug.Log("씬 제목이 비어 있습니다.");
            return;
        }

        if (manager.dialogueContainer.childCount > 0)
        {
            UpdateScene(sceneTitle);
        }
        else
        {
            Debug.Log("대사가 최소 하나는 있어야 합니다.");
            return;
        }

    }

    public void UpdateScene(string sceneTitle)
    {
        Debug.Log("수정 버튼 호출됨");
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
        ScriptDataHandler.UpdateScene(editingScene, sceneTitle, dialogues);

        dialogueBoxManager.DeleteAllDialogues();
        panelController.OpenAddScriptPanel();
        sceneViewer.DisplayScenes();
        if (ScriptMemoryStore.currentScript != null && ScriptMemoryStore.currentScript.scenes != null)
        {
            Debug.Log("📋 currentScript에 저장된 씬 번호 리스트:");
            foreach (var scene in ScriptMemoryStore.currentScript.scenes)
            {
                Debug.Log($"- SceneNumber: {scene.sceneNumber}, Title: {scene.title}");
            }
        }
        else
        {
            Debug.LogWarning("⚠ currentScript나 scenes가 비어있습니다.");
        }
    }
}

