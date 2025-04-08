using System;
using System.Collections.Generic;

[Serializable]
public class SceneData
{
    public int sceneNumber;
    public string title;
    public List<Dialogue> dialogues = new List<Dialogue>();
    
    [NonSerialized] public ScriptData parentScript; // 런타임 전용 (저장 안 됨)
}