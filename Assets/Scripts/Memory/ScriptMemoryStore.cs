using System.Collections.Generic;

public static class ScriptMemoryStore
{
    public static ScriptData currentScript = new ScriptData();

    public static void ResetScript(string newTitle)
    {
        currentScript = new ScriptData
        {
            title = newTitle,
            scenes = new List<SceneData>()
        };
    }
    public static Dialogue GetDialougeByLineId(int lineId)
    {
        foreach (SceneData scene in currentScript.scenes)
        {
            foreach (Dialogue dialogue in scene.dialogues)
            {
                if (dialogue.lineId == lineId) return dialogue;
            }
        }
        return null;
    }
}
