using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using Unity.VisualScripting;

namespace OmojiP.AudioSystem
{
    
    public class AudioManager : Utils.Singleton<AudioManager>
    {
        [SerializeField] AudioMixer audioMixer;

        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource seSource;

        private Sounds sounds;

        public Sounds Sounds
        {
            get
            {
                if (!sounds)
                {
                    sounds = FindObjectOfType<Sounds>();
                }

                return sounds;
            }
        }

        public void SetVolume(StandardMixerGroupType groupType, float normalizedVolume)
        {
            audioMixer.SetFloat(groupType.ToString(), Mathf.Lerp(-80f, 0f, normalizedVolume));
        }

        public float GetNormalizedVolume(StandardMixerGroupType groupType)
        {
            if (audioMixer.GetFloat(groupType.ToString(), out float vol))
            {
                return Mathf.Clamp(vol, 0f, 1f);
            }
            
            throw new UnityException("Can't get volume of type " + groupType.ToString());
        }
        

        private Tween fadeTween;

        /// <summary>
        /// 指定のClipをFadeしながら再生
        /// </summary>
        /// <param name="bgmClip"></param>
        /// <param name="isLoop"></param>
        /// <param name="fadeInTime"></param>
        /// <param name="fadeOutTime"></param>
        public async UniTask PlayBgm(AudioClip bgmClip, bool isLoop = true, float fadeInTime = 0.1f, float fadeOutTime = 0.1f,  float startTime = 0f, CancellationToken ct = default)
        {
            if (!Application.isPlaying)
            {
                fadeTween?.Kill();
                bgmSource.volume = 0f;
                Debug.LogWarning("AudioManager: Application is not playing");
                return;
            }

            if (!Application.isFocused)
            {
                fadeTween?.Kill();
                bgmSource.volume = 0f;   
            }
            
            await UniTask.WaitUntil(() => Application.isFocused, cancellationToken: ct);
            
            // 再生中でない場合はフェードアウトせず、直接フェードインを始める
            if (!bgmSource.isPlaying)
            {
                fadeTween?.Kill(); // 既存のフェード処理を停止

                bgmSource.clip = bgmClip;
                bgmSource.loop = isLoop;
                bgmSource.Play();
                bgmSource.time = startTime;

                fadeTween = DOTween.To(() => bgmSource.volume, x => bgmSource.volume = x, 1f, fadeInTime); // フェードイン
            }
            else
            {
                // 再生中の場合はフェードアウトしてから新しいBGMを再生
                fadeTween?.Kill(); // 既存のフェード処理を停止

                fadeTween = DOTween.To(() => bgmSource.volume, x => bgmSource.volume = x, 0f, fadeOutTime)
                    .OnComplete(() =>
                    {
                        bgmSource.clip = bgmClip;
                        bgmSource.loop = isLoop;
                        bgmSource.Play();                        
                        bgmSource.time = startTime;

                        fadeTween = DOTween.To(() => bgmSource.volume, x => bgmSource.volume = x, 1f,
                            fadeInTime); // フェードイン
                    });
            }
        }


        /// <summary>
        /// BGMをFadeしながら停止
        /// </summary>
        /// <param name="fadeOutTime"></param>
        public void StopBgm(float fadeOutTime = 0.1f)
        {
            if (!bgmSource.isPlaying) return;

            fadeTween?.Kill(); // 既存のフェード処理を停止
            fadeTween = DOTween.To(() => bgmSource.volume, x => bgmSource.volume = x, 0f, fadeOutTime);
        }

        /// <summary>
        /// 指定のClipを一度だけ再生
        /// </summary>
        /// <param name="seClip"></param>
        public void PlaySe(AudioClip seClip)
        {
            seSource.PlayOneShot(seClip);
        }
        
        /// <summary>
        /// 指定のSeTypeを一度だけ再生
        /// </summary>
        public void PlaySe(Sounds.SeType seType)
        {
            AudioClip seClip = null;
            
            switch (seType)
            {
                case Sounds.SeType.NONE:
                    return;
                case Sounds.SeType.SE_HYUN:
                    seClip = sounds.se_hyun;
                    break;
                case Sounds.SeType.SE_STUMP:
                    seClip = sounds.se_stump;
                    break;
                case Sounds.SeType.SE_ESCAPE:
                    seClip = sounds.se_escape;
                    break;
                case Sounds.SeType.SE_SKIRT:
                    seClip = sounds.se_skirt;
                    break;
                default:
                    throw new NotImplementedException();
            }

            PlaySe(seClip);
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                // バックグラウンドに移行したとき
                Debug.LogWarning("AudioManager is paused");
                fadeTween?.Kill();
                bgmSource.volume = 0f;
                AudioListener.pause = true;
            }
            else
            {
                // フォアグラウンドに戻ったとき
                bgmSource.volume = 1f;
                AudioListener.pause = false;
            }
        }
        
        void OnApplicationQuit()
        {
            Debug.LogWarning("AudioManager is paused");
            fadeTween?.Kill();
            bgmSource.volume = 0f;
            AudioListener.pause = true;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                // フォアグラウンドに戻ったとき
                bgmSource.volume = 1f;
                AudioListener.pause = false;                
            }
            else
            {
                // バックグラウンドに移行したとき
                Debug.LogWarning("AudioManager is paused");
                fadeTween?.Kill();
                bgmSource.volume = 0f;
                AudioListener.pause = true;
            }
        }

        public enum StandardMixerGroupType
        {
            Master,
            BGM,
            SE
        }
    }
}