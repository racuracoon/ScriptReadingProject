using System;
using System.Collections.Generic;

[Serializable]
public class ScriptData
{
    public string title;
    public List<SceneData> scenes = new List<SceneData>();
}