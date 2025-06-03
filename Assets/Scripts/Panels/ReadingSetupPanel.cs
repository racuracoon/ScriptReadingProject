using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadingSetupPanel : MonoBehaviour
{
    public Button backToMainBtn;
    public Button playBtn;
    public PanelController panelController;
    public Transform floorTransform;
    public SceneDetailContainer sceneDetailContainer;
    public PlayableCharacterContainer playableCharacterContainer;
    public SelectSceneContainer selectSceneContainer;
    public TTSService ttsService;
    public PlaybackManager playbackManager;
    public UIControllerInPlay uIControllerInPlay;
    public MessagePanel messagePanel;
    private SceneData selectedScene;
    private bool isSceneSelected = false;

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
        if (isSceneSelected)
        {
            if (playableCharacterContainer.isCharacterSelected)
            {
                List<CharacterData> characters = CharacterObjectManager.GetCharacterListByScene(selectedScene);
                playbackManager.SetCurrentScene(selectedScene);
                messagePanel.OpenPanel("리딩 환경을 준비 중입니다.\n잠시만 기다려 주세요.");
                // TTS 요청 먼저
                await ttsService.RequestTTSAsync(selectedScene);

                // 아바타 로딩 완료까지 대기
                await AvatarLoaderOnPlay.SpawnAvatar(characters, floorTransform, ttsService);

                isSceneSelected = false;
                // 패널 닫고 리딩 시작
                ClearAll();
                messagePanel.ClosePanel();
                panelController.CloseAll();
                playbackManager.StartPlay();
            }
            else
            {
                messagePanel.OpenTemporaryPanel("플레이할 역할을 선택하셔야 플레이할 수 있습니다.");
                return;
            }
        }
        else
        {
            messagePanel.OpenTemporaryPanel("플레이할 씬을을 선택하셔야 플레이할 수 있습니다.");
            return;
        }
    }

    public void SetSelectedScene(SceneData scene)
    {
        isSceneSelected = true;
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
