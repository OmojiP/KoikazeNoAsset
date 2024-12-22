using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using UnityEngine;

public class PlayBgmOnAnimator : MonoBehaviour
{
    public void PlayBgmEpSuccess()
    {
        AudioManager.Instance.PlayBgm(AudioManager.Instance.Sounds.bgm_storySuccess).Forget();
    }
}
