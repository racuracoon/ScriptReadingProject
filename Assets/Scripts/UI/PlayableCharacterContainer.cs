using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayableCharacterContainer : MonoBehaviour
{
    public RectTransform playableCharacterContainer;
    public GameObject playableCharacterPrefab;

    private float startX = 0;
    private float buttonWidth = 320f;
    private List<Button> allPlayabelCharacterBtn = new List<Button>();
    private string userCharacter;
    private Color normalColor = Color.white;
    private Color selectedColor = new Color(1f, 0.8f, 0.2f);
    public bool isCharacterSelected = false;

    public void LoadPlayableCharacterBtn(SceneData scene)
    {
        if (playableCharacterPrefab == null)
        {
            Debug.LogError($"❌ [NULL] playableCharacterPrefab is missing on object: {gameObject.name}");
            return;
        }
        ClearContainer();

        float currentX = startX;
        List<CharacterData> characters = CharacterObjectManager.GetCharacterListByScene(scene);

        allPlayabelCharacterBtn.Clear();

        foreach (CharacterData character in characters)
        {
            GameObject playableCharacterBtn = Instantiate(playableCharacterPrefab, playableCharacterContainer);
            TMP_Text text = playableCharacterBtn.GetComponentInChildren<TMP_Text>();
            if (text != null)
                text.text = character.name;
            Button btn = playableCharacterBtn.GetComponentInChildren<Button>();
            if (btn == null)
                Debug.Log("현재 버튼이 null 입니다.");
            else
            {
                allPlayabelCharacterBtn.Add(btn);
                var capturedBtn = btn;

                btn.onClick.AddListener(() => OnClickplayableCharacterBtn(character, capturedBtn));
            }
            RectTransform btnRT = playableCharacterBtn.GetComponent<RectTransform>();
            btnRT.anchorMin = new Vector2(0f, 0.5f);
            btnRT.anchorMax = new Vector2(0f, 0.5f);
            btnRT.pivot = new Vector2(0f, 0.5f);

            btnRT.anchoredPosition = new Vector2(currentX, 0f);
            btnRT.sizeDelta = new Vector2(buttonWidth, 0f);

            currentX += buttonWidth;
        }

        playableCharacterContainer.sizeDelta = new Vector2(
            currentX,
            playableCharacterContainer.sizeDelta.y
        );
    }

    public void OnClickplayableCharacterBtn(CharacterData selectedCharacter, Button selectedBtn)
    {
        if (userCharacter == null || userCharacter != selectedCharacter.name)
        {
            foreach (CharacterData character in CharacterMemoryStore.characterList)
            {
                character.isUser = false;
            }
            foreach (Button btn in allPlayabelCharacterBtn)
            {
                SetButtonColor(btn, normalColor);
            }
            isCharacterSelected = true;
            selectedCharacter.isUser = true;
            SetButtonColor(selectedBtn, selectedColor);
            userCharacter = selectedCharacter.name;

        }
        else if (userCharacter == selectedCharacter.name)
        {
            isCharacterSelected = false;
            selectedCharacter.isUser = false;
            userCharacter = null;
            SetButtonColor(selectedBtn, normalColor);
        }
    }

    public void SetButtonColor(Button btn, Color color)
    {
        var colors = btn.colors;
        colors.normalColor = color;
        btn.colors = colors;

        Image img = btn.GetComponent<Image>();
        if (img != null)
            img.color = color;
    }

    public void ClearContainer()
    {
        foreach (Transform child in playableCharacterContainer)
        {
            Destroy(child.gameObject);
        }
        isCharacterSelected = false;
    }
}
