using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmojiP.StorySystem
{
    public static class StoryAnimationUtil
    {
        public static void PlayAnimatorState(this Animator animator, (int, int) stateHashAndLayerIndex)
        {
            Debug.Log(stateHashAndLayerIndex);
            
            animator.Play(stateHashAndLayerIndex.Item1, stateHashAndLayerIndex.Item2, 0f);
        }
    }
}