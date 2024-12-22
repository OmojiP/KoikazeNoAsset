using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace OmojiP.Utils
{

    public static class AwaitUtility
    {
        /// <summary>
        /// アニメーションをステートハッシュ指定で再生し、終了を待機する
        /// (次のステートに自動遷移する場合は使えない)
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="animationStateHash"></param>
        /// <param name="layerIndex"></param>
        public static async UniTask WaitUntilPlayAnimationEnd(Animator animator, int animationStateHash, int layerIndex, CancellationToken ct = default)
        {
            animator.Play(animationStateHash, layerIndex, 0f);

            // ステートの遷移に1フレーム待つ必要がある
            await UniTask.Yield(cancellationToken: ct);

            await UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= 1f, cancellationToken: ct);
        }
        
        /// <summary>
        /// Time.timeScaleを線形変化させる
        /// </summary>
        public static async UniTask ChangeTimeScale(float endTimeScaleValue, float duration)
        {
            // DOTween のシーケンスを生成
            await DOTween.To(() => Time.timeScale, x => Time.timeScale = x, endTimeScaleValue, duration)
                .SetEase(Ease.Linear) // 任意のイージングを設定
                .ToUniTask();         // UniTask として待機
        }
    }
}