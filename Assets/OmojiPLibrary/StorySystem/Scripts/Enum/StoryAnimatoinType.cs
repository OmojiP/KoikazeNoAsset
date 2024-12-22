using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmojiP.StorySystem
{
    public enum StoryCharacterAnimationType
    {
        Idle,
        Smile,
        IdleRight,
        IdleLeft,
        AppearFromRight,
        AppearFromLeft,
        DisappearToRight,
        DisappearToLeft,
        Pop,
        GoPyururun,
        KoikazeAppearFadeMaskGrass,
        KoikazeIdleMaskGrass,
        AppearPyururun,
        DisappearPyururun,
        KoikazeIdleMaskGrassNear,
        KoikazeEP1EndRunAway,
        AseriPyururun,
        NormalPyururun,
        NiyaniyaPyururun,
        SmilePyururun,
        KoikazeAppearMaskGrassNear,
        KoikazeAppearMaskNear,
        KoikazeSurprise,
        KoikazeAtozusari,
        KoikazeAtozusariSurprise,
        KoikazeTeretere,
        KoikazeAppearNoMaskNear,
        EP2EndMagao,
        EP2EndNikoniko,
        EP2EndWhite,
        KoikazeAppearFadeMask,
        KoikazeKyoton,
        EP3EndSkirtStart,
        EP3EndTere,
        EP3EndWhite,
        KoikazeSmile,
    }
    public enum StoryTextAnimationType
    {
        Idle,
        Shake,
    }
    public enum StoryBoxAnimationType
    {
        Idle,
        Shake,
        UnActive,
        Active,
    }

    public static class StoryAnimationTypeConstants
    {
        private const string charaStateIdle = "Idle";
        private const string charaStateSmile = "Smile";
        private const string charaStateIdleRight = "IdleRight";
        private const string charaStateIdleLeft = "IdleLeft";
        private const string charaStateAppearFromRight = "AppearFromRight";
        private const string charaStateAppearFromLeft = "AppearFromLeft";
        private const string charaStateDisappearFromRight = "DisappearToRight";
        private const string charaStateDisappearFromLeft = "DisappearToLeft";
        private const string charaStatePop = "Pop";
        private const string charaStateGoPyururun = "GoPyururun";
        private const string charaStateKoikazeAppearFadeMaskGrass = "AppearFadeMaskGrass";
        private const string charaStateKoikazeIdleMaskGrass = "IdleMaskGrass";
        private const string charaStateAppearPyururun = "AppearPyururun";
        private const string charaStateDisappearPyururun = "DisappearPyururun";
        private const string charaStateKoikazeIdleMaskGrassNear = "IdleMaskGrassNear";
        private const string charaStateKoikazeEP1EndRunAway = "RunAway";
        private const string charaStateAseriPyururun = "AseriPyururun";
        private const string charaStateNormalPyururun = "NormalPyururun";
        private const string charaStateNiyaniyaPyururun = "NiyaniyaPyururun";
        private const string charaStateSmilePyururun = "SmilePyururun";
        private const string charaStateKoikazeAppearMaskGrassNear = "AppearMaskGrassNear";
        private const string charaStateKoikazeAppearMaskNear = "AppearMaskNear";
        private const string charaStateKoikazeSurprise = "KoikazeSurprise";
        private const string charaStateKoikazeAtozusari = "KoikazeAtozusari";
        private const string charaStateKoikazeAtozusariSurprise = "KoikazeAtozusariSurprise";
        private const string charaStateKoikazeTeretere = "KoikazeTeretere";
        private const string charaStateKoikazeAppearNoMaskNear = "AppearNoMaskNear";
        private const string charaStateEP2EndMagao = "EP2EndMagao";
        private const string charaStateEP2EndNikoniko = "EP2EndNikoniko";
        private const string charaStateEP2EndWhite = "EP2EndWhite";
        private const string charaStateKoikazeAppearFadeMask = "AppearFadeMask";
        private const string charaStateKoikazeKyoton = "KoikazeKyoton";
        private const string charaStateEP3EndSkirtStart = "EP3EndSkirtStart";
        private const string charaStateEP3EndTere = "EP3EndTere";
        private const string charaStateEP3EndWhite = "EP3EndWhite";
        private const string charaStateKoikazeSmile = "KoikazeSmile";

        private const string textStateIdle = "Idle";
        private const string textStateShake = "Shake";

        private const string boxStateIdle = "Idle";
        private const string boxStateShake = "Shake";
        private const string boxStateUnActive = "UnActive";
        private const string boxStateActive = "Active";

        private const int LAYER_MOVE = 0;
        private const int LAYER_FACE = 1;
        
        private static Dictionary<StoryCharacterAnimationType, (int, int)> charaStateHashDict;
        private static Dictionary<StoryTextAnimationType, int> textStateHashDict;
        private static Dictionary<StoryBoxAnimationType, int> boxStateHashDict;

        public static Dictionary<StoryCharacterAnimationType, (int, int)> CharaStateHashDict
        {
            get
            {
                if (charaStateHashDict == null)
                {
                    charaStateHashDict = new Dictionary<StoryCharacterAnimationType, (int, int)>();
                    
                    charaStateHashDict.Add(StoryCharacterAnimationType.Idle, (Animator.StringToHash(charaStateIdle), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.Smile, (Animator.StringToHash(charaStateSmile), LAYER_FACE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.IdleRight, (Animator.StringToHash(charaStateIdleRight), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.IdleLeft, (Animator.StringToHash(charaStateIdleLeft), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.AppearFromRight, (Animator.StringToHash(charaStateAppearFromRight), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.AppearFromLeft, (Animator.StringToHash(charaStateAppearFromLeft), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.DisappearToRight, (Animator.StringToHash(charaStateDisappearFromRight), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.DisappearToLeft, (Animator.StringToHash(charaStateDisappearFromLeft), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.Pop, (Animator.StringToHash(charaStatePop), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.GoPyururun, (Animator.StringToHash(charaStateGoPyururun), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeAppearFadeMaskGrass, (Animator.StringToHash(charaStateKoikazeAppearFadeMaskGrass), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeIdleMaskGrass, (Animator.StringToHash(charaStateKoikazeIdleMaskGrass), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.AppearPyururun, (Animator.StringToHash(charaStateAppearPyururun), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.DisappearPyururun, (Animator.StringToHash(charaStateDisappearPyururun), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeIdleMaskGrassNear, (Animator.StringToHash(charaStateKoikazeIdleMaskGrassNear), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeEP1EndRunAway, (Animator.StringToHash(charaStateKoikazeEP1EndRunAway), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.AseriPyururun, (Animator.StringToHash(charaStateAseriPyururun), LAYER_FACE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.NormalPyururun, (Animator.StringToHash(charaStateNormalPyururun), LAYER_FACE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.NiyaniyaPyururun, (Animator.StringToHash(charaStateNiyaniyaPyururun), LAYER_FACE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.SmilePyururun, (Animator.StringToHash(charaStateSmilePyururun), LAYER_FACE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeAppearMaskGrassNear, (Animator.StringToHash(charaStateKoikazeAppearMaskGrassNear), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeAppearMaskNear, (Animator.StringToHash(charaStateKoikazeAppearMaskNear), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeSurprise, (Animator.StringToHash(charaStateKoikazeSurprise), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeAtozusari, (Animator.StringToHash(charaStateKoikazeAtozusari), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeAtozusariSurprise, (Animator.StringToHash(charaStateKoikazeAtozusariSurprise), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeTeretere, (Animator.StringToHash(charaStateKoikazeTeretere), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeAppearNoMaskNear, (Animator.StringToHash(charaStateKoikazeAppearNoMaskNear), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.EP2EndMagao, (Animator.StringToHash(charaStateEP2EndMagao), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.EP2EndNikoniko, (Animator.StringToHash(charaStateEP2EndNikoniko), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.EP2EndWhite, (Animator.StringToHash(charaStateEP2EndWhite), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeAppearFadeMask, (Animator.StringToHash(charaStateKoikazeAppearFadeMask), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeKyoton, (Animator.StringToHash(charaStateKoikazeKyoton), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.EP3EndSkirtStart, (Animator.StringToHash(charaStateEP3EndSkirtStart), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.EP3EndTere, (Animator.StringToHash(charaStateEP3EndTere), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.EP3EndWhite, (Animator.StringToHash(charaStateEP3EndWhite), LAYER_MOVE));
                    charaStateHashDict.Add(StoryCharacterAnimationType.KoikazeSmile, (Animator.StringToHash(charaStateKoikazeSmile), LAYER_MOVE));
                }
                
                return charaStateHashDict;
            }
        }

        public static Dictionary<StoryTextAnimationType, int> TextStateHashDict
        {
            get
            {
                if (textStateHashDict == null)
                {
                    textStateHashDict = new Dictionary<StoryTextAnimationType, int>();
                    
                    textStateHashDict.Add(StoryTextAnimationType.Idle, Animator.StringToHash(textStateIdle));
                    textStateHashDict.Add(StoryTextAnimationType.Shake, Animator.StringToHash(textStateShake));
                }
                
                return textStateHashDict;
            }
        }

        public static Dictionary<StoryBoxAnimationType, int> BoxStateHashDict
        {
            get
            {
                if (boxStateHashDict == null)
                {
                    boxStateHashDict = new Dictionary<StoryBoxAnimationType, int>();
                    
                    boxStateHashDict.Add(StoryBoxAnimationType.Idle, Animator.StringToHash(boxStateIdle));
                    boxStateHashDict.Add(StoryBoxAnimationType.Shake, Animator.StringToHash(boxStateShake));
                    boxStateHashDict.Add(StoryBoxAnimationType.UnActive, Animator.StringToHash(boxStateUnActive));
                    boxStateHashDict.Add(StoryBoxAnimationType.Active, Animator.StringToHash(boxStateActive));
                }
                
                return boxStateHashDict;
            }
        }
    }
}