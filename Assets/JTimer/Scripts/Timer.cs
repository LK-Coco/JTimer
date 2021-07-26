using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        private double _cachedTime;
        private double _duration;
        private Action<double> _onUpdate;
        private Action<int> _onLoop;
        private Action _onComplete;

        private Stopwatch _stopwatch;
        private double _preTime;
        private double _nowTime;
        private TimerState _state;
        private int _currentLoop;

        #endregion

        #region Constructor

        public Timer(long duration,bool useScaleTime,bool isLoop, int loopTimes,Action<double> onUpdate, Action onComplete)
        {
            _duration = duration;
            _useScaleTime = useScaleTime;
            _isLoop = isLoop;
            _loopTimes = loopTimes;
            _onUpdate = onUpdate;
            _onComplete = onComplete;
            _state = TimerState.Inited;

            _stopwatch = new Stopwatch();
        }

        public Timer(long duration, bool useScaleTime, bool isLoop, int loopTimes,Action onComplete)
        {
            _duration = duration;
            _useScaleTime = useScaleTime;
            _isLoop = isLoop;
            _loopTimes = loopTimes;
            _onComplete = onComplete;
            _state = TimerState.Inited;

            _stopwatch = new Stopwatch();
        }

        public Timer(long duration, bool useScaleTime, bool isLoop, int loopTimes)
        {
            _duration = duration;
            _useScaleTime = useScaleTime;
            _isLoop = isLoop;
            _loopTimes = loopTimes;

            _stopwatch = new Stopwatch();
        }

        #endregion

        #region Public Methods

        public Timer OnUpdated(Action<double> action)
        {
            this._onUpdate = action;
            return this;
        }

        public Timer OnLooped(Action<int> action)
        {
            this._onLoop = action;
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
            _currentLoop = 0;

            _stopwatch.Start();
            _preTime = _stopwatch.Elapsed.TotalSeconds;
        }

        public void Pause()
        {
            _state = TimerState.Paused;
        }

        public void Resume()
        {
            _state = TimerState.Started;
            _preTime = _stopwatch.Elapsed.TotalSeconds;
        }

        public void Stop()
        {
            _state = TimerState.Stoped;
        }

        public void Update()
        {
            if (_state != TimerState.Started) return;
            _cachedTime += _useScaleTime ? GetDeltaTime() * UnityEngine.Time.timeScale : GetDeltaTime();
            _onUpdate?.Invoke(_cachedTime);
            if (_cachedTime > _duration)
            {
                _onComplete?.Invoke();
                if (_isLoop && _loopTimes > _currentLoop)
                {
                    _currentLoop++;
                    _cachedTime -= _duration;
                    _onLoop?.Invoke(_currentLoop);
                }
                else
                {
                    _state = TimerState.Stoped;
                }
            }
        }

        #endregion

        #region private Methods

        private double GetDeltaTime()
        {
            _nowTime = _stopwatch.Elapsed.TotalSeconds;
            var delta = _nowTime - _preTime;
            _preTime = _nowTime;

            return delta;
        } 

        #endregion

        #region Static Methods

        public static Timer Delay(long duration, bool useScaleTime, Action onComplete)
        {
            return new Timer(duration, useScaleTime, false, 0,null,onComplete);
        }

        public static Timer Repeat(long duration, bool useScaleTime, int loopTimes,Action onComplete)
        {
            return new Timer(duration, useScaleTime, true, loopTimes, null, onComplete);
        }

        #endregion
    }

}
