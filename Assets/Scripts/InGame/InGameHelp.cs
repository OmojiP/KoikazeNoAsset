using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using OmojiP.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBishoujo.InGame
{
    public class InGameHelp : MonoBehaviour
    {
        [SerializeField] Button okButton;

        Animator helpPanelAnimator;

        bool isOkClicked = false;

        public async UniTask PlayHelp()
        {
            okButton.onClick.AddListener(OnOkButton);
            helpPanelAnimator = GetComponent<Animator>();

            await AwaitUtility.WaitUntilPlayAnimationEnd(helpPanelAnimator, Animator.StringToHash("HelpStart"), 0);

            isOkClicked = false;

            await UniTask.WaitUntil(() => isOkClicked);
            AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_select2);

            await AwaitUtility.WaitUntilPlayAnimationEnd(helpPanelAnimator, Animator.StringToHash("HelpEnd"), 0);
        }

        private void OnOkButton()
        {
            Debug.Log("ボタンが押された");
            isOkClicked = true;
        }

    }
}