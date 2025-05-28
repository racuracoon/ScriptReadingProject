using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneDetailContainer : MonoBehaviour
{
    public RectTransform sceneListContainer;
    public GameObject dialogueTextPrefab;

    private float spacing = 40;
    private float startY = 10;

    public void LoadSceneDetail(SceneData scene)
    {
        ClearContainer();
        float currentY = startY;
        foreach (Dialogue dialogue in scene.dialogues)
        {
            GameObject item = Instantiate(dialogueTextPrefab, sceneListContainer);
            RectTransform boxRT = item.GetComponent<RectTransform>();

            boxRT.anchorMin = new Vector2(0.5f, 1f);
            boxRT.anchorMax = new Vector2(0.5f, 1f);
            boxRT.pivot = new Vector2(0.5f, 1f);
            currentY += spacing;            
            boxRT.anchoredPosition = new Vector2(0f, -currentY);
            sceneListContainer.sizeDelta = new Vector2(
                boxRT.sizeDelta.x,
                currentY
            );

            DialogueTextPrefab itemText = item.GetComponent<DialogueTextPrefab>();
            if (itemText != null)
            {
                itemText.characterText.text = dialogue.character;
                itemText.lineText.text = dialogue.line;
            }
        }
    }

    public void ClearContainer()
    {
        foreach (Transform child in sceneListContainer)
        {
            Destroy(child.gameObject);
        }
    }
}