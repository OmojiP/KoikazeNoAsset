using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using BlockBishoujo.Constant;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using OmojiP.SceneLoadSystem;
using OmojiP.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBishoujo.Manager
{
    public class EndRollManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            pyururunText.text = String.Empty;
            
            StartEndroll(destroyCancellationToken).Forget();
        }

        async UniTask StartEndroll(CancellationToken ct)
        {
            await UniTask.WaitUntil(() => !SceneLoader.Instance.IsSceneLoading, cancellationToken: ct);
            
            AudioManager.Instance.PlayBgm(AudioManager.Instance.Sounds.bgm_storySuccess).Forget();
            
            await AwaitUtility.WaitUntilPlayAnimationEnd(endrollAnimator, Animator.StringToHash("EndRoll"), 0);
            
            AudioManager.Instance.StopBgm(fadeOutTime:0.5f);
            
            SceneLoader.Instance.LoadScene(Scenes.TITLE).Forget();
        }

        [SerializeField] Animator endrollAnimator;
        [SerializeField] private Text pyururunText;

        [SerializeField] private float characterSpan;
        [SerializeField] private float statementSpan;
        [SerializeField, TextArea] private string message;

        public void StartPyururunText()
        {
            PlayMessage(destroyCancellationToken).Forget();
        }
        
        async UniTask PlayMessage(CancellationToken ct)
        {
            pyururunText.text = String.Empty;

            string[] lines = message.Split('\n');

            foreach (var line in lines)
            {
                foreach (var c in line)
                {
                    pyururunText.text += c;

                    await UniTask.Delay((int)(1000 * characterSpan), cancellationToken: ct);
                }

                pyururunText.text += Environment.NewLine;
                await UniTask.Delay((int)(1000 * (statementSpan - characterSpan)), cancellationToken: ct);
            }
        }
    }
}