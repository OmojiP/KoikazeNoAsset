using System;
using System.Collections;
using System.Collections.Generic;
using BlockBishoujo.Constant;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BlockBishoujo.InGame
{
    /// <summary>
    /// Blockに付けるコンポーネント ボールが当たって壊れた際にイベントを発火
    /// </summary>
    public class Block : MonoBehaviour
    {
        public event Action onBallEntered;

        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] SpriteRenderer[] _outlineSpriteRenderers;
        
        [SerializeField] Color outlineColor = Color.yellow;
        [SerializeField] float outlineThickness = 1.2f;
        
        Vector2 blowDir = new Vector2(1, 2).normalized;
        float blowSpeed = 20f;

        public void InitBlock(Sprite sprite, Sprite white)
        {
            _spriteRenderer.sprite = sprite;

            Vector2[] outlinePositions = new Vector2[8];

            for (int i = 0; i < outlinePositions.Length; i++)
            {
                outlinePositions[i] = new Vector2(Mathf.Cos( (float)i/outlinePositions.Length * 2 * Mathf.PI), Mathf.Sin( (float)i/outlinePositions.Length * 2 * Mathf.PI));
            }
            
            
            for (var i = 0; i < _outlineSpriteRenderers.Length; i++)
            {
                var spriteRenderer = _outlineSpriteRenderers[i];
                spriteRenderer.sprite = white;
                spriteRenderer.color = outlineColor;
                spriteRenderer.transform.localPosition = outlinePositions[i] * transform.localScale.x * outlineThickness;
            }

            isGaming = true;
        }

        bool isGaming = false;
        
        public void GameEnd()
        {
            isGaming = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (isGaming && other.gameObject.CompareTag(Tags.BALL))
            {
                // 吹っ飛ぶ
                this.GetComponent<BoxCollider2D>().enabled = false;
                Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
                
                // 方向 dir に速度を設定
                // rb.velocity = blowDir * blowSpeed;
                // rb.velocity = (transform.position-other.transform.position).normalized * blowSpeed;
                rb.AddForce((transform.position-other.transform.position).normalized * blowSpeed , ForceMode2D.Impulse);

                onBallEntered?.Invoke();
                
                UnActivateObj().Forget();

            }
        }

        async UniTaskVoid UnActivateObj()
        {
            await UniTask.Delay(2000);
            
            this.gameObject.SetActive(false);
        }
    }
}