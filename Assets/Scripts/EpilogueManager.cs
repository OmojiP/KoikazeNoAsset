using System.Collections;
using System.Collections.Generic;
using BlockBishoujo.Constant;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using OmojiP.SceneLoadSystem;
using OmojiP.StorySystem;
using UnityEngine;

namespace BlockBishoujo.Manager
{
    public class EpilogueManager : MonoBehaviour
    {
        [SerializeField] StoryEpisodeCell[] successEpisodeCells;
        [SerializeField] StoryEpisodeCell[] failedEpisodeCells;

        AudioClip audioClip;
        StoryEpisodeCell episodeCell;

        StoryManager storyManager;

        private string nextScene;

        void Start()
        {
            if (GameTempDataAgent.isStageCleared)
            {
                episodeCell = successEpisodeCells[GameTempDataAgent.stageId];
                audioClip = AudioManager.Instance.Sounds.bgm_storySuccess;
            }
            else
            {
                episodeCell = failedEpisodeCells[GameTempDataAgent.stageId];
                audioClip = AudioManager.Instance.Sounds.bgm_storyFailed;
            }

            nextScene = (GameTempDataAgent.stageId == 2 && GameTempDataAgent.isStageCleared) ? Scenes.ENDING : Scenes.STAGE_SELECT;

            storyManager = FindObjectOfType<StoryManager>();
            storyManager.StorySystemSetUp(episodeCell);

            StartStory().Forget();
        }

        async UniTaskVoid StartStory()
        {
            await UniTask.WaitUntil(() => !SceneLoader.Instance.IsSceneLoading);

            if (GameTempDataAgent.isStageCleared && GameTempDataAgent.stageId == 2)
            {
                
            }
            else
            {
                AudioManager.Instance.PlayBgm(audioClip).Forget();
            }
            
            
            storyManager.StorySystemStart(episodeCell, () =>
                    {
                        AudioManager.Instance.StopBgm(fadeOutTime:1f);
                        SceneLoader.Instance.LoadScene(nextScene).Forget();
                    })
                .Forget();
        }
    }
}