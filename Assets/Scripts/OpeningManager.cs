using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using OmojiP.SceneLoadSystem;
using OmojiP.StorySystem;
using OmojiP.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBishoujo.Manager
{
    public class OpeningManager : MonoBehaviour
    {
        // 紙ぺらの語りを入れてそれが終わったらストーリー開始

        [SerializeField] Animator soloTalkPanelAnimator;

        [SerializeField] Text soloTalkText;
        [SerializeField] GameObject soloTalkPanel;

        [SerializeField] string[] soloTalkContexts;
        [SerializeField] float soloTalkSpan = 0.5f;
        [SerializeField] float soloTalkCharSpan = 0.05f;
        [SerializeField] float soloTalkAndStorySpan = 1f;

        [SerializeField] private StoryEpisodeCell episodeCell;

        private float talkSpeed = 1;

        private StoryManager storyManager;
        
        void Start()
        {
            StartOpening().Forget();
        }


        async UniTaskVoid StartOpening()
        {
            storyManager = FindObjectOfType<StoryManager>();            
            storyManager.StorySystemSetUp(episodeCell);

            // 画面タップで加速
            soloTalkPanel.GetComponent<OnClickDownEvent>().onClickDown += () => ChangeSoloTalkSpeed(0.5f);
            soloTalkPanel.GetComponent<OnClickUpEvent>().onClickup += () => ChangeSoloTalkSpeed(1);
            
            await UniTask.WaitUntil(() => !SceneLoader.Instance.IsSceneLoading);

            AudioManager.Instance.PlayBgm(AudioManager.Instance.Sounds.bgm_story).Forget();
            
            await GameDataManager.PlayedOpening();
            
            await StartSoloTalk();
            
            await StartStory();
        }

        
        void ChangeSoloTalkSpeed(float speed)
        {
            talkSpeed = speed;
        }

        async UniTask StartSoloTalk()
        {
            // soloTalkPanelAnimator.SetTrigger("Start");

            for (var i = 0; i < soloTalkContexts.Length; i++)
            {
                var context = soloTalkContexts[i];
                foreach (var c in context)
                {
                    soloTalkText.text += c;
                    await UniTask.Delay((int)(soloTalkCharSpan * 1000 * talkSpeed));
                }

                await UniTask.Delay((int)(soloTalkSpan * 1000 * talkSpeed));

                if (i != soloTalkContexts.Length - 1)
                {
                    soloTalkText.text += "\n";
                }
            }

            await UniTask.Delay((int)(soloTalkAndStorySpan * 1000));
            
            await AwaitUtility.WaitUntilPlayAnimationEnd(
                soloTalkPanelAnimator,
                Animator.StringToHash("SoloTalkEnd"),
                0);
        }
        
        async UniTask StartStory()
        {
            await UniTask.Delay(500);
            
            storyManager.StorySystemStart(episodeCell,
                    () =>
                    {
                        AudioManager.Instance.StopBgm(fadeOutTime:1f);
                        SceneLoader.Instance.LoadScene(BlockBishoujo.Constant.Scenes.STAGE_SELECT).Forget();
                    })
                .Forget();
        }
    }
}