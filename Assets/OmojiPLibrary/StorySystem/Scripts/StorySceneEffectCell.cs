using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using UnityEngine;


namespace OmojiP.StorySystem
{
    // ストーリーのキャラクター情報
    [Serializable]
    public class StorySceneEffectCell
    {
        [SerializeField] private string inAnimationStateName;
        [SerializeField] private string outAnimationStateName;
        [SerializeField] private float awaitStartTime;
        [SerializeField] private float awaitMidTime;
        [SerializeField] private float awaitEndTime;
        [SerializeField] public bool isClearMessageBox;
        [SerializeField] private Sprite backgroundSprite;
        [SerializeField] private StoryCharacterCell characterCell;


        public async UniTask PlayEvent(MessageBox messageBox)
        {
            await UniTask.Delay((int)(1000 * awaitStartTime));
         
            AudioManager.Instance.PlaySe(characterCell.seType);
            
            // シーンチェンジと背景変更
            await StoryPrefabs.Instance.SceneChangeAnimate(inAnimationStateName);

            StoryPrefabs.Instance.ChangeBackground(backgroundSprite);
            
            messageBox.PlayMessageBoxAnim(characterCell);
            
            foreach (var chara in characterCell.characterTypesAndAnimations)
            {
                StoryPrefabs.Instance.storyViewDict[chara.storyViewType].gameObject.SetActive(true);
                StoryPrefabs.Instance.storyViewDict[chara.storyViewType].GetComponent<Animator>()
                    .PlayAnimatorState(StoryAnimationTypeConstants.CharaStateHashDict[chara.characterAnimationType]);
                
                await UniTask.Yield();
            }
            
            await UniTask.Delay((int)(1000 * awaitMidTime));
            
            await StoryPrefabs.Instance.SceneChangeAnimate(outAnimationStateName);
            
            await UniTask.Delay((int)(1000 * awaitEndTime));

        }
    }
}