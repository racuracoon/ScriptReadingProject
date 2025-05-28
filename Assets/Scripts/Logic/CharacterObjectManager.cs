using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterObjectManager : MonoBehaviour
{
    public static HashSet<string> GetCharacterNameList()
    {
        HashSet<string> characterSet = new HashSet<string>();
        if (ScriptMemoryStore.currentScript == null)
        {
            Debug.Log("현재 스크립트가 비어있음");
            return characterSet;
        }

        foreach (SceneData scene in ScriptMemoryStore.currentScript.scenes)
        {
            if (scene.dialogues == null) continue;

            foreach (Dialogue dialogue in scene.dialogues)
            {
                string name = dialogue.character?.Trim();
                if (!string.IsNullOrEmpty(name))
                {
                    characterSet.Add(name);
                }
            }
        }

        return characterSet;
    }

    public static List<CharacterData> GetCharacterListByScene(SceneData scene)
    {
        List<CharacterData> result = new List<CharacterData>();
        if (scene == null || scene.dialogues == null)
        {
            Debug.LogWarning("GetCharacterListByScene: scene 또는 dialogue가 null");
            return result;
        }

        // 1. 이 씬에 등장하는 캐릭터 이름 수집
        HashSet<string> characterNames = new HashSet<string>();
        foreach (Dialogue dialogue in scene.dialogues)
        {
            string name = dialogue.character?.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                characterNames.Add(name);
            }
        }

        // 2. 등장하는 이름에 해당하는 CharacterData만 수집
        foreach (string name in characterNames)
        {
            CharacterData data = CharacterMemoryStore.characterList.Find(c => c.name == name);
            if (data != null)
            {
                result.Add(data);
            }
        }

        return result;
    }

    public static void CreateCharacterObject()
    {
        if (ScriptMemoryStore.currentScript == null || ScriptMemoryStore.currentScript.scenes == null)
        {
            Debug.LogWarning("스크립트가 비어 있습니다.");
            return;
        }
        HashSet<string> characterNames = GetCharacterNameList();
        foreach (string name in characterNames)
        {
            CharacterData existing = CharacterMemoryStore.characterList.Find(c => c.name == name);
            if (existing != null) // 해당 캐릭터가 이미 존재하면
            {
                existing.dialogues.Clear();    // 기존 대사 초기화
                foreach (SceneData scene in ScriptMemoryStore.currentScript.scenes)
                {
                    if (scene.dialogues == null) continue;

                    foreach (Dialogue dialogue in scene.dialogues)
                    {
                        if (dialogue.character.Trim() == name)
                        {
                            existing.dialogues.Add(new Dialogue
                            {
                                lineId = dialogue.lineId,
                                line = dialogue.line
                            });
                        }
                    }

                }
            }
            else  // 새로운 캐릭터인 경우
            {
                CharacterData characterData = new CharacterData
                {
                    name = name,
                    dialogues = new List<Dialogue>()
                };

                foreach (SceneData scene in ScriptMemoryStore.currentScript.scenes)
                {
                    if (scene.dialogues == null) continue;

                    foreach (Dialogue dialogue in scene.dialogues)
                    {
                        if (dialogue.character.Trim() == name)
                        {
                            characterData.dialogues.Add(new Dialogue
                            {
                                lineId = dialogue.lineId,
                                line = dialogue.line
                            });
                        }
                    }
                }
                CharacterMemoryStore.characterList.Add(characterData);
                Debug.Log($"✅ 새로운 캐릭터 등록: {characterData.name}, 대사 수: {characterData.dialogues.Count}");
            }

        }

    }
}