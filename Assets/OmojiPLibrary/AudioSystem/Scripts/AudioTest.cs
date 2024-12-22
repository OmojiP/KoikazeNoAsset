using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using UnityEngine;

public class AudioTest : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.Instance.PlayBgm(AudioManager.Instance.Sounds.bgm_a).Forget();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            AudioManager.Instance.PlayBgm(AudioManager.Instance.Sounds.bgm_b).Forget();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.Instance.StopBgm();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_a);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            AudioManager.Instance.PlayBgm(AudioManager.Instance.Sounds.bgm_a, startTime:3).Forget();
        }
    }
}
