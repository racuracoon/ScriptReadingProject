using UnityEngine;
using ReadyPlayerMe.Core;

public static class LoadAvatorHandler
{
    private static AvatarObjectLoader avatarLoader;
    private static GameObject currentAvatar;

    public static void LoadAvatar(string avatarUrl)
    {
        if (currentAvatar != null)
        {
            Object.Destroy(currentAvatar);
            currentAvatar = null;
        }

        avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += OnAvatarLoaded;
        avatarLoader.OnFailed += OnAvatarLoadFailed;
        avatarLoader.LoadAvatar(avatarUrl);
    }

    private static void OnAvatarLoaded(object sender, CompletionEventArgs args)
    {
        GameObject avatar = args.Avatar;
        currentAvatar = avatar;
        
        Vector3 avatarPosition = new Vector3(10010f, 0f, 0f);       // avatarCamera 앞에 위치 시킴
        avatar.transform.position = avatarPosition;                 

        Vector3 lookTarget = new Vector3(10000f, 0f, 0f);           // 카메라를 응시하도록 방향 변경
        avatar.transform.LookAt(lookTarget);

        Animator animator = avatar.GetComponent<Animator>() ?? avatar.AddComponent<Animator>();
        var controller = Resources.Load<RuntimeAnimatorController>("Animations/StandingAnimator");

        if (controller != null)
        {
            animator.runtimeAnimatorController = controller;
            Debug.Log("✅ 아바타 애니메이터 적용 완료");
        }
        else
        {
            Debug.LogError("❌ 애니메이션 컨트롤러를 찾을 수 없습니다.");
        }
    }

    private static void OnAvatarLoadFailed(object sender, FailureEventArgs args)
    {
        Debug.LogError("❌ 아바타 로드 실패 (잘못된 URL): " + args.Message);
    }

    public static void CancelLoad()
    {
        avatarLoader?.Cancel();
    }
}
