using System.Collections;
using System.Collections.Generic;
using System.Threading;
using BlockBishoujo.Constant;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using OmojiP.SceneLoadSystem;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBishoujo.Album
{
    public class AlbumManager : MonoBehaviour
    {
        [SerializeField] private AlbumPage[] albumPages;
        [SerializeField] private AlbumButton[] albumButtons;
        [SerializeField] private Button backButton;
        
        [SerializeField] AlbumPopup albumPopup;
        
        // AlbumPopup

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < albumPages.Length; i++)
            {
                int pageId = i;
                
                albumButtons[pageId].button.onClick.AddListener(() =>
                {
                    albumPopup.AlbumPopUp(albumPages[pageId], destroyCancellationToken).Forget();
                });
                albumButtons[pageId].still.sprite = albumPages[pageId].buttonSprite;
            }
            
            backButton.onClick.AddListener(() =>
            {
                backButton.interactable = false;
                AudioManager.Instance.StopBgm();
                AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_select2);
                SceneLoader.Instance.LoadScene(Scenes.STAGE_SELECT).Forget();
            });
            
            StartAsync(destroyCancellationToken).Forget();
            
        }

        async UniTask StartAsync(CancellationToken ct)
        {
            await UniTask.WaitUntil(() => !SceneLoader.Instance.IsSceneLoading, cancellationToken: ct);
            
            AudioManager.Instance.PlayBgm(AudioManager.Instance.Sounds.bgm_album).Forget();
        }

    }
}