using System;
using System.Collections;
using System.Collections.Generic;
using BlockBishoujo.Constant;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using OmojiP.AudioSystem;
using OmojiP.Utils;
using UnityEngine;
using Random = System.Random;

namespace BlockBishoujo.InGame
{
    /// <summary>
    /// ボールの挙動制御
    /// </summary>
    public class Ball : MonoBehaviour
    {
        private event Action onBallReturn;

        private Vector2 shooterPos;
        private Rigidbody2D rb;
        
        private Collider2D[] colliders;
        
        private Animator animator;
        
        public void BallSetUp(Action onBallReturn, Vector2 shooterPos)
        {
            this.onBallReturn = onBallReturn;
            this.shooterPos = shooterPos;
            colliders = GetComponents<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            
            transform.position = shooterPos;
        }
        
        /// <summary>
        /// ボールを指定方向に発射する、帰ってきたときにonBallReturnを実行
        /// </summary>
        public void Shoot(Vector2 dir, float speed)
        {
            rb.velocity = dir.normalized * speed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {

            if (other.gameObject.CompareTag(Tags.WALL))
            {
                // 壁に当たった
            }
            else if (other.gameObject.CompareTag(Tags.BLOCK))
            {
                // ブロックに当たった
                // ブロックの破壊はブロック側で行っている
                PlayHitEffect().Forget();
            }
            else if (other.gameObject.CompareTag(Tags.BALL_COLLECT))
            {
                // ボールを回収するエリアに当たった
                GoBackBallShooter();
            }
        }

        private bool isGoBacking = false;

        // BallShooterへ戻る
        public void GoBackBallShooter()
        {
            if(isGoBacking) return;
            isGoBacking = true;
            
            foreach (var col in colliders)
            {
                col.enabled = false;
            }
            
            rb.velocity = Vector2.zero;
            
            transform.DOMove(shooterPos, 0.3f).OnComplete(() =>
            {
                foreach (var col in colliders)
                {
                    col.enabled = true;
                }

                isGoBacking = false;
                onBallReturn?.Invoke();
            });
        }

        async UniTask PlayHitEffect()
        {
            if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
            {
                AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_breakBlock1);
            }
            else
            {
                AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_breakBlock2);
            }
            
            if (animator)
            {
                animator.Play("HitBlock");
            }

            await AwaitUtility.ChangeTimeScale(0.5f, 0.05f);
            // Time.timeScale = 0.3f;
            
            await UniTask.Delay(30);
            
            await AwaitUtility.ChangeTimeScale(1f, 0.05f);
            // Time.timeScale = 1.0f;
        }
    }
}