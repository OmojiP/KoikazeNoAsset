using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace BlockBishoujo.Manager
{
    [Serializable]
    public struct GameData
    {
        public bool isPlayedOpening;
        
        public bool[] isOpenStages;
        public bool[] isStageClears;
        public bool[] isStageUnlockAnimationPlayed;

        public GameData(bool isPlayedOpening, bool[] isOpenStages, bool[] isStageClears, bool[] isStageUnlockAnimationPlayed)
        {
            this.isPlayedOpening = isPlayedOpening;
            this.isOpenStages = isOpenStages;
            this.isStageClears = isStageClears;
            this.isStageUnlockAnimationPlayed = isStageUnlockAnimationPlayed;
        }
    }
}