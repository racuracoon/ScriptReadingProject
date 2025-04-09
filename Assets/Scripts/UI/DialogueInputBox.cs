using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueInputBox : MonoBehaviour
{
    public TMP_InputField characterInput;
    public TMP_InputField dialogueInput;
    public Button deleteButton;

    void Start()
    {
        deleteButton.onClick.AddListener(OnClickDelete);
    }

    private void OnClickDelete()
    {
        var manager = UnityEngine.Object.FindFirstObjectByType<DialogueBoxManager>();

        if(manager.dialogueContainer.childCount >1){
            Destroy(gameObject); 
        }
        else
        {
            Debug.Log("대사가 최소 하나는 있어야 합니다.");
            return;
        }

        if (manager != null)
        {
            manager.Invoke("RepositionAllDialogueBoxes", 0.005f); // 매니저에서 딜레이 호출
        }
    }

    private void RefreshLayout()
    {
        Debug.Log("레이아웃 조정");
        var manager = UnityEngine.Object.FindFirstObjectByType<DialogueBoxManager>();
        if (manager != null)
        {
            manager.RepositionAllDialogueBoxes();
        }
        else
        {
            Debug.LogWarning("❗ DialogueBoxManager를 찾을 수 없습니다.");
        }
    } 

}
