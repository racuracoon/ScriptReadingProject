using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackPanel : MonoBehaviour
{
    public GameObject feedbackPanel;
    public Button closeBtn;
    public FeedbackContainer feedbackContainer;

    void Start()
    {
        closeBtn.onClick.AddListener(OnClickCloseBtn);
    }

    private void OnClickCloseBtn()
    {
        feedbackPanel.SetActive(false);
    }
}
