using System.Collections;
using System.Collections.Generic;
using OmojiP.AudioSystem;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBishoujo.Manager
{
    public class AudioSettingManager : MonoBehaviour
    {
        [SerializeField] Button audioSettingButton;
        [SerializeField] Button audioOkButton;
        [SerializeField] Slider masterSlider;
        [SerializeField] Slider bgmSlider;
        [SerializeField] Slider seSlider;

        [SerializeField] GameObject audioSettingPanel;
        
        void Start()
        {
            masterSlider.onValueChanged.AddListener( (value) =>
            {
                AudioManager.Instance.SetVolume(AudioManager.StandardMixerGroupType.Master, value);
            });
            bgmSlider.onValueChanged.AddListener( (value) =>
            {
                AudioManager.Instance.SetVolume(AudioManager.StandardMixerGroupType.BGM, value);
            });
            seSlider.onValueChanged.AddListener( (value) =>
            {
                AudioManager.Instance.SetVolume(AudioManager.StandardMixerGroupType.SE, value);
            });
            
            audioSettingButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_select2);
                audioSettingPanel.SetActive(true);
            });
            audioOkButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_select2);
                audioSettingPanel.SetActive(false);
            });

            masterSlider.value = 0.8f;
            bgmSlider.value = 1f;
            seSlider.value = 1f;
            
            audioSettingPanel.SetActive(false);
        }
    }
}