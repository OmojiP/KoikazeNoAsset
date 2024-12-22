using Cysharp.Threading.Tasks;
using OmojiP.SceneLoadSystem;
using UnityEngine;

namespace OmojiP.StorySystem
{
    /// <summary>
    /// ストーリーの再生が終わったらそのまま次のシーンへ移動する
    /// </summary>
    public class StoryEndMoveScene : MonoBehaviour
    {
        [SerializeField] private StoryEpisodeCell episodeCell;

        /// <summary>
        /// あんまいい実装じゃないけど一旦これで、選択肢を自動生成してやりたい
        /// </summary>
        [SerializeField] private string nextSceneName;

        StoryManager storyManager;
        
        void Start()
        {
            storyManager = FindObjectOfType<StoryManager>();
            storyManager.StorySystemSetUp(episodeCell);
            
            StartStory().Forget();
        }

        async UniTaskVoid StartStory()
        {
            await UniTask.WaitUntil(() => !SceneLoader.Instance.IsSceneLoading);

            storyManager.StorySystemStart(episodeCell, () => { SceneLoader.Instance.LoadScene(nextSceneName).Forget(); })
                .Forget();
        }
    }
}