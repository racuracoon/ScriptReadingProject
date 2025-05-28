using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public string name;
    public List<Dialogue> dialogues = new List<Dialogue>();
    public string avatarUrl = "https://models.readyplayer.me/6836c606d6b67cbdfabfd8bd.glb";
    public bool isUser = false;

    [System.NonSerialized] // 저장 X, 런타임 전용
    public GameObject avatar;
}
