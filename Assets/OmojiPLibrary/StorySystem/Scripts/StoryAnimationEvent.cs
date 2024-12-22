using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using UnityEngine;

namespace OmojiP.StorySystem
{
    [CreateAssetMenu(fileName = "New StoryAnimationEvent", menuName = "OmojiP/Story System/Story Event/StoryAnimationEvent")]
    public class StoryAnimationEvent : ScriptableObject, IStoryEvent
    {
        public bool IsAutoEvent { get; } = false;
        
        [SerializeField] private ScriptableObject nextStoryEvent;
        [SerializeField] StoryCharacterCell characterCell;
        
        // プロパティを通して IStoryEvent としてアクセス
        private IStoryEvent NextStoryEvent
        {
            get
            {
                if(nextStoryEvent == null) return null;
                
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
        

        public void SetEvent(IStoryEvent nextStoryEvent, StoryCharacterCell characterCell)
        {
            this.nextStoryEvent = nextStoryEvent as ScriptableObject;
            this.characterCell = characterCell;
        }
        
        public async UniTask StartEvent(MessageBox messageBox)
        {
            //テキスト送りも画面タップでのスキップもこっちで行う
            
            // ボックスの内容の変更 -> MessageBoxで行う
            // キャラクターの表示・アニメーション
            
            AudioManager.Instance.PlaySe(characterCell.seType);

            foreach (var chara in characterCell.characterTypesAndAnimations)
            {
                StoryPrefabs.Instance.storyViewDict[chara.storyViewType].gameObject.SetActive(true);
                StoryPrefabs.Instance.storyViewDict[chara.storyViewType].GetComponent<Animator>()
                    .PlayAnimatorState(StoryAnimationTypeConstants.CharaStateHashDict[chara.characterAnimationType]);

                await UniTask.Yield();
            }
            
            await messageBox.PlayStoryCell(characterCell);
        }

        public IStoryEvent GetNextEvent()
        {
            return NextStoryEvent;
        }
    }
}