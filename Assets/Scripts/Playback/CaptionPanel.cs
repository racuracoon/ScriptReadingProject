using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CaptionPanel : MonoBehaviour
{
    public GameObject captionPanel;
    public TMP_Text characterText;
    public TMP_Text lineText;

    public void UpdatePanel(int lineId)
    {
        Dialogue dialogue = ScriptMemoryStore.GetDialougeByLineId(lineId);
        characterText.text = dialogue.character;
        lineText.text = dialogue.line;
    }
    public void ClearPanel()
    {
        characterText.text = "";
        lineText.text = "";
    }
}
