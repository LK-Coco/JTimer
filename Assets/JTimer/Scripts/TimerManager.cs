using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTimer
{

    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Instance;

        private List<Timer> _timers = new List<Timer>();

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateAllTimers();
        }

        private void UpdateAllTimers()
        {
            for (int i = 0; i < _timers.Count; i++)
            {
                _timers[i].Update();
            }

            _timers.RemoveAll(t => t.IsDone);
        }

        public void Register(Timer timer)
        {
            _timers.Add(timer);
        }


    }

}