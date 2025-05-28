using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterLIstContiner : MonoBehaviour
{
    public GameObject characterButtonPrefab;
    public RectTransform characterListParent;
    public EnvironmentSetupPanel environmentSetupPanel;

    public void CreateCharacterBtn()
    {
        ClearContainer();

        float currentY = 0f;
        float buttonHeight = 80f;

        foreach (CharacterData character in CharacterMemoryStore.characterList)
        {
            GameObject characterBtn = Instantiate(characterButtonPrefab, characterListParent);
            TMP_Text text = characterBtn.GetComponentInChildren<TMP_Text>();
            if (text != null)
                text.text = character.name;

            Button btn = characterBtn.GetComponentInChildren<Button>();
            if (btn == null)
                Debug.LogWarning("현재 버튼이 null 입니다.");
            else
            {
                btn.onClick.AddListener(() => OnClickCharacterButton(character));
            }

            RectTransform btnRT = characterBtn.GetComponent<RectTransform>();
            btnRT.anchorMin = new Vector2(0f, 1f);
            btnRT.anchorMax = new Vector2(0f, 1f);
            btnRT.pivot = new Vector2(0f, 1f);

            btnRT.anchoredPosition = new Vector2(0f, -currentY);
            btnRT.sizeDelta = new Vector2(600f, buttonHeight);

            currentY += buttonHeight;
        }

        characterListParent.sizeDelta = new Vector2(
            characterListParent.sizeDelta.x,
            currentY
        );
    }

    public void OnClickCharacterButton(CharacterData character)
    {
        Debug.Log($"캐릭터가 클릭됨 {character.name}");
        string avatarUrl = character.avatarUrl;
        LoadAvatorHandler.LoadAvatar(avatarUrl);
        environmentSetupPanel.selectedCharacter = character;
    }

    private void ClearContainer()
    {
        foreach (Transform child in characterListParent)
        {
            Destroy(child.gameObject);
        }
    }

}
