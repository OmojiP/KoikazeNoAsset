using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using OmojiP.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBishoujo.Album
{
    public class AlbumPopup : MonoBehaviour
    {
        
        /*
        
        スチルの枚数に応じてページボタン数を変更
        ページボタンに画像の変更を登録
        画像変更はAnimatorで動きを付ける
        閉じるボタンでPopUpを閉じる(Animator)
        
        */
        
        [SerializeField] Animator albumPageButtonPrefab;
        [SerializeField] Transform pageButtonsParent;
        [SerializeField] Image stillImage;
        [SerializeField] Animator stillAnimator;
        [SerializeField] private Button closeButton;
        
        AlbumPage albumPage;
        Animator[] pageButtons;
        int currentPageId;
        bool isChangingPage;
        bool isPopupAnimating;

        private void Start()
        {
            ResetAlbumPopUp();
            closeButton.onClick.AddListener(() =>
            {
                AlbumPopdown(destroyCancellationToken).Forget();
            });
        }

        public async UniTask AlbumPopUp(AlbumPage albumPage, CancellationToken ct)
        {
            isPopupAnimating = true;
            this.albumPage = albumPage;

            ResetAlbumPopUp();

            pageButtons = new Animator[albumPage.stillSprites.Length];
            
            for (var i = 0; i < albumPage.stillSprites.Length; i++)
            {
                pageButtons[i] = Instantiate(albumPageButtonPrefab, pageButtonsParent);
                int pageId = i;

                pageButtons[i].SetBool("Selected", i == 0);
                
                pageButtons[i].gameObject.GetComponent<Button>().onClick.AddListener(async () =>
                {
                    isChangingPage = true;
                    await ChangePage(pageId, destroyCancellationToken);
                    isChangingPage = false;
                });
            }
            
            var s = albumPage.stillSprites[0];
            stillImage.sprite = s;
            
            await AwaitUtility.WaitUntilPlayAnimationEnd(stillAnimator, Animator.StringToHash("Popup"), 0, ct);
            
            isPopupAnimating = false;
        }

        public async UniTask AlbumPopdown(CancellationToken ct)
        {
            isPopupAnimating = true;

            await AwaitUtility.WaitUntilPlayAnimationEnd(stillAnimator, Animator.StringToHash("Popdown"), 0, ct);

            ResetAlbumPopUp();
            
            isPopupAnimating = false;
        }

        void ResetAlbumPopUp()
        {
            isChangingPage = false;
            currentPageId = 0;

            if (pageButtons != null)
            {
                foreach (var p in pageButtons)
                {
                    Destroy(p.gameObject);
                }   
            }

            pageButtons = null;
        }

        async UniTask ChangePage(int pageId, CancellationToken ct)
        {
            if (pageId < 0 || pageId >= pageButtons.Length)
            {
                throw new System.Exception("Invalid page ID");
            }

            for (int i = 0; i < pageButtons.Length; i++)
            {
                pageButtons[i].SetBool("Selected", i == pageId);
            }

            currentPageId = pageId;
            
            await AwaitUtility.WaitUntilPlayAnimationEnd(stillAnimator, Animator.StringToHash("PageChange"), 0, ct);
        }

        // stillのアニメーションに設定
        public void ChangeStillImage()
        {
            stillImage.sprite = albumPage.stillSprites[currentPageId];
        }
    }
}