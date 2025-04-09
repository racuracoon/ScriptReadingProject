using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DialogueBoxManager : MonoBehaviour
{
    public TMP_InputField sceneTitleInput;
    public RectTransform dialogueContainer;
    public GameObject dialogueInputPrefab;
    public Button addDialogueBtn;

    private float defaultY = -40f;

    void Start()
    {
        addDialogueBtn.onClick.AddListener(AddDialogueContainer);
        AddDialogueContainer(); // 최초 1개 생성
    }

    public void AddDialogueContainer()
    {
        GameObject dialogueBox = Instantiate(dialogueInputPrefab, dialogueContainer);
        RectTransform boxRT = dialogueBox.GetComponent<RectTransform>();

        boxRT.anchorMin = new Vector2(0f, 1f);
        boxRT.anchorMax = new Vector2(0f, 1f);
        boxRT.pivot = new Vector2(0f, 1f);

        // 높이 기본값
        float height = boxRT.rect.height;
        if (height <= 0f) height = 65f;

        // 위치 설정
        float y = -GetTotalDialogueHeight();
        boxRT.anchoredPosition = new Vector2(40f, y);
        RepositionAllDialogueBoxes();
    }

    public void RepositionAllDialogueBoxes()
    {
        float currentY = defaultY;

        foreach (Transform child in dialogueContainer)
        {
            RectTransform boxRT = child.GetComponent<RectTransform>();
            if (boxRT == null) continue;

            float height = boxRT.rect.height;
            if (height <= 0f) height = 65f;

            boxRT.anchoredPosition = new Vector2(40f, currentY);
            currentY -= height;
        }

        dialogueContainer.sizeDelta = new Vector2(
            dialogueContainer.sizeDelta.x,
            Mathf.Abs(currentY)
        );
    }

    private float GetTotalDialogueHeight()
    {
        float total = defaultY;
        foreach (Transform child in dialogueContainer)
        {
            RectTransform boxRT = child.GetComponent<RectTransform>();
            if (boxRT != null)
            {
                float h = boxRT.rect.height;
                if (h <= 0f) h = 65f;
                total -= h;
            }
        }
        return Mathf.Abs(total);
    }
}