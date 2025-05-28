using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSceneContainer : MonoBehaviour
{
    public GameObject sceneButtonPrefab;
    public RectTransform sceneListContainer;
    public ReadingSetupPanel readingSetupPanel;

    public void LoadSceneBtn()    // 버튼 프리팹 생성 함수 
    {
        readingSetupPanel.ClearAll();
        float currentY = 0f;
        float buttonHeight = 80f;
        List<SceneData> scenes = ScriptMemoryStore.currentScript.scenes;
        for (int i = 0; i < scenes.Count; i++)
        {
            SceneData scene = scenes[i];

            GameObject SceneBtn = Instantiate(sceneButtonPrefab, sceneListContainer);
            TMP_Text text = SceneBtn.GetComponentInChildren<TMP_Text>();
            if (text != null)
                text.text = $"S#{i + 1} {scene.title}";
            Button btn = SceneBtn.GetComponentInChildren<Button>();
            if (btn == null)
                Debug.LogWarning("현재 버튼이 null 입니다.");
            else
            {
                btn.onClick.AddListener(() => OnClickSceneButton(scene));
            }
            RectTransform btnRT = SceneBtn.GetComponent<RectTransform>();
            btnRT.anchorMin = new Vector2(0.5f, 1f);
            btnRT.anchorMax = new Vector2(0.5f, 1f);
            btnRT.pivot = new Vector2(0.5f, 1f);

            btnRT.anchoredPosition = new Vector2(0f, -currentY);
            btnRT.sizeDelta = new Vector2(600f, buttonHeight);

            currentY += buttonHeight;
        }
        sceneListContainer.sizeDelta = new Vector2(
            sceneListContainer.sizeDelta.x,
            currentY
        );
    }

    public void ClearContainer()
    {
        foreach (Transform child in sceneListContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnClickSceneButton(SceneData scene)
    {
        readingSetupPanel.SetSelectedScene(scene);
    }

}
