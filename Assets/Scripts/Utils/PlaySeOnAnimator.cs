using System.Collections;
using System.Collections.Generic;
using OmojiP.AudioSystem;
using UnityEngine;

public class PlaySeOnAnimator : MonoBehaviour
{
    [SerializeField] Sounds.SeType seType;
    
    public void PlaySe()
    {
        AudioManager.Instance.PlaySe(seType);
    }
}
