using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using OmojiP.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OmojiP.SceneLoadSystem
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        [SerializeField] GameObject loadingScreen;
        [SerializeField] Slider progressBar;
        [SerializeField] private Animator loadingScreenAnimator;
        
        private string loadingInStateName = "LoadingIn";
        private int loadingInStateHash;
        private string loadingOutStateName = "LoadingOut";
        private int loadingOutStateHash;

        private bool isSceneLoading = false;
        public bool IsSceneLoading => isSceneLoading;
        
        private void Start()
        {
            InitializeLoadingScreen();
            
            isSceneLoading = false;

            loadingInStateHash = Animator.StringToHash(loadingInStateName);
            loadingOutStateHash = Animator.StringToHash(loadingOutStateName);
        }
        
        private void InitializeLoadingScreen()
        {
            progressBar.value = 0;
            waitMethodList = new();
        }
        private async UniTask StartLoadingScreen()
        {
            InitializeLoadingScreen();
            loadingScreen.SetActive(true);
            
            await AwaitUtility.WaitUntilPlayAnimationEnd(loadingScreenAnimator, loadingInStateHash, 0);
            
            // loadingScreenAnimator.Play(loadingInStateHash);
            // // ステートの遷移に1フレーム待つ必要がある
            // await UniTask.Yield();
            // await UniTask.WaitUntil( () => loadingScreenAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        }
        private async UniTask EndLoadingScreen()
        {
            await AwaitUtility.WaitUntilPlayAnimationEnd(loadingScreenAnimator, loadingOutStateHash, 0);
         
            // loadingScreenAnimator.Play(loadingOutStateHash);   
            // // ステートの遷移に1フレーム待つ必要がある
            // await UniTask.Yield();
            // await UniTask.WaitUntil( () => loadingScreenAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
            
            loadingScreen.SetActive(false);
            InitializeLoadingScreen();
        }
        
        // 非同期でシーンをロード
        public async UniTask LoadScene(string sceneName, float loadingMinTime = 0.5f)
        {
            isSceneLoading = true;

            await StartLoadingScreen();

            // シーンを非同期でロードし、完了を待機
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            // ロードが完了するまで進捗を確認
            while (asyncLoad.progress >= 0.9f)
            {
                // Debug.Log("Loading progress: " + (asyncLoad.progress * 100) + "%");
                await UniTask.Yield(); // 1フレーム待機
            }

            // ユーザーの操作や条件が整ったらシーンをアクティブにする
            asyncLoad.allowSceneActivation = true;

            await UniTask.Delay((int)(loadingMinTime*1000));
            
            await UniTask.WaitUntil(() =>
                waitMethodList.All(func => func())
            );
            
            await EndLoadingScreen();

            isSceneLoading = false;
        }

        private List<Func<bool>> waitMethodList;
        
        // 準備が明けるまでシーンを開けたくない場合、これを使用して準備関数を登録する
        public void AddWaitMethod(Func<bool> method)
        {
            if (!isSceneLoading)
            {
                Debug.LogWarning("シーンロード中でないため関数が登録されません");
            }
            
            waitMethodList.Add(method);
        }
    }
}

