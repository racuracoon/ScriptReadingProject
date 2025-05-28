using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FeedbackData
{
    public string feedbackId;          // 피드백 조회에 사용
    public int lineId;              // 대사
    public List<float> scores;      // 점수
    public string commet;          // 피드백 멘트
}   
