using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePrefab : MonoBehaviour
{
    public TMP_InputField characterInput;
    public TMP_InputField dialogueInput;
    public Button deleteButton;
    public DialogueListContainer dialogueListContainer;

    void Start()
    {
        deleteButton.onClick.AddListener(OnClickDelete);
    }

    private void OnClickDelete()
    {
        var manager = UnityEngine.Object.FindFirstObjectByType<DialogueListContainer>();

        if(manager.dialogueListContainer.childCount >1){
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

}
