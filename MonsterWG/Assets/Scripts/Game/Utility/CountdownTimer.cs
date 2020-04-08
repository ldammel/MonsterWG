using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Utility
{
    public class CountdownTimer : MonoBehaviour
    {
        #region Variables

        public float timeLeft = 300.0f;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image timerImage;
        [SerializeField] private ToggleObject target;

        private bool _stop = true;
        private float _minutes;
        private float _seconds;
        private float _startTime;
        
        #endregion

        #region Functions
        
        private void Start()
        {
            StartTimer();
        }

        public void SetTimeLeft(float value)
        {
            timeLeft = value;
        }

        public void StartTimer()
        {
            _stop = false;
            _startTime = timeLeft;
            StartCoroutine(UpdateCoroutine());
        }

        private void Update() 
        {
            if(_stop) return;
            timeLeft -= Time.deltaTime;
            _minutes = Mathf.Floor(timeLeft / 60);
            _seconds = timeLeft % 60;
            if(_seconds > 59) _seconds = 59;
            if (!(_minutes < 0)) return;
            _stop = true;
            _minutes = 0;
            _seconds = 0;
            target.ToggleObjects();
        }
 
        private IEnumerator UpdateCoroutine()
        {
            while(!_stop)
            {
                text.text = $"{_minutes:0}:{_seconds:00}";
                timerImage.fillAmount = timeLeft / _startTime;
                yield return new WaitForSeconds(0.2f);
            }
        }
        
        #endregion
    }
}
