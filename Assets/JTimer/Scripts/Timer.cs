using System;
using System.Collections.Generic;


namespace JTimer
{

    public class Timer
    {
        #region Public Properties

        public bool IsDone => _state == TimerState.Stoped;

        #endregion

        #region Private Enum

        private enum TimerState
        {
            Started = 0,
            Paused = 1,
            Stoped = 2,
            Inited = 3,
        }

        #endregion

        #region Private Fields
        
        private bool _useScaleTime;
        private bool _isLoop;
        private int _loopTimes;
        private float _cachedTime;
        private float _duration;
        private Action<float> _onUpdate;
        private Action _onComplete;
        private TimerState _state;

        #endregion

        #region Constructor

        public Timer(float duration,bool useScaleTime,bool isLoop, int loopTimes,Action<float> onUpdate, Action onComplete)
        {
            _duration = duration;
            _useScaleTime = useScaleTime;
            _isLoop = isLoop;
            _loopTimes = loopTimes;
            _onUpdate = onUpdate;
            _onComplete = onComplete;
            _state = TimerState.Inited;
        }

        public Timer(float duration, bool useScaleTime, bool isLoop, int loopTimes)
        {
            _duration = duration;
            _useScaleTime = useScaleTime;
            _isLoop = isLoop;
            _loopTimes = loopTimes;
        }

        #endregion

        #region Public Methods

        public Timer OnUpdated(Action<float> action)
        {
            this._onUpdate = action;
            return this;
        }

        public Timer OnCompleted(Action action)
        {
            this._onComplete = action;
            return this;
        }

        public void Start()
        {
            if (_state != TimerState.Inited) return;
            TimerManager.Instance.Register(this);
            _state = TimerState.Started;
        }

        public void Pause()
        {
            _state = TimerState.Paused;
        }

        public void Resume()
        {
            _state = TimerState.Started;
        }

        public void Stop()
        {
            _state = TimerState.Stoped;
        }

        #endregion

        #region Internal Methods

        internal void Update()
        {
            if (_state != TimerState.Started) return;
            _cachedTime += _useScaleTime ? UnityEngine.Time.deltaTime : UnityEngine.Time.unscaledDeltaTime;
            _onUpdate?.Invoke(_cachedTime);
            if (_cachedTime > _duration)
            {
                _onComplete?.Invoke();
                if (_isLoop && _loopTimes > 0)
                {
                    _loopTimes--;
                    _cachedTime -= _duration;
                }
                else
                {
                    _state = TimerState.Stoped;
                }
            }
        }

        #endregion

        #region Static Methods

        public static Timer Delay(float duration, bool useScaleTime, Action onComplete)
        {
            return new Timer(duration, useScaleTime, false, 0,null,onComplete);
        }

        public static Timer Repeat(float duration, bool useScaleTime, int loopTimes,Action onComplete)
        {
            return new Timer(duration, useScaleTime, true, loopTimes, null, onComplete);
        }

        #endregion
    }

}
