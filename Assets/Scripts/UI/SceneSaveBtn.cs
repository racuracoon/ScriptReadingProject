using UnityEngine;
using TMPro;

public class SceneSaveBtn : MonoBehaviour
{
    public DialogueBoxManager dialogueBoxManager;
    public TMP_InputField sceneTitleInput;

    public void OnClickSaveScene()
    {
        var dialogues = DialogueDataHandler.ExtractDialogues(dialogueBoxManager.dialogueContainer);
        var scene = SceneDataHandler.CreateScene(sceneTitleInput.text, dialogues);
        ScriptDataHandler.AddScene(scene);

        Debug.Log($"✅ 씬 저장됨: {scene.title}, 대사 수: {scene.dialogues.Count}");
    }
}
