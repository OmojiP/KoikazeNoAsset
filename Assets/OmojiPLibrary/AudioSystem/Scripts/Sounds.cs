using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmojiP.AudioSystem
{
    /// <summary>
    /// ここに使用するサウンドを登録する
    /// 増えてきたらロード管理を検討する
    /// </summary>
    public class Sounds : MonoBehaviour
    {
        [Header("ここに使用するサウンドを登録する")] 
        
        [Header("BGM")]
        
        public AudioClip bgm_a;
        public AudioClip bgm_b;
        public AudioClip bgm_c;
        public AudioClip bgm_title;
        public AudioClip bgm_story;
        public AudioClip bgm_play;
        public AudioClip bgm_storySuccess;
        public AudioClip bgm_storyFailed;
        public AudioClip bgm_stageSelect;
        public AudioClip bgm_album;

        [Header("SE")] 
        
        public AudioClip se_a;
        public AudioClip se_b;
        public AudioClip se_c;
        public AudioClip se_hyun;
        public AudioClip se_punch;
        public AudioClip se_breakBlock1;
        public AudioClip se_breakBlock2;
        public AudioClip se_titleSelect;
        public AudioClip se_select1;
        public AudioClip se_select2;
        public AudioClip se_pushDown;
        public AudioClip se_pushUp;
        public AudioClip se_pageTurn;
        public AudioClip se_shoot;
        public AudioClip se_gameStart;
        public AudioClip se_gameEnd;
        public AudioClip se_gameSuccess;
        public AudioClip se_gameFail;
        public AudioClip se_stump;
        public AudioClip se_escape;
        public AudioClip se_skirt;
        


        public enum SeType
        {
            NONE,
            SE_HYUN,
            SE_STUMP,
            SE_ESCAPE,
            SE_SKIRT
        }
    }
}