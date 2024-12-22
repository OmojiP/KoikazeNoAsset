using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OmojiP.StorySystem;
using UnityEngine;

namespace OmojiP.StorySystem
{
    /// <summary>
    /// シーンチェンジのアニメーションを再生する
    /// </summary>
    [CreateAssetMenu(fileName = "New StorySceneEffectEvent",
        menuName = "OmojiP/Story System/Story Event/StorySceneEffectEvent")]
    public class StorySceneEffectEvent : ScriptableObject, IStoryEvent
    {
        public bool IsAutoEvent { get; } = true;
        
        [SerializeField] private ScriptableObject nextStoryEvent;
        [SerializeField] private StorySceneEffectCell sceneEffectCell;

        // プロパティを通して IStoryEvent としてアクセス
        private IStoryEvent NextStoryEvent
        {
            get
            {
                if (nextStoryEvent == null) return null;

                if (nextStoryEvent is IStoryEvent storyEvent)
                {
                    return storyEvent;
                }
                else
                {
                    Debug.LogError("nextStoryEvent is not an IStoryEvent.");
                    return null;
                }
            }
        }


        public void SetEvent(IStoryEvent nextStoryEvent, StorySceneEffectCell sceneEffectCell)
        {
            this.nextStoryEvent = nextStoryEvent as ScriptableObject;
            this.sceneEffectCell = sceneEffectCell;
        }


        public async UniTask StartEvent(MessageBox messageBox)
        {
            if (sceneEffectCell.isClearMessageBox)
            {
                messageBox.ClearMessage();
            }
            
            await sceneEffectCell.PlayEvent(messageBox);
        }

        public IStoryEvent GetNextEvent()
        {
            return NextStoryEvent;
        }
    }
}