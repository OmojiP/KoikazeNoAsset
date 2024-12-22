using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OmojiP.Utils
{
    public class OnClickUpEvent : MonoBehaviour, IPointerUpHandler
    {
        public event Action onClickup;
    
        public void OnPointerUp(PointerEventData eventData)
        {
            onClickup?.Invoke();
        }
    }
}

