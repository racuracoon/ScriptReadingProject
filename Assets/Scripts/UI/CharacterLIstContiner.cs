using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class CharacterLIstContiner : MonoBehaviour
{
    public GameObject characterButtonPrefab;
    public RectTransform characterListParent; // 반드시 RectTransform

    public void CreateCharacterBtn()
    {
        Debug.Log("버튼 생성 메서드");
        HashSet<string> characterList = LoadCharacterList();
        ClearContainer();

        float currentY = 0f;
        float buttonHeight = 80f;

        foreach (string character in characterList)
        {
            GameObject characterBtn = Instantiate(characterButtonPrefab, characterListParent);
            TMP_Text text = characterBtn.GetComponentInChildren<TMP_Text>();
            if (text != null)
                text.text = character;

            RectTransform btnRT = characterBtn.GetComponent<RectTransform>();
            btnRT.anchorMin = new Vector2(0f, 1f);
            btnRT.anchorMax = new Vector2(0f, 1f);
            btnRT.pivot = new Vector2(0f, 1f);

            btnRT.anchoredPosition = new Vector2(0f, -currentY);
            btnRT.sizeDelta = new Vector2(600f, buttonHeight);

            currentY += buttonHeight;
        }

        // ✅ 스크롤 활성화를 위한 Content 높이 조정
        characterListParent.sizeDelta = new Vector2(
            characterListParent.sizeDelta.x,
            currentY
        );
    }

    public HashSet<string> LoadCharacterList()
    {
        HashSet<string> characterList = new HashSet<string>();
        Debug.Log(ScriptMemoryStore.currentScript.title);

        foreach (SceneData scene in ScriptMemoryStore.currentScript.scenes)
        {
            if (scene.dialogues.Count == 0)
            {
                Debug.LogWarning($"⚠ scene.dialogues가 비어 있습니다. scene: {scene.title}");
                continue;
            }

            foreach (Dialogue dialogue in scene.dialogues)
            {
                Debug.Log($"추가될 캐릭터 : {dialogue.character}");
                characterList.Add(dialogue.character);
            }
        }

        return characterList;
    }

    private void ClearContainer()
    {
        Debug.Log("화면 초기화");
        foreach (Transform child in characterListParent)
        {
            Destroy(child.gameObject);
        }
    }
}
