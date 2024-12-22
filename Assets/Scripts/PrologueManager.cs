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
    /// <summary>
    /// stageIdによって流すプロローグを変えて、終わったらInGameへ
    /// </summary>
    public class PrologueManager : MonoBehaviour
    {
        [SerializeField] StoryEpisodeCell[] prologueCells;

        StoryManager storyManager;
        
        void Start()
        {
            storyManager = FindObjectOfType<StoryManager>();
            storyManager.StorySystemSetUp(prologueCells[GameTempDataAgent.stageId]);
            
            StartStory().Forget();
        }

        async UniTaskVoid StartStory()
        {
            await UniTask.WaitUntil(() => !SceneLoader.Instance.IsSceneLoading);
            
            AudioManager.Instance.PlayBgm(AudioManager.Instance.Sounds.bgm_story).Forget();

            storyManager.StorySystemStart(prologueCells[GameTempDataAgent.stageId], () =>
                {
                    AudioManager.Instance.StopBgm(fadeOutTime:1f);
                    SceneLoader.Instance.LoadScene(Scenes.INGAME).Forget();
                })
                .Forget();
        }
        
    }
}