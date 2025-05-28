using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneListContainer : MonoBehaviour
{
    public RectTransform sceneListContainer;
    public GameObject sceneTextPrefab;
    public PanelController panelController;
    public DialogueListContainer dialogueListContainer;

    private float spacing = 70f;
    private float startY = -20f;

    public void LoadSceneTexts()
    {
        ClearAllSceneText();

        if (ScriptMemoryStore.currentScript == null)
        {
            Debug.LogWarning("현재 Script가 비어있습니다.");
            return;
        }

        var scenes = ScriptMemoryStore.currentScript.scenes;
        float currentY = startY;

        for (int i = 0; i < scenes.Count; i++)
        {
            var scene = scenes[i];

            GameObject item = Instantiate(sceneTextPrefab, sceneListContainer);
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
                            OnClickSceneText(scene);
                        });
                    }
                }
            }
            else
            {
                Debug.LogWarning("TMP_Text를 찾을 수 없음");
            }

            currentY -= spacing;
        }

        sceneListContainer.sizeDelta = new Vector2(
            sceneListContainer.sizeDelta.x,
            Mathf.Abs(currentY) + 40f
        );
    }

    public void OnClickSceneText(SceneData scene)
    {
        dialogueListContainer.LoadDialogueList(scene);

        panelController.OpenInputScenePanel();
    }

    public void ClearAllSceneText()
    {
        foreach (Transform child in sceneListContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
