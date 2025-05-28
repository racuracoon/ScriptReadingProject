using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CharacterBtn : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private Button button;
    private string characterName;

    public void Init(string character, Action<string> onClickCallback)
    {
        characterName = character;
        text.text = character;

        if (button == null)
            button = GetComponent<Button>();

        button.onClick.RemoveAllListeners(); // 중복 제거
        button.onClick.AddListener(() => onClickCallback(characterName));
    }
}
