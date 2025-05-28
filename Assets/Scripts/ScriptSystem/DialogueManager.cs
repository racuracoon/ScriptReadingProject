using System.Collections.Generic;
using UnityEngine;

public class DialogueManager
{
    // 컨테이너에서 Dialgoues 받아옴
    public static List<Dialogue> GetDialoguesFromContainer(Transform dialogueContainer)
    {
        List<Dialogue> dialogues = new List<Dialogue>();

        int index = 0;

        foreach (Transform child in dialogueContainer)
        {
            DialoguePrefab box = child.GetComponent<DialoguePrefab>();
            if (box != null)
            {
                string character = box.characterInput.text.Trim();
                string line = box.dialogueInput.text.Trim();

                Dialogue dialogue = new Dialogue
                {
                    character = character,
                    line = line,
                };

                dialogues.Add(dialogue);
                index++;
            }
            else
            {
                Debug.LogWarning(" DialogueInputBox 컴포넌트를 찾을 수 없음");
            }
        }

        return dialogues;
    }

    public static void GrantDialogueId(SceneData scene)
    {
        if (scene == null || scene.dialogues == null)
        {
            Debug.LogWarning("Scene 또는 대사 목록이 비어 있습니다.");
            return;
        }

        int sceneNumber = scene.sceneNumber;

        for (int i = 0; i < scene.dialogues.Count; i++)
        {
            Dialogue dialogue = scene.dialogues[i];
            string lineId = $"{sceneNumber:D2}{(i + 1):D3}";
            dialogue.lineId = int.Parse(lineId);
            Debug.Log($"대사 ID : {lineId}");
        }

        Debug.Log($"✅ 씬 {sceneNumber}의 대사 ID가 할당되었습니다.");
    }
}
