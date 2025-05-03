using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ScriptDataHandler
{
    public static void CreateNewScript(string title)
    {
        ScriptMemoryStore.currentScript = new ScriptData
        {
            title = title,
            scenes = new List<SceneData>()
        };

        Debug.Log($"새 스크립트 생성됨: {title}");
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

                if(scene.sceneNumber == updatingScene.sceneNumber)
                {
                    find = true;
                    scene.title = sceneTitle;
                    scene.dialogues = dialogues;
                }
            }
        }
        if(find == false){
            Debug.Log("못찾음");
        }
        else
        {
            Debug.LogWarning("⚠ currentScript나 scenes가 비어있습니다.");
        }
        Debug.Log("수정 완료");
    }


    public static void PrintScript()
    {
        var script = ScriptMemoryStore.currentScript;
        if (script == null)
        {
            Debug.Log("스크립트 없음");
            return;
        }

        Debug.Log($"📘 Script: {script.title} / 씬 수: {script.scenes.Count}");
        foreach (var scene in script.scenes)
        {
            Debug.Log($"  └ Scene #{scene.sceneNumber} - {scene.title} (대사 {scene.dialogues.Count}개)");
        }
    }
}