using System;
using System.Collections.Generic;
using Base.Classes;
using UnityEngine;

namespace Infrastructure.Services.Timer
{
    public class TimeService : BaseService
    {
        public bool IsActive;
        private readonly List<EncapsulatedTimer> _timers = new List<EncapsulatedTimer>();

        private void Clear()
        {
            _timers.Clear();
        }

        public void ResumeTimer(Timer timer, float time)
        {
            var newTimer = (EncapsulatedTimer)timer;
            newTimer.SetNewTime(time);
            if(!_timers.Contains(newTimer))
            {
                _timers.Add(newTimer);
            }
        }
        public Timer GetTimer(float time, Action onComplete, Action<float> onUpdate = null)
        {
            var timer = new EncapsulatedTimer(time, onComplete, onUpdate);
            _timers.Add(timer);
            
            return timer;
        }
        
        public void StopTimer(Timer timer, float time)
        {
            if(timer == null)
                return;
            
            var timerToStop = (EncapsulatedTimer)timer;
            timerToStop.IsStopped = true;

            EncapsulatedTimer stopTimer = (EncapsulatedTimer)GetTimer(time, () =>
            {
                timerToStop.IsStopped = false;
            });
            _timers.Add(stopTimer);
        }
        public override void Update()
        {
            if(!IsActive)
                return;

            List<EncapsulatedTimer> expiredTimers = new List<EncapsulatedTimer>();
            
            foreach (var timer in _timers)
            {
                timer.UpdateTime(Time.deltaTime);
                if (timer.IsDestroyed || timer.IsCompleted)
                {
                    expiredTimers.Add(timer);
                }
            }

            foreach (var timer in expiredTimers)
            {
                if(timer.TimeLeft == 0 || timer.IsDestroyed)
                {
                    _timers.Remove(timer);
                }
            }
        }
        
        private class EncapsulatedTimer : Timer
        {
            public float TimeLeft;
            
            public bool IsStopped;
            public bool IsCompleted;
            public bool IsDestroyed;

            private readonly Action _onComplete;
            private readonly Action<float> _onUpdate;
            
            public EncapsulatedTimer(float time, Action onComplete, Action<float> onUpdate = null)
            {
                _onComplete = onComplete;
                _onUpdate = onUpdate;
                TimeLeft = time;
                IsCompleted = false;
                IsStopped = false;
            }

            public override void Destroy()
            {
                IsDestroyed = true;
            }
            public void SetNewTime(float time)
            {
                TimeLeft = time;
                IsCompleted = false;
                IsDestroyed = false;
            }
            public void UpdateTime(float delta)
            {
                if(IsDestroyed)
                    return;
                
                if(IsStopped)
                    return;
                
                if(IsCompleted)
                    return;

                _onUpdate?.Invoke(TimeLeft);
                TimeLeft = Mathf.Clamp(TimeLeft - delta, 0, float.MaxValue);
                if (TimeLeft == 0)
                {
                    IsCompleted = true;
                    _onComplete?.Invoke();
                }
            }
        }
    }
}