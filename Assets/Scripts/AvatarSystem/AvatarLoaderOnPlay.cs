using UnityEngine;
using ReadyPlayerMe.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AvatarLoaderOnPlay
{
    private static float floorY; // floor의 Y 위치 (자동 설정)
    private static Vector3 speakingPosition;

    public static async Task SpawnAvatar(List<CharacterData> characters, Transform floorTransform, TTSService ttsService)
    {
        List<CharacterData> AICharacters = new List<CharacterData>();
        CharacterData UserCharacter = null;

        foreach (CharacterData character in characters)
        {
            if (character.isUser)
                UserCharacter = character;
            else
                AICharacters.Add(character);
        }

        if (UserCharacter == null)
        {
            Debug.LogError("❌ 사용자 캐릭터가 지정되지 않았습니다.");
            return;
        }

        int aiCount = AICharacters.Count;
        Vector3 floorPos = floorTransform.position;
        speakingPosition = floorPos;
        speakingPosition.z -= 1.2f;
        float floorWidth = floorTransform.localScale.x * 10f;
        floorY = floorPos.y;

        Vector3 userPos = new Vector3(floorPos.x, 1.7068f, floorPos.z);

        List<Task> loadTasks = new List<Task>();

        // ✅ 사용자 아바타 로딩 Task
        if (!string.IsNullOrEmpty(UserCharacter.avatarUrl))
        {
            var userTask = LoadAvatarAsync(UserCharacter.avatarUrl, userPos, UserCharacter, ttsService);
            loadTasks.Add(userTask);
        }

        // ✅ AI 아바타 위치 계산 (좌우 균등 분산)
        List<Vector3> positions = GetSpawnPositions(aiCount, floorPos, floorWidth);

        for (int i = 0; i < aiCount; i++)
        {
            CharacterData aiCharacter = AICharacters[i];
            Vector3 spawnPos = positions[i];

            if (string.IsNullOrEmpty(aiCharacter.avatarUrl))
            {
                Debug.LogWarning($"⚠️ [{aiCharacter.name}]의 아바타 URL이 없습니다.");
                continue;
            }

            var aiTask = LoadAvatarAsync(aiCharacter.avatarUrl, spawnPos, aiCharacter, ttsService);
            loadTasks.Add(aiTask);
        }

        // ✅ 모든 아바타 로딩이 끝날 때까지 대기
        await Task.WhenAll(loadTasks);

        Debug.Log("✅ 모든 아바타 로딩 완료");
    }

    private static Task LoadAvatarAsync(string url, Vector3 position, CharacterData characterData, TTSService ttsService)
    {
        var tcs = new TaskCompletionSource<GameObject>();
        var loader = new AvatarObjectLoader();

        loader.OnCompleted += (sender, args) =>
        {
            OnAvatarLoaded(args, position, characterData, ttsService); // ✅ 이거 꼭 필요!!
            tcs.TrySetResult(args.Avatar);
        };

        loader.OnFailed += (sender, args) =>
        {
            Debug.LogError($"❌ 아바타 로드 실패: {args.Message}");
            tcs.TrySetException(new System.Exception(args.Message));
        };

        loader.LoadAvatar(url);
        return tcs.Task;
    }

    private static void OnAvatarLoaded(CompletionEventArgs args, Vector3 basePosition, CharacterData characterData, TTSService ttsService)
    {
        GameObject avatar = args.Avatar;

        SkinnedMeshRenderer renderer = avatar.GetComponentInChildren<SkinnedMeshRenderer>();
        if (renderer == null)
        {
            Debug.LogWarning("❌ SkinnedMeshRenderer가 없습니다.");
            return;
        }

        float avatarBottomY = renderer.bounds.min.y;
        float offsetY = floorY - avatarBottomY;

        Vector3 finalPosition = new Vector3(
            basePosition.x,
            basePosition.y + offsetY,
            basePosition.z
        );

        avatar.transform.position = finalPosition;

        if (characterData.isUser)
        {
            avatar.transform.Rotate(0f, 180f, 0f);
        }

        Debug.Log($"✅ 아바타 로드 완료: {characterData.name}");

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

        if (characterData.isUser)
        {
            avatar.AddComponent<UserAvatarCamera>();
        }
        AudioSource avatarAudio = avatar.GetComponent<AudioSource>();
        if (avatarAudio == null)
        {
            avatarAudio = avatar.AddComponent<AudioSource>();
            avatarAudio.spatialBlend = 1.0f;
            avatarAudio.playOnAwake = false;
        }
        avatar.AddComponent<AvatarBehaviour>();
        characterData.avatar = avatar;
        // 2. CharacterData 주입
        AvatarBehaviour avatarBehaviour = avatar.GetComponent<AvatarBehaviour>();
        avatarBehaviour.Init(avatar, avatarAudio, ttsService, finalPosition);
        avatar.name = characterData.name;
    }

    private static void OnAvatarLoadFailed(object sender, FailureEventArgs args)
    {
        Debug.LogError($"❌ 아바타 로드 실패: {args.Message}");
    }

    private static List<Vector3> GetSpawnPositions(int count, Vector3 floorCenter, float floorWidth)
    {
        List<Vector3> positions = new List<Vector3>();
        float spacing = floorWidth / count;
        if (count == 1)
            spacing = 0;
        float startX;
        if (count % 2 == 0)
        {
            startX = floorCenter.x - ((spacing / 2) + spacing * ((count - 2) / 2));
        }
        else
        {
            startX = floorCenter.x - (spacing * ((count - 1) / 2));
        }
        float z = floorCenter.z - 6f;
        for (int i = 0; i < count; i++)
        {
            float x = startX + i * spacing;
            positions.Add(new Vector3(x, 1.7068f, z));
        }

        return positions;
    }

    public static void MoveAvatarToSpeakingPosition(GameObject avatar)
    {
        avatar.transform.position = speakingPosition;
    }
}
