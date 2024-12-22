using UnityEngine;
using UnityEngine.Serialization;

namespace BlockBishoujo.InGame
{
    [CreateAssetMenu(fileName = "InGameCharacter", menuName = "BlockBishoujo Asset/InGameCharacter")]
    public class InGameCharacter : ScriptableObject
    {
        // 状況に合わせてアニメーションを変化させる
        [SerializeField] private string animationNormalState;
        [SerializeField] private string animationClearState;
        [SerializeField] private string animationFailedState;
        [SerializeField] GameObject characterPrefab;
        
        public GameObject CharacterPrefab => characterPrefab;

        // public string GetAnimationState(float existBlockRatio)
        // {
        //     if (Mathf.Abs(existBlockRatio) < 0.0001f)
        //     {
        //         return animationStates[0];
        //     }
        //     
        //     return animationStates[Mathf.CeilToInt(existBlockRatio * (animationStates.Length-1))];
        // }
        
        public string GetAnimationNormalState()
        {
            return animationNormalState;
        }
        
        public string GetAnimationClearState()
        {
            return animationClearState;
        }

        public string GetAnimationFailedState()
        {
            return animationFailedState;
        }

    }
}