using TMPro;
using UnityEngine;
using System.Collections;

public class MessagePanel : MonoBehaviour
{
    public GameObject messagePanel;
    public TMP_Text commentText;

    public void Start()
    {
        messagePanel.SetActive(false);
    }

    public void OpenPanel(string comment)
    {
        commentText.text = comment;
        messagePanel.SetActive(true);
    }
    public void ClosePanel()
    {
        messagePanel.SetActive(false);
    }
    public void OpenTemporaryPanel(string comment)
    {
        OpenPanel(comment);
        StartCoroutine(ShowTemporaryMessage());
    }

    private IEnumerator ShowTemporaryMessage()
    {
        yield return new WaitForSeconds(1.5f);
        ClosePanel();
    }
}
