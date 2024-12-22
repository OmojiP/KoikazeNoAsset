using System;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using OmojiP.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OmojiP.StorySystem
{
    /// <summary>
    /// StorySystemの起点
    /// </summary>
    public class StoryManager : MonoBehaviour
    {
        private MessageBox messageBox;
        [SerializeField] private OnClickDownEvent onClickDownEvent;

        [SerializeField] Canvas storyCanvas;
        
        [Header("Episode Panel")]
        [SerializeField] private Text episodeNumberText;
        [SerializeField] private Text episodeNameText;
        
        [Header("Message Box")]
        [SerializeField] private Animator boxAnimator;
        [SerializeField] private Text charaNameText;
        [SerializeField] private TextMeshProUGUI messageTextTMP;
        [SerializeField] private GameObject namePlateObj;
        [SerializeField] private Image nextLeadingImage;

        [Header("StoryViewObj")]
        // StoryViewObjを登録(Animatorのついたキャラクターイラスト等)
        [SerializeField] Transform storyViewParent;
        [SerializeField] StoryViewObj[] storyViewPrefabs;
        
        [Header("StorySceneChange")]
        
        [SerializeField] Transform storySceneChangeParent;
        [SerializeField] GameObject storySceneChangePrefab;
        
        [Header("StoryBackground")]
        
        [SerializeField] Image storyBackgroundImage;
        
        
        // イベント表示中フラグ
        bool isEventing = false;
        // イベント完了待機時の画面タップフラグ
        bool isScreenClickedOnEventEnd = false;
        

        bool isStorySystemSetupComplete = false;
        
        public void StorySystemSetUp(StoryEpisodeCell episodeCell)
        {
            onClickDownEvent.onClickDown += OnScreenClicked;

            storyCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            storyCanvas.worldCamera = Camera.main;
            
            StoryPrefabs.Instance.SetStoryViews(storyViewParent, storyViewPrefabs, storySceneChangeParent, storySceneChangePrefab, storyBackgroundImage);

            messageBox = new MessageBox(boxAnimator, charaNameText, messageTextTMP, namePlateObj, nextLeadingImage,
                onClickDownEvent);

            episodeNumberText.text = "EP" + episodeCell.episodeNumber;
            episodeNameText.text = "「" + episodeCell.episodeName + "」";

            storyBackgroundImage.sprite = episodeCell.firstBackGroundSprite;
            
            isStorySystemSetupComplete = true;
        }

        /// <summary>
        /// StoryEventを実行
        /// </summary>
        public async UniTask StorySystemStart(StoryEpisodeCell episodeCell, Action onComplete)
        {
            if (!isStorySystemSetupComplete)
            {
                Debug.LogError("StorySystem is Not Setup Completed");
            }
            
            var currentEvent = episodeCell.FirstEvent;
            
            while(true)
            {
                isEventing = true;
                
                await currentEvent.StartEvent(messageBox);

                isEventing = false;

                if (currentEvent.IsAutoEvent)
                {
                    
                }
                else if (StorySetting.isAutoSkip)
                {
                    await UniTask.Delay(1000);
                }
                else
                {
                    await UniTask.WaitUntil(() => isScreenClickedOnEventEnd);
                    isScreenClickedOnEventEnd = false;
                    
                    AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_pageTurn);
                }

                currentEvent = currentEvent.GetNextEvent();

                if (currentEvent == null)
                {
                    break;
                }
            }

            StorySystemEnd(onComplete);
        }


        void StorySystemEnd(Action onComplete)
        {
            // メッセージボックスなどを消して終了後の処理(callback)を実行
            // Debug.Log("StorySystemEnd");
            
            onComplete?.Invoke();
        }
        
        
        void OnScreenClicked()
        {
            if(isEventing) return;
            
            isScreenClickedOnEventEnd = true;
        }
    }
}
