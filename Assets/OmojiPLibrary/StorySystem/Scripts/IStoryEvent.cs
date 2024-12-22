using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace OmojiP.StorySystem
{
    public interface IStoryEvent
    {
        bool IsAutoEvent { get; }
        
        UniTask StartEvent(MessageBox messageBox);

        IStoryEvent GetNextEvent();
    }
}