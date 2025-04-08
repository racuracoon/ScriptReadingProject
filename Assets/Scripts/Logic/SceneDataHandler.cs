using System.Collections.Generic;
using UnityEngine;

public class SceneDataHandler
{
    public static void SaveSceneToScript(ScriptData script, SceneData scene)
    {
        script.scenes.Add(scene);
    }

    public static SceneData CreateScene(string title, List<Dialogue> dialogues)
    {
        return new SceneData
        {
            title = title,
            dialogues = dialogues
        };
    }
}