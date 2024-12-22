using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBishoujo.InGame
{
    /// <summary>
    /// ブロックの残りの数や当たった場所、時間に応じてキャラの表情やセリフを切り替える
    /// </summary>
    public class CharacterManager
    {
        // 仮の実装
        InGameCharacter character;
        private Transform characterParent;

        private float blockAllCount;

        Animator characterAnimator;
        
        public CharacterManager(BlocksManager blocksManager, InGameCharacter character, Transform characterParent)
        {
            // blocksManager.onBlockUpdate += OnBlockUpdated;
            blockAllCount = blocksManager.blockAllCount;
            this.character = character;
            this.characterParent = characterParent;
            
            characterAnimator = Object.Instantiate(character.CharacterPrefab, characterParent).GetComponent<Animator>();
            
            characterAnimator.Play(character.GetAnimationNormalState());
        }

        // 残りブロック数のアップデート時に呼ばれる
        private void OnBlockUpdated(int blockCount)
        {
            // if (blockCount <= 0)
            // {
            //     characterAnimator.Play(character.GetAnimationClearState());
            // }
        }

        public void OnGameEnd(bool isClear)
        {
            if (isClear)
            {
                characterAnimator.Play(character.GetAnimationClearState());
            }
            else
            {
                characterAnimator.Play(character.GetAnimationFailedState());
            }
        }
    }
}