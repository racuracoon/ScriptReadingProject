using System;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackMemoryStore : MonoBehaviour
{
    private static List<FeedbackData> feedbackList = new List<FeedbackData>();


    // 리스트에 저장
    public static void Add(AudioClip audioClip, int lineId, List<float> scores, string feedbackComment)
    {
        FeedbackData feedback = new FeedbackData();
        feedback.feedbackId = Guid.NewGuid().ToString();
        MicClipStore.Store(lineId, audioClip);
        feedback.lineId = lineId;
        feedback.scores = scores;
        feedback.commet = feedbackComment;

        feedbackList.Add(feedback);
        Debug.Log("피드백 추가됨");
    }

    // 저장된 특정 피드백 반환
    public static FeedbackData GetFeedback(string feedbackID)
    {
        foreach (FeedbackData feedback in feedbackList)
        {
            if (feedback.feedbackId == feedbackID)
                return feedback;
        }
        return null;
    }

    public static List<FeedbackData> GetFeedbackList()
    {
        if (feedbackList != null)
            return feedbackList;
        else
        {
            Debug.Log("feedbackList가 비어있습니다.");
            return null;
        }
    }

    // 저장된 전체 피드백 삭제
    public static void Clear()
    {
        feedbackList.Clear();
        Debug.Log("feedback 리스트 초기화");
    }

}
