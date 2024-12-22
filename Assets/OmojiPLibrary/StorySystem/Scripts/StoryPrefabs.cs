using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Cysharp.Threading.Tasks;
using OmojiP.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace OmojiP.StorySystem
{
    public class StoryPrefabs : Singleton<StoryPrefabs>
    {
        // Prefabリストに追加されているPrefabをまとめて生成し、

        StoryViewObj[] storyViewPrefabs;
        
        Transform storyViewParent;
        private Transform storySceneChangeParent;

        public Dictionary<StoryViewType, StoryViewObj> storyViewDict;
        private Animator storySceneChangeAnimator;
        
        private Image storyBackgroundImage;

        public void SetStoryViews(Transform storyViewParent, StoryViewObj[] storyViewPrefabs, Transform storySceneChangeParent, GameObject storySceneChangePrefab, Image storyBackgroundImage)
        {
            this.storyViewParent = storyViewParent;
            this.storyViewPrefabs = storyViewPrefabs;
            this.storySceneChangeParent = storySceneChangeParent;
            this.storyBackgroundImage = storyBackgroundImage;
            
            storyViewDict = new();
            foreach (var t in storyViewPrefabs)
            {
                var obj = Instantiate(t, storyViewParent);
                obj.gameObject.SetActive(false);
                storyViewDict.Add(obj.storyViewType, obj);
            }

            storySceneChangeAnimator =
                Instantiate(storySceneChangePrefab, storySceneChangeParent).GetComponent<Animator>();
        }
        
        public void ViewAnimate(StoryViewType storyViewType, int stateHash)
        {
            storyViewDict[storyViewType].GetComponent<Animator>().Play(stateHash);
        }
        public void ViewAnimate(StoryViewType storyViewType, string stateName)
        {
            int stateHash = Animator.StringToHash(stateName);
            ViewAnimate(storyViewType, stateHash);
        }
        
        public void ViewSetActive(StoryViewType storyViewType, bool isActivate)
        {
            storyViewDict[storyViewType].gameObject.SetActive(isActivate);
        }

        public async UniTask SceneChangeAnimate(int stateHash)
        {
            await AwaitUtility.WaitUntilPlayAnimationEnd(storySceneChangeAnimator, stateHash, 0);
        }
        public async UniTask SceneChangeAnimate(string stateName)
        {
            int stateHash = Animator.StringToHash(stateName);
            await SceneChangeAnimate(stateHash);
        }

        public void ChangeBackground(Sprite sprite)
        {
            storyBackgroundImage.sprite = sprite;
        }
    }
}