using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.TextCore.Text;

public class CharacterMemoryStore
{
    public static List<CharacterData> characterList = new List<CharacterData>();

    public static CharacterData GetCharacterByLine(int lineId)
    {
        foreach (CharacterData character in characterList)
        {
            foreach (Dialogue dialogue in character.dialogues)
            {
                if (dialogue.lineId == lineId)
                {
                    return character;
                }
            }
        }
        return null;
    }

    public static void ReportRemovedCharacters(SceneData currentScene, List<string> characters)
    {
        List<string> confirmedRemovals = new List<string>();

        foreach (string name in characters)
        {
            bool existsElsewhere = false;

            foreach (SceneData scene in ScriptMemoryStore.currentScript.scenes)
            {
                if (scene == currentScene) continue;

                foreach (Dialogue d in scene.dialogues)
                {
                    if (d.character == name)
                    {
                        existsElsewhere = true;
                        break;
                    }
                }

                if (existsElsewhere)
                    break;
            }

            if (!existsElsewhere)
            {
                confirmedRemovals.Add(name);
            }
        }

        characters.AddRange(confirmedRemovals);

        foreach (string name in confirmedRemovals)
        {
            CharacterData character = characterList.Find(c => c.name == name);
            if (character != null)
            {
                characterList.Remove(character);
            }
        }
    }
}
