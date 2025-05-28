using UnityEngine;

public class UserAvatarCamera : MonoBehaviour
{
    [Header("눈 위치 경로 (필요 시 수정 가능)")]
    public string leftEyePath = "Renderer_EyeLeft";
    public string rightEyePath = "Renderer_EyeRight";

    [Header("카메라 설정")]
    public float fov = 60f;
    public Vector3 positionOffset = new Vector3(0f, 0f, 0.1f);

    void Start()
    {
        // 눈 트랜스폼 찾기
        Transform leftEye = transform.Find(leftEyePath);
        Transform rightEye = transform.Find(rightEyePath);

        if (leftEye == null || rightEye == null)
        {
            Debug.LogWarning("❌ 눈 위치(eye bone)를 찾지 못했습니다.");
            return;
        }

        // 눈 중앙 계산
        Vector3 eyeCenter = (leftEye.position + rightEye.position) / 2f;
        eyeCenter.y += 1.5f;

        // 카메라 생성
        GameObject camObj = new GameObject("UserFirstPersonCamera");
        Camera cam = camObj.AddComponent<Camera>();
        cam.fieldOfView = fov;

        // 카메라 위치 및 회전
        camObj.transform.position = eyeCenter + positionOffset;
        camObj.transform.rotation = Quaternion.LookRotation(transform.forward);
        camObj.transform.SetParent(transform);

        // 기존 메인 카메라 비활성화
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.enabled = false;

            AudioListener listener = mainCam.GetComponent<AudioListener>();
            if (listener != null)
            {
                listener.enabled = false;
            }
        }

        // 새 카메라에 AudioListener 추가 및 메인 태그 지정
        cam.tag = "MainCamera";
        cam.gameObject.AddComponent<AudioListener>();

        Debug.Log("✅ User 1인칭 카메라 생성 완료");
    }
}