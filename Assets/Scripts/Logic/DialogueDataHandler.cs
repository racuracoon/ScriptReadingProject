using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueDataHandler
{
    public static List<Dialogue> ExtractDialogues(Transform dialogueContainer)
    {
        List<Dialogue> dialogues = new List<Dialogue>();

        int index = 0;

        foreach (Transform child in dialogueContainer)
        {
            DialogueInputBox box = child.GetComponent<DialogueInputBox>();
            if (box != null)
            {
                string character = box.characterInput.text.Trim();
                string line = box.dialogueInput.text.Trim();


                if (string.IsNullOrEmpty(character) && string.IsNullOrEmpty(line))
                {
                    Debug.LogWarning("대사 박스가 비어있음");
                    continue;
                }

                Dialogue dialogue = new Dialogue
                {
                    character = character,
                    line = line,
                    index = index,
                    id = Guid.NewGuid().ToString()
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
}
