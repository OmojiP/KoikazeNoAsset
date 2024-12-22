using BlockBishoujo.Constant;
using BlockBishoujo.Manager;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using OmojiP.SceneLoadSystem;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBishoujo.StageSelect
{
    /// <summary>
    /// StageSelectシーンの制御
    /// </summary>
    public class StageSelectManager : MonoBehaviour
    {
        [SerializeField] private GameObject cannotTouchPanel;
        [SerializeField] StageButtonPanel[] stageButtonPanels;
        [SerializeField] private Button backButton;
        [SerializeField] private Button albumButton;
        
        bool isSceneLoadStarted = false;
        bool isSceneReady = false;
        
        void Start()
        {
            isSceneLoadStarted = false;
            isSceneReady = false;

            cannotTouchPanel.SetActive(true);
            
            SceneLoader.Instance.AddWaitMethod(() => isSceneReady);
            
            SetUpScene().Forget();
        }

        async UniTask SetUpScene()
        {
            var isOpenStages = await GameDataManager.GetIsOpenStages();
            var isPlayedUnlocks = await GameDataManager.GetIsStageUnlockAnimationPlayed();
            var isClearStages = await GameDataManager.GetIsStageClears();
            var newUnlockedStageIds = await GameDataManager.GetNewUnlockedStageIds();

            backButton.onClick.AddListener(() =>
            {
                backButton.interactable = false;
                OnBackTitle();
            });
            
            albumButton.onClick.AddListener(() =>
            {
                albumButton.interactable = false;
                GoToAlbum();
            });
            albumButton.gameObject.SetActive(isClearStages[^1]);
            
            for (int i = 0; i < stageButtonPanels.Length; i++)
            {
                int stageId = i;
                stageButtonPanels[i].SetStagePanel( isPlayedUnlocks[i], isClearStages[i] , () => OnStageSelect(stageId));
            }

            // ここでシーンを空ける
            isSceneReady = true;
            
            await UniTask.WaitUntil(() => !SceneLoader.Instance.IsSceneLoading);
            
            AudioManager.Instance.PlayBgm(AudioManager.Instance.Sounds.bgm_stageSelect).Forget();
            
            //アンロックアニメーションがあったら再生する
            if (newUnlockedStageIds is not null && newUnlockedStageIds.Length > 0)
            {
                foreach (var id in newUnlockedStageIds)
                {
                    int stageId = id;
                    await stageButtonPanels[id].PlayUnlockStage(() => OnStageSelect(stageId));
                }

                await GameDataManager.UpdateUnlockAnimationPlayedStages();
            }
            
            
            
            cannotTouchPanel.SetActive(false);

        }

        async UniTask StageUnlockEvent()
        {
            await GameDataManager.GetNewUnlockedStageIds();
        }

        void OnStageSelect(int i)
        {
            Debug.Log($"OnStageSelect{i}");
            
            if(isSceneLoadStarted) return;

            isSceneLoadStarted = true;

            GameTempDataAgent.stageId = i;
            SceneLoader.Instance.LoadScene(Scenes.STAGE_PROLOGUE).Forget();
        }

        void OnBackTitle()
        {
            AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_select2);
            AudioManager.Instance.StopBgm(fadeOutTime:1f);
            SceneLoader.Instance.LoadScene(Scenes.TITLE, loadingMinTime:1f).Forget(); 
        }
        
        void GoToAlbum()
        {
            AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_select2);
            AudioManager.Instance.StopBgm(fadeOutTime:1f);
            SceneLoader.Instance.LoadScene(Scenes.STAGE_ALBUM, loadingMinTime:1f).Forget(); 
        }
    }
}