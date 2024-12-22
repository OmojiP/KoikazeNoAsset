using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OmojiP.Utils
{
    public class OnClickDownEvent : MonoBehaviour, IPointerDownHandler
    {
        public event Action onClickDown;
    
        public void OnPointerDown(PointerEventData eventData)
        {
            onClickDown?.Invoke();
        }
    }
}

