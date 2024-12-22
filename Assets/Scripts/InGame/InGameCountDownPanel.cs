using System.Collections;
using System.Collections.Generic;
using OmojiP.AudioSystem;
using UnityEngine;

namespace BlockBishoujo.InGame
{
    public class InGameCountDownPanel : MonoBehaviour
    {
        public void OnGameStartAnimarion()
        {
            AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_gameStart);
        }
    }
}