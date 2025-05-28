using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadingSetupPanel : MonoBehaviour
{
    public Button backToMainBtn;
    public Button playBtn;
    private SceneData selectedScene;
    public PanelController panelController;
    public Transform floorTransform;
    public SceneDetailContainer sceneDetailContainer;
    public PlayableCharacterContainer playableCharacterContainer;
    public SelectSceneContainer selectSceneContainer;
    public TTSService ttsService;
    public PlaybackManager playbackManager;
    public UIControllerInPlay uIControllerInPlay;

    public void Start()
    {
        backToMainBtn.onClick.AddListener(OnClickBackToMainBtn);
        playBtn.onClick.AddListener(OnClickPlayBtn);
    }

    private void OnClickBackToMainBtn()
    {
        ClearAll();
        panelController.OpenMainPanel();
    }

    private async void OnClickPlayBtn()
    {
        ClearAll();
        if (selectedScene != null)
        {
            List<CharacterData> characters = CharacterObjectManager.GetCharacterListByScene(selectedScene);
            playbackManager.SetCurrentScene(selectedScene);

            // TTS 요청 먼저
            await ttsService.RequestTTSAsync(selectedScene);

            // 아바타 로딩 완료까지 대기
            await AvatarLoaderOnPlay.SpawnAvatar(characters, floorTransform, ttsService);

            // 패널 닫고 리딩 시작작
            panelController.CloseAll();
            playbackManager.StartPlay();
        }
        else
        {
            Debug.Log("플레이할 캐릭터를 선택하셔야 플레이할 수 있습니다.");
        }
    }

    public void SetSelectedScene(SceneData scene)
    {
        selectedScene = scene;
        sceneDetailContainer.LoadSceneDetail(selectedScene);
        playableCharacterContainer.LoadPlayableCharacterBtn(selectedScene);
    }

    public void ClearAll()
    {
        sceneDetailContainer.ClearContainer();
        playableCharacterContainer.ClearContainer();
        selectSceneContainer.ClearContainer();
    }
}
