using System.Linq;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using OmojiP.SceneLoadSystem;
using OmojiP.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBishoujo.Manager
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] private GameObject cannotTouchPanel;

        [SerializeField] private Image koikazeImage;
        [SerializeField] Sprite[] koikazeSprites;
        
        [SerializeField] OnClickDownEvent startScreen;
        [SerializeField] Button resetButton;

        bool isGameStarted = false;

        private bool isPlayedOpening = false;

        
        void Start()
        {
            isGameStarted = false;
            
            cannotTouchPanel.SetActive(true);
            
            startScreen.onClickDown += OnClickStart;
            resetButton.onClick.AddListener(() => OnClickResetData().Forget());

            Application.targetFrameRate = 60;
            
            DataLoad().Forget();
        }
        
        async UniTask DataLoad()
        {
            isPlayedOpening = await GameDataManager.GetIsPlayedOpening();
            cannotTouchPanel.SetActive(false);
            
            // ゲームのクリア度合いに応じて画像を変更
            var isStageClears = await GameDataManager.GetIsStageClears();
            int clearCount = isStageClears.Count(x => x);
            int spriteIndex = (clearCount >= koikazeSprites.Length) ?  koikazeSprites.Length - 1 : clearCount;
            koikazeImage.sprite = koikazeSprites[spriteIndex];
            Debug.Log($"clearCount: {clearCount}, spriteIndex: {spriteIndex}");
            
            await UniTask.WaitUntil(() => !SceneLoader.Instance.IsSceneLoading);
            
            AudioManager.Instance.PlayBgm(AudioManager.Instance.Sounds.bgm_title).Forget();
        }

        void LoadStart()
        {
            //Openingをまだ見ていない場合Openingへ
            if (isPlayedOpening)
            {
                SceneLoader.Instance.LoadScene(BlockBishoujo.Constant.Scenes.STAGE_SELECT, loadingMinTime:1f).Forget();
            }
            else
            {
                SceneLoader.Instance.LoadScene(BlockBishoujo.Constant.Scenes.OPENING, loadingMinTime:1f).Forget(); 
            }
            
        }

        private void OnClickStart()
        {
            if (isGameStarted) return;

            isGameStarted = true;
            
            AudioManager.Instance.StopBgm(fadeOutTime:1f);
            AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_titleSelect);

            LoadStart();
        }

        private async UniTaskVoid OnClickResetData()
        {
            AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_select2);
            cannotTouchPanel.SetActive(true);
            await GameDataManager.ResetAndSaveData();
            isPlayedOpening = false;
            cannotTouchPanel.SetActive(false);
        }
    }
}