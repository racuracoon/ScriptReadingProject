using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneViewer : MonoBehaviour
{
    public RectTransform sceneContainer;
    public GameObject sceneTextPrefab;
    public DialogueBoxManager dialogueBoxManager;
    public PanelController panelController;

    private float spacing = 70f;
    private float startY = -20f;

    public void DisplayScenes()
    {
        ClearDisplay();

        if (ScriptMemoryStore.currentScript == null)
        {
            Debug.LogWarning("currentScript가 null임");
            return;
        }

        var scenes = ScriptMemoryStore.currentScript.scenes;
        float currentY = startY;

        for (int i = 0; i < scenes.Count; i++)
        {
            var scene = scenes[i];
            int index = i;

            GameObject item = Instantiate(sceneTextPrefab, sceneContainer);
            RectTransform boxRT = item.GetComponent<RectTransform>();

            boxRT.anchorMin = new Vector2(0f, 1f);
            boxRT.anchorMax = new Vector2(0f, 1f);
            boxRT.pivot = new Vector2(0f, 1f);

            boxRT.anchoredPosition = new Vector2(400f, currentY);
            boxRT.sizeDelta = new Vector2(600f, 60f);

            TMP_Text text = item.GetComponentInChildren<TMP_Text>();
            if (text != null)
            {
                text.text = $"S#{i + 1} {scene.title}";
                Transform target = item.transform.Find("SceneTextItem");
                if (target != null)
                {
                    Button btn = target.GetComponent<Button>();
                    if (btn != null)
                    {
                        btn.onClick.AddListener(() =>
                        {
                            panelController.OpenUpdateScenePanel(scenes[index]);
                        });
                    }
                }
            }
            else
            {
                Debug.LogWarning("TMP_Text를 찾을 수 없음");
            }

            Debug.Log($"프리팹 배치됨: {scene.title} @ {boxRT.anchoredPosition}, size: {boxRT.sizeDelta}");

            currentY -= spacing;
        }

        sceneContainer.sizeDelta = new Vector2(
            sceneContainer.sizeDelta.x,
            Mathf.Abs(currentY) + 40f
        );
    }

    public void ClearDisplay()
    {
        foreach (Transform child in sceneContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
