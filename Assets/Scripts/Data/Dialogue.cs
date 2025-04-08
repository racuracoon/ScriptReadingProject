using System;

[Serializable]
public class Dialogue
{
    public string character;
    public string line;

    public int index;
    public string id;

    [NonSerialized] public SceneData parentScene;
}
