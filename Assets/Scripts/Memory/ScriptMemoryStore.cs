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
}
