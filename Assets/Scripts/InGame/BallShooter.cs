using OmojiP.AudioSystem;
using OmojiP.Utils;
using Unity.Mathematics;
using UnityEngine;

namespace BlockBishoujo.InGame
{
    /// <summary>
    /// ボールを発射する
    /// </summary>
    public class BallShooter : MonoBehaviour
    {
        // ボールプレハブ
        [SerializeField] private Ball ballPrefab;
        // 矢印
        [SerializeField] private GameObject arrowObj;

        // 発射の入力
        [SerializeField] private OnClickDownEvent onShootClickDownEvent;

        // ボール
        private Ball ball;
        
        // 矢印の向き(角度 Deg)
        private float shootAngle;
        // 発射する向き(ベクトル)
        private Vector2 shootDir;
        
        // 矢印の向きのオフセット インスペクター上で調整
        [SerializeField] private float shootAngleOffset;
        
        // 矢印の向きの変化するスピード 難易度調整
        [SerializeField] private float shootAngleMoveSpeed;
        // ボールの射出速度 難易度調整
        [SerializeField] private float ballSpeed;

        private bool isGaming;
        // 発射構え中フラグ
        private bool isShootAiming;

        // 矢印時計回りフラグ
        private bool isArrowClockwise;

        // 矢印Objの向き
        private float offsetedArrowAngle;
        
        void Start()
        {
            shootAngle = 0;
            shootDir = Vector2.zero;

            onShootClickDownEvent.onClickDown += OnShootClickDown;
            
            offsetedArrowAngle = shootAngle + shootAngleOffset;
            arrowObj.transform.rotation = Quaternion.Euler(0, 0, offsetedArrowAngle);

            ball = Instantiate(ballPrefab, transform.position, quaternion.identity);
            ball.BallSetUp(OnBallReturn, this.transform.position);
        }

        public void GameStart()
        {
            isGaming = true;
            isShootAiming = true;
        }
        public void GameEnd()
        {
            isGaming = false;
        }

        void Update()
        {
            if (!isGaming)return;
            
            // ボール発射中ならEnter or 画面タップで Ball を引き戻す
            if (!isShootAiming)
            {
                // Enterキーで発射
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    CollectBall();
                }
            }
            // ボール狙い中
            else
            {
                // 矢印の回転向きを変える
                if (isArrowClockwise)
                {
                    shootAngle -= shootAngleMoveSpeed * Time.deltaTime;

                    if (shootAngle <= -60)
                    {
                        shootAngle = -60;
                        isArrowClockwise = false;
                    }
                }
                else
                {
                    shootAngle += shootAngleMoveSpeed * Time.deltaTime;

                    if (shootAngle >= 60)
                    {
                        shootAngle = 60;
                        isArrowClockwise = true;
                    }
                }

                // 矢印Objの向きの更新
                offsetedArrowAngle = shootAngle + shootAngleOffset;
                arrowObj.transform.rotation = Quaternion.Euler(0, 0, offsetedArrowAngle);

                // Enterキーで発射
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ShootBall();
                }
            }
        }

        // offsetedArrowAngleの方向に発射
        void ShootBall()
        {
            if(!isShootAiming) return;
            isShootAiming = false;
            
            shootDir = new Vector2(Mathf.Cos(offsetedArrowAngle * Mathf.Deg2Rad),
                Mathf.Sin(offsetedArrowAngle * Mathf.Deg2Rad));

            AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_shoot);
            ball.Shoot(shootDir, ballSpeed);
        }

        void CollectBall()
        {
            if(isShootAiming) return;
            
            // ボールに戻ってきてもらう 戻ってきたら OnBallReturn が実行される
            ball.GoBackBallShooter();
        }

        // 画面タップでも発射
        void OnShootClickDown()
        {
            if(!isGaming) return;
            
            if (isShootAiming)
            {
                ShootBall();
            }
            else
            {
                CollectBall();
            }
        }
        
        // ボールが回収エリアに当たった時に呼ぶ
        void OnBallReturn()
        {
            isShootAiming = true;
        }
    }
}