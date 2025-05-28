using System.Collections.Generic;
using UnityEngine;

public class ScriptManager
{
    public static void CreateScript(string title)
    {
        ScriptMemoryStore.currentScript = new ScriptData
        {
            title = title,
            scenes = new List<SceneData>()
        };

        Debug.Log($"새 스크립트 생성됨: {title}");
    }

    public static void SaveScript(string title)
    {
        if (ScriptMemoryStore.currentScript != null && ScriptMemoryStore.currentScript.scenes != null)
        {
            ScriptMemoryStore.currentScript.title = title;
            Debug.Log("스크립트가 성공적으로 저장됨");
            Debug.Log($"제목 : {ScriptMemoryStore.currentScript.title}");
        }
        else
        {
            Debug.Log("저장 실패");
        }
    }

    // 현재 스크립트에 씬 추가
    public static void AddScene(SceneData scene)
    {
        if (ScriptMemoryStore.currentScript == null)
        {
            Debug.LogWarning("currentScript가 비어있습니다. 먼저 CreateNewScript를 호출하세요.");
            return;
        }

        scene.sceneNumber = ScriptMemoryStore.currentScript.scenes.Count + 1;
        ScriptMemoryStore.currentScript.scenes.Add(scene);

        Debug.Log($"✅ 씬 #{scene.sceneNumber} 추가됨: {scene.title}");
    }

    public static void UpdateScene(SceneData updatingScene, string sceneTitle, List<Dialogue> dialogues)
    {
        if (ScriptMemoryStore.currentScript == null || updatingScene == null)
        {
            Debug.LogWarning("currentScript나 씬이이 비어있습니다.");
            return;
        }
        bool find = false;
        if (ScriptMemoryStore.currentScript != null && ScriptMemoryStore.currentScript.scenes != null)
        {
            foreach (var scene in ScriptMemoryStore.currentScript.scenes)
            {
                Debug.Log($"메모리:{scene.sceneNumber}");
                Debug.Log($"업데이트 씬 : {updatingScene.sceneNumber}");

                if (scene.sceneNumber == updatingScene.sceneNumber)
                {
                    find = true;
                    scene.title = sceneTitle;
                    scene.dialogues = dialogues;
                }
            }
        }
        if (find == false)
        {
            Debug.LogWarning("⚠ Scene번호를 찾을 수 없습니다.");
        }
        else
        {
            Debug.LogWarning("⚠ currentScript나 scenes가 비어있습니다.");
        }
        DialogueManager.GrantDialogueId(updatingScene);
        Debug.Log("수정 완료");
    }

    public static SceneData GetScene(int sceneNumber)
    {
        foreach (SceneData scene in ScriptMemoryStore.currentScript.scenes)
        {
            if (scene.sceneNumber == sceneNumber)
            {
                return scene;
            }
        }
        Debug.LogWarning($"sceneNumber {sceneNumber}에 해당하는 씬을 찾을 수 없습니다.");
        return null;
    }
}
