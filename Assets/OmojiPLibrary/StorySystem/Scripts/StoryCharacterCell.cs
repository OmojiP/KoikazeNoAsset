using System;
using System.Collections;
using System.Collections.Generic;
using OmojiP.AudioSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace OmojiP.StorySystem
{
    // ストーリーのキャラクター情報
    [Serializable]
    public class StoryCharacterCell
    {
        public ViewTypeAndCharacterAnimationType[] characterTypesAndAnimations;

        public bool isNamePlateActive = true;
        public string characterName;
        
        [TextArea] public string messageContext;
        
        public StoryTextAnimationType textAnimationType;
        public StoryBoxAnimationType boxAnimationType;

        public Sounds.SeType seType;
        public FontSizeType fontSizeType;
        
        public StoryCharacterCell(ViewTypeAndCharacterAnimationType[] characterTypesAndAnimations , bool isNamePlateActive , string characterName, string messageContext, StoryTextAnimationType textAnimationType , StoryBoxAnimationType boxAnimationType)
        {
            this.characterTypesAndAnimations = characterTypesAndAnimations;
            this.isNamePlateActive = isNamePlateActive;
            this.characterName = characterName;
            this.messageContext = messageContext;
            this.textAnimationType = textAnimationType;
            this.boxAnimationType = boxAnimationType;
        }
    }

    [Serializable]
    public class ViewTypeAndCharacterAnimationType
    {
        public StoryViewType storyViewType;
        public StoryCharacterAnimationType characterAnimationType;

        public ViewTypeAndCharacterAnimationType(StoryViewType storyViewType,
            StoryCharacterAnimationType characterAnimationType)
        {
            this.storyViewType = storyViewType;
            this.characterAnimationType = characterAnimationType;
        }
    }

    public enum FontSizeType
    {
        Regular = 38,
        Big = 50,
        Small = 30,
    }
}

