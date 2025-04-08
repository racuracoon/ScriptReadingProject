using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DialogueBoxManager : MonoBehaviour
{
    // 대사 프리팹 관리

    public TMP_InputField sceneTitleInput;
    public RectTransform dialogueContainer;           // 프리팹이 추가될 위치
    public GameObject dialogueInputPrefab;          // 프리팹 연결
    public Button addDialogueBtn;

    void Start()
    {
        addDialogueBtn.onClick.AddListener(AddDialogueContainer);
        AddDialogueContainer();
    }

    private float currentDialogueY = -40f;

    public void AddDialogueContainer()
    {
        GameObject dialogueBox = Instantiate(dialogueInputPrefab, dialogueContainer);
        RectTransform boxRT = dialogueBox.GetComponent<RectTransform>();

        boxRT.anchorMin = new Vector2(0f, 1f);
        boxRT.anchorMax = new Vector2(0f, 1f);
        boxRT.pivot = new Vector2(0f, 1f);

        // 프리팹 높이 계산
        float height = boxRT.rect.height;
        if(height <= 0f) height = 65f;
        

        // 위치 설정
        boxRT.anchoredPosition = new Vector2(40f, currentDialogueY);

        // 다음 프리팹 위치 계산
        currentDialogueY -= height;

        dialogueContainer.sizeDelta = new Vector2(
            dialogueContainer.sizeDelta.x,
            Mathf.Abs(currentDialogueY)
        );
    }
}
