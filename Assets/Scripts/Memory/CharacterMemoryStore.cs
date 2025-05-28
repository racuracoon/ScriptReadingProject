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
}
