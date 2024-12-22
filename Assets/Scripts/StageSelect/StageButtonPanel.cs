using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OmojiP.AudioSystem;
using OmojiP.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(Image)), RequireComponent(typeof(Button))]
public class StageButtonPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Image image;

    Image ButtonImage
    {
        get
        {
            if (image)
            {
                return image;
            }
            else
            {
                image = GetComponent<Image>();
                return image;
            }
        }
    }
    
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite pushedSprite;
    [SerializeField] private Vector3 pushedMove;
    [SerializeField] private Transform stickerImageTransform;
    private Vector3 normalStickerPos;
    private Vector3 pushedStickerPos;
    
    [SerializeField] private GameObject clearLabel;
    
    [SerializeField] bool isComingSoon = false;
    
    Animator animator;
    
    public void SetStagePanel(bool isPlayedUnlockStage, bool isClearStage, Action onButton)
    {
        normalStickerPos = stickerImageTransform.position;
        pushedStickerPos = stickerImageTransform.position + pushedMove;
        
        animator = GetComponent<Animator>();
        // clearLabel.SetActive(false);

        PlayStagePanelAnimation(isPlayedUnlockStage);

        Debug.Log($"{gameObject.name}: isOpenStage: {isPlayedUnlockStage}");
        
        if (isPlayedUnlockStage)
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                GetComponent<Button>().interactable = false;
                onButton?.Invoke();
            });
        }
        
        if (isClearStage)
        {
            // clearLabel.SetActive(true);
        }
        
        
    }

    public async UniTask PlayUnlockStage(Action onButton)
    {
        if (!isComingSoon)
        {
            await AwaitUtility.WaitUntilPlayAnimationEnd(animator, Animator.StringToHash("Unlock"), 0);
        }
        
        GetComponent<Button>().onClick.AddListener(() =>
        {
            onButton?.Invoke();
        });
        
        PlayStagePanelAnimation(true);
    }

    private void PlayStagePanelAnimation(bool isOpenStage)
    {
        if (isOpenStage)
        {
            animator.Play(Animator.StringToHash("IdleUnlock"), 0);
        }
        else
        {
            if (isComingSoon)
            {
                animator.Play(Animator.StringToHash("IdleUnlock"), 0);
            }
            else
            {
                animator.Play(Animator.StringToHash("IdleLock"), 0);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_pushDown);
     
        ButtonImage.sprite = pushedSprite;
        stickerImageTransform.position = pushedStickerPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySe(AudioManager.Instance.Sounds.se_pushUp);
        
        ButtonImage.sprite = normalSprite;
        stickerImageTransform.position = normalStickerPos;
    }
}
