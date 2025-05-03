using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueBoxManager : MonoBehaviour
{
    public TMP_InputField sceneTitleInput;
    public RectTransform dialogueContainer;
    public GameObject dialogueInputPrefab;
    public Button addDialogueBtn;
    public SceneUpdateBtn sceneUpdateBtn;

    private float defaultY = -40f;

    void Start()
    {
        addDialogueBtn.onClick.AddListener(AddDialogueBox);
    }

    public void AddDialogueBox()
    {
        GameObject dialogueBox = Instantiate(dialogueInputPrefab, dialogueContainer);
        RectTransform boxRT = dialogueBox.GetComponent<RectTransform>();

        boxRT.anchorMin = new Vector2(0f, 1f);
        boxRT.anchorMax = new Vector2(0f, 1f);
        boxRT.pivot = new Vector2(0f, 1f);

        float y = -GetTotalDialogueHeight();
        boxRT.anchoredPosition = new Vector2(40f, y);
        Debug.Log($"추가 Y : {y}");
        Debug.Log("박스 추가");

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
        float total = 0f;
        foreach (Transform child in dialogueContainer)
        {
            RectTransform boxRT = child.GetComponent<RectTransform>();
            if (boxRT != null)
            {
                float height = boxRT.rect.height;
                Debug.Log($"높이 : {height}");
                if (height <= 0f) height = 65f;
                total += height;
            }
        }
        return total;
    }

    public void DeleteAllDialogues()
    {
        foreach (Transform child in dialogueContainer)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("삭제");
        sceneTitleInput.text = "";
    }

    public void LoadDialogue(Dialogue dialogue)
    {
        GameObject box = Instantiate(dialogueInputPrefab, dialogueContainer);
        RectTransform boxRT = box.GetComponent<RectTransform>();

        boxRT.anchorMin = new Vector2(0f, 1f);
        boxRT.anchorMax = new Vector2(0f, 1f);
        boxRT.pivot = new Vector2(0f, 1f);

        float y = -GetTotalDialogueHeight();
        boxRT.anchoredPosition = new Vector2(40f, y);

        DialogueInputBox boxScript = box.GetComponent<DialogueInputBox>();
        if (boxScript != null)
        {
            boxScript.characterInput.text = dialogue.character;
            boxScript.dialogueInput.text = dialogue.line;
        }

        RepositionAllDialogueBoxes();
    }

    public void LoadScene(SceneData scene)
    {
        DeleteAllDialogues();

        sceneTitleInput.text = scene.title;

        foreach (var dialogue in scene.dialogues)
        {
            LoadDialogue(dialogue);
        }
        sceneUpdateBtn.getScene(scene);
    }
}
