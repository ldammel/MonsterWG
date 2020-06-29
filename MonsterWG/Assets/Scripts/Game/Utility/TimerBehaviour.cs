using UnityEngine;
using UnityEngine.Events;

namespace Game.Utility
{
    public class TimerBehaviour : MonoBehaviour
    {
        public float duration = 1f;
        [SerializeField] private UnityEvent onTimerEnd = null;

        private Timer _timer;
        private bool _active;

        public void EndTimer()
        {
            _active = false;
            _timer.OnTimerEnd -= HandleTimerEnd;
            _timer = null;
        }

        public void StartTimer()
        {
            // Create a new timer and initialize it
            _timer = new Timer(duration);

            // Subscribe to the OnTimerEnd event to be able to handle that scenario
            _timer.OnTimerEnd += HandleTimerEnd;
            _active = true;
        }

        private void HandleTimerEnd()
        {
            // Alert any listeners that the timer has ended
            onTimerEnd.Invoke();
        }

        private void Update()
        {
            if (!_active) return;
            _timer.Tick(Time.deltaTime);
        }
    }
}