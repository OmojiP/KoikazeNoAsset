using BlockBishoujo.Constant;
using BlockBishoujo.Manager;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using OmojiP.SceneLoadSystem;
using OmojiP.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBishoujo.InGame
{
    /// <summary>
    /// InGameシーンの流れを制御する
    /// </summary>
    public class InGameManager : MonoBehaviour
    {
        /*

        １．シーン遷移の終了を待つ
        ２．ミッションを表示
        ３．ゲームの開始
        ４．リザルト(成功, 失敗)の表示
        ５．シーン遷移

        */

        [Header("UI")]
        [SerializeField] private Animator missionPanelAnimator;
        [SerializeField] private InGameHelp inGameHelp;
        [SerializeField] private Animator countDownPanelAnimator;
        [SerializeField] private Animator gameEndPanelAnimator;
        // [SerializeField] private Animator gameResultPanelAnimator;

        [SerializeField] private Image backGroundImage;
        [SerializeField] private Transform characterParent;
        [SerializeField] private Text timerText;
        
        [SerializeField] private Image missionInGameImage;
        // [SerializeField] private Text missionText;
        [SerializeField] private Image missionImage;
        [SerializeField] private OnClickDownEvent clearFailedPanel;

        [Header("Data")] 
        [SerializeField] InGameStageData[] inGameStageDatas;
        private InGameStageData currentInGameStageData;
        [SerializeField] private GridBlockGenerator gridBlockGenerator;
        
        private BlocksManager blocksManager;
        private GameTimer gameTimer;
        
        private BallShooter ballShooter;

        private bool isGameEnded;

        private CharacterManager characterManager;
        
        
        void Start()
        {
            currentInGameStageData = inGameStageDatas[GameTempDataAgent.stageId];
            
            ballShooter = FindObjectOfType<BallShooter>();
            
            // ミッション設定
            missionImage.sprite = currentInGameStageData.missionSprite;
            // missionText.text = currentInGameStageData.missionName;
            missionInGameImage.sprite = currentInGameStageData.missionInGameSprite;
            
            // 背景の設定
            backGroundImage.sprite = currentInGameStageData.backGroundSprite;
            
            // ブロックの生成
            var blocks = gridBlockGenerator.GenerateGrid(currentInGameStageData.gridBlockData);

            blocksManager = new BlocksManager(blocks);
            blocksManager.onBlockUpdate += OnBlockCountUpdated;

            // キャラクターの配置
            characterManager = new CharacterManager(blocksManager, currentInGameStageData.character, characterParent);
            
            isGameEnded = false;
            
            clearFailedPanel.onClickDown += () =>
            {
                AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_select1);
                SceneLoader.Instance.LoadScene(Scenes.STAGE_EPILOGUE).Forget();
            };
            
            
            //// ミッション、ゲーム、リザルトを初期化
            
            // 矢印、タイマー
            gameTimer = new GameTimer(timerText, currentInGameStageData.timeLimit, () => { GameEnd(false).Forget(); });
            
            // シーン遷移の終了を待機
            InGameStart().Forget();
        }

        async UniTaskVoid InGameStart()
        {
            await UniTask.WaitUntil(() => !SceneLoader.Instance.IsSceneLoading);
            
            AudioManager.Instance.PlayBgm(AudioManager.Instance.Sounds.bgm_play).Forget();

            if (!(await GameDataManager.GetIsStageClears())[0])
            {
                await inGameHelp.PlayHelp();
            }
            
            // ミッションがあったら再生する
            // await WaitableAnimatorPlayer.Play(missionPanelAnimator, "Mission");
            await AwaitUtility.WaitUntilPlayAnimationEnd(missionPanelAnimator, Animator.StringToHash("Mission"), 0);
            
            // ゲームのカウントダウン
            await AwaitUtility.WaitUntilPlayAnimationEnd(countDownPanelAnimator, Animator.StringToHash("CountDown"), 0);
            // await WaitableAnimatorPlayer.Play(countDownPanelAnimator, "CountDown");
            
            // ゲームの開始
            ballShooter.GameStart();
            gameTimer.TimerStart(destroyCancellationToken).Forget();
        }
        
        private void OnBlockCountUpdated(int blockCount)
        {
            if (blockCount <= 0)
            {
                GameEnd(true).Forget();
            }
        }
        

        async UniTask GameEnd(bool isGameCleared)
        {
            if(isGameEnded) return;

            ballShooter.GameEnd();
            characterManager.OnGameEnd(isGameCleared);
            
            isGameEnded = true;

            AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_gameEnd);
            gameTimer.TimerStop();
            blocksManager.GameEnd();
            
            // Finishを表示して、ゲームをスローにして、ちょっと経ったらゲームクリア/失敗演出
            
            // ゲーム終了演出
            
            await AwaitUtility.ChangeTimeScale(0.5f, 0.2f);
            
            await UniTask.Delay(500);
            
            AudioManager.Instance.StopBgm(fadeOutTime: 1f);
            await UniTask.Delay(500);
            
            // await WaitableAnimatorPlayer.Play(gameEndPanelAnimator, "Finish");
            await AwaitUtility.WaitUntilPlayAnimationEnd(gameEndPanelAnimator, Animator.StringToHash("Finish"), 0);
            
            await AwaitUtility.ChangeTimeScale(1f, 0.6f);
            
            if (isGameCleared)
            {
                GameTempDataAgent.isStageCleared = true;
                
                await GameDataManager.ClearStage(GameTempDataAgent.stageId);
                // ゲームクリア演出
                AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_gameSuccess);
                
                // await WaitableAnimatorPlayer.Play(missionPanelAnimator, "MissionClear");
                await AwaitUtility.WaitUntilPlayAnimationEnd(missionPanelAnimator, Animator.StringToHash("MissionClear"), 0);
            }
            else
            {
                GameTempDataAgent.isStageCleared = false;
                
                // ゲーム失敗演出
                AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_gameFail);
                // await WaitableAnimatorPlayer.Play(missionPanelAnimator, "MissionFailed");
                await AwaitUtility.WaitUntilPlayAnimationEnd(missionPanelAnimator, Animator.StringToHash("MissionFailed"), 0);
            }
            
        }
        
    }
}