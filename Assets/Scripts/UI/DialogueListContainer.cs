using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueListContainer : MonoBehaviour
{
    public RectTransform dialogueListContainer;
    public GameObject dialogueInputPrefab;
    public InputScenePanel inputScenePanel;

    private float defaultY = -40f;

    public void AddDialogueBox()
    {
        GameObject dialogueBox = Instantiate(dialogueInputPrefab, dialogueListContainer);
        RectTransform boxRT = dialogueBox.GetComponent<RectTransform>();

        boxRT.anchorMin = new Vector2(0f, 1f);
        boxRT.anchorMax = new Vector2(0f, 1f);
        boxRT.pivot = new Vector2(0f, 1f);

        float y = -GetTotalDialogueHeight();
        boxRT.anchoredPosition = new Vector2(40f, y);

        RepositionAllDialogueBoxes();

    }

    public void LoadDialogue(Dialogue dialogue)
    {
        GameObject box = Instantiate(dialogueInputPrefab, dialogueListContainer);
        RectTransform boxRT = box.GetComponent<RectTransform>();

        boxRT.anchorMin = new Vector2(0f, 1f);
        boxRT.anchorMax = new Vector2(0f, 1f);
        boxRT.pivot = new Vector2(0f, 1f);

        float y = -GetTotalDialogueHeight();
        boxRT.anchoredPosition = new Vector2(40f, y);

        DialoguePrefab boxScript = box.GetComponent<DialoguePrefab>();
        if (boxScript != null)
        {
            boxScript.characterInput.text = dialogue.character;
            boxScript.dialogueInput.text = dialogue.line;
        }

        RepositionAllDialogueBoxes();
    }

    public void RepositionAllDialogueBoxes()
    {
        float currentY = defaultY;
        foreach (Transform child in dialogueListContainer)
        {
            RectTransform boxRT = child.GetComponent<RectTransform>();
            if (boxRT == null) continue;

            float height = boxRT.rect.height;
            if (height <= 0f) height = 65f;

            boxRT.anchoredPosition = new Vector2(40f, currentY);
            currentY -= height;
        }
        dialogueListContainer.sizeDelta = new Vector2(
            dialogueListContainer.sizeDelta.x,
            Mathf.Abs(currentY)
        );
    }

    private float GetTotalDialogueHeight()
    {
        float total = 0f;
        foreach (Transform child in dialogueListContainer)
        {
            RectTransform boxRT = child.GetComponent<RectTransform>();
            if (boxRT != null)
            {
                float height = boxRT.rect.height;
                if (height <= 0f) height = 65f;
                total += height;
            }
        }
        return total;
    }

    public void ClearAllDialogues()
    {
        foreach (Transform child in dialogueListContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void LoadDialogueList(SceneData scene)
    {
        inputScenePanel.ClearInput();
        foreach (var dialogue in scene.dialogues)
        {
            LoadDialogue(dialogue);
        }
        inputScenePanel.SetUpdatingScene(scene);
    }
}
