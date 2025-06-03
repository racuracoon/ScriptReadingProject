using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public DialogueListContainer dialogueListContainer;
    public MessagePanel messagePanel;

    public bool SaveScene(string sceneTitle)
    {
        bool validate = ValidateDialogues();
        if (!validate) return false;
        var dialogues = DialogueManager.GetDialoguesFromContainer(dialogueListContainer.dialogueListContainer);
        var scene = CreateScene(sceneTitle, dialogues);
        AddScene(scene);
        DialogueManager.GrantDialogueId(scene);

        CharacterObjectManager.CreateCharacterObject();


        if (ScriptMemoryStore.currentScript != null && ScriptMemoryStore.currentScript.scenes != null)
        {
            foreach (SceneData s in ScriptMemoryStore.currentScript.scenes)
            {
                Debug.Log($"씬 {scene.sceneNumber}: {scene.title}");

                foreach (Dialogue dialogue in scene.dialogues)
                {
                    Debug.Log($"{dialogue.character} : {dialogue.line}");
                }
            }
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool UpdateScene(SceneData updateScene, string sceneTitle)
    {
        bool validate = ValidateDialogues();
        if (!validate) return false;

        var dialogues = DialogueManager.GetDialoguesFromContainer(dialogueListContainer.dialogueListContainer);
        var sceneToUpdate = GetSceneByNumber(updateScene.sceneNumber);

        if (sceneToUpdate == null)
        {
            Debug.Log("⚠ 해당 sceneNumber의 씬을 찾지 못했습니다.");
            return false;
        }

        sceneToUpdate.title = sceneTitle;
        sceneToUpdate.dialogues = dialogues;

        DialogueManager.GrantDialogueId(sceneToUpdate);

        CharacterObjectManager.CreateCharacterObject();

        Debug.Log($"✅ 씬 #{sceneToUpdate.sceneNumber} 수정 완료: {sceneToUpdate.title}");
        return true;
    }

    public static SceneData CreateScene(string title, List<Dialogue> dialogues)
    {
        return new SceneData
        {
            title = title,
            dialogues = dialogues
        };
    }


    // Script에 Scene 추가 
    public static void AddScene(SceneData scene)
    {
        if (ScriptMemoryStore.currentScript == null)
        {
            Debug.LogWarning("currentScript가 비어있습니다.");
            return;
        }

        scene.sceneNumber = ScriptMemoryStore.currentScript.scenes.Count + 1;
        ScriptManager.AddScene(scene);

        Debug.Log($"✅ 씬 #{scene.sceneNumber} 추가됨: {scene.title}");
    }

    private SceneData GetSceneByNumber(int sceneNumber)
    {
        foreach (SceneData scene in ScriptMemoryStore.currentScript.scenes)
        {
            if (scene.sceneNumber == sceneNumber)
                return scene;
        }

        return null;
    }

    private bool ValidateDialogues()
    {
        foreach (Transform child in dialogueListContainer.dialogueListContainer)
        {
            DialoguePrefab box = child.GetComponent<DialoguePrefab>();
            if (box != null)
            {
                string character = box.characterInput.text.Trim();
                string line = box.dialogueInput.text.Trim();

                if (string.IsNullOrEmpty(character) || string.IsNullOrEmpty(line))
                {
                    messagePanel.OpenTemporaryPanel("캐릭터 또는 대사가 비어있습니다.");
                    return false;
                }
            }
            else return false;
            
        }
        return true;
    }
}
