using Cysharp.Threading.Tasks;
using OmojiP.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OmojiP.StorySystem
{
    /// <summary>
    /// MessageBoxの操作
    /// </summary>
    public class MessageBox
    {
        private Text charaNameText;
        private TextMeshProUGUI messageTextTMP;
        private GameObject namePlateObj;
        private Animator messageBoxAnimator;
        private Animator messageTextAnimator;
        private Image nextLeadingImage;

        public MessageBox(Animator messageBoxAnimator, Text charaNameText, TextMeshProUGUI messageTextTMP, GameObject namePlateObj, Image nextLeadingImage, OnClickDownEvent onClickDownEvent)
        {
            this.messageBoxAnimator = messageBoxAnimator;
            this.charaNameText = charaNameText;
            this.messageTextTMP = messageTextTMP;
            this.namePlateObj = namePlateObj;
            this.nextLeadingImage = nextLeadingImage;

            onClickDownEvent.onClickDown += OnScreenClicked;
            
            nextLeadingImage.gameObject.SetActive(false);
            messageBoxAnimator.gameObject.SetActive(false);
            
            messageTextAnimator = messageTextTMP.GetComponent<Animator>();
        }
        
        // メッセージ送り中に画面をタップしたら全部表示する
        // キャラクターの表示はIStoryEventで行う
        
        bool isMessageShowing = false;
        bool isScreenClicked = false;

        public void PlayMessageBoxAnim(StoryCharacterCell characterCell)
        {
            
            messageBoxAnimator.Play(StoryAnimationTypeConstants.BoxStateHashDict[characterCell.boxAnimationType]);
            messageTextAnimator.Play(StoryAnimationTypeConstants.TextStateHashDict[characterCell.textAnimationType]);
        }
        
        public async UniTask PlayStoryCell(StoryCharacterCell characterCell)
        {
            isMessageShowing = true;
            
            namePlateObj.SetActive(characterCell.isNamePlateActive);
            
            messageBoxAnimator.gameObject.SetActive(true);
            nextLeadingImage.gameObject.SetActive(false);
            charaNameText.text = characterCell.characterName;

            PlayMessageBoxAnim(characterCell);
            
            // メッセージ送りとかアニメーションを入れる
            // 入力があったらスキップして全部表示する
            messageTextTMP.text = string.Empty;
            messageTextTMP.fontSize = (int)characterCell.fontSizeType;
            string[] textLines = characterCell.messageContext.Split('\n');
            
            
            bool isBreakMessageAsync = false;
            foreach (string line in textLines)
            {
                char[] texts = line.ToCharArray();
                
                foreach (var t in texts)
                {
                    messageTextTMP.text += t;
                    await UniTask.Delay((int)(50 * StorySetting.messageSpeed));

                    if (isScreenClicked)
                    {
                        isScreenClicked = false;
                        isBreakMessageAsync = true;
                        messageTextTMP.text = characterCell.messageContext;
                        break;
                    }
                }

                if (isBreakMessageAsync)
                {
                    break;
                }
                
                messageTextTMP.text += '\n';
                await UniTask.Delay((int)(500 * StorySetting.messageSpeed));

                isScreenClicked = false;
            }
            
            
            // 終わったら送りImageを表示する
            
            nextLeadingImage.gameObject.SetActive(true);
            
            isMessageShowing = false;
        }

        public void ClearMessage()
        {
            nextLeadingImage.gameObject.SetActive(false);
            charaNameText.text = string.Empty;
            messageTextTMP.text = string.Empty;
        }
        
        void OnScreenClicked()
        {
            if(!isMessageShowing) return;
            
            isScreenClicked = true;
        }
        
    }
}