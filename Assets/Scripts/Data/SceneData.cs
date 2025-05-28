using System;
using System.Collections.Generic;

[Serializable]
public class SceneData
{
    public int sceneNumber;
    public string title;
    public List<Dialogue> dialogues = new List<Dialogue>();    
}