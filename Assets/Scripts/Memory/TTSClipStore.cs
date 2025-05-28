using System.Collections.Generic;
using UnityEngine;

// TTS로 생성된 AudioClip을 lineId 기준으로 캐싱하는 저장소
public static class TTSClipStore
{
    private static readonly Dictionary<int, AudioClip> clipCache = new Dictionary<int, AudioClip>();

    // lineId에 대응되는 AudioClip을 저장 (항상 덮어쓰기)
    public static void Store(int lineId, AudioClip clip)
    {
        if (clip == null)
            return;

        clipCache[lineId] = clip; 
    }

    // lineId에 대응되는 AudioClip 반환 (없으면 null)
    public static AudioClip Get(int lineId)
    {
        return clipCache.TryGetValue(lineId, out var clip) ? clip : null;
    }

    // 해당 lineId에 대해 Clip이 캐싱돼 있는지 확인
    public static bool Contains(int lineId)
    {
        return clipCache.ContainsKey(lineId);
    }

    // 전체 캐시 초기화
    public static void Clear()
    {
        clipCache.Clear();
    }
}
