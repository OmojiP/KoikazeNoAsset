using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBishoujo.InGame
{
    public class GameTimer
    {
        private Text timerText;
        private event Action onTimerEnd;

        private float timeLimit;

        bool isTimerRunning = false;
        
        public GameTimer(Text timerText, float timeLimit, Action onTimerEnd)
        {
            this.timerText = timerText;
            this.timeLimit = timeLimit;
            this.onTimerEnd += onTimerEnd;
            timerText.text = $"{timeLimit:F0}";
        }

        public async UniTaskVoid TimerStart(CancellationToken ct)
        {
            isTimerRunning = true;
            
            while (true)
            {
                if (!isTimerRunning)
                {
                    break;
                }
                
                timeLimit -= Time.deltaTime;
                
                timerText.text = $"{timeLimit:F0}";
                await UniTask.Yield(ct);

                if (timeLimit <= 0)
                {
                    break;
                }
            }
            
            onTimerEnd?.Invoke();
        }

        
        public void TimerStop()
        {
            isTimerRunning = false;
        }

    }
}