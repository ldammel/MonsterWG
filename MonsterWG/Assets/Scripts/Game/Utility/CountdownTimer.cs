using System;
using System.Collections;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Utility
{
    public class CountdownTimer : MonoBehaviour
    {
        #region Variables

        public float timeLeft = 300.0f;
        private float _currentTime = 0f;
        [SerializeField] private Image timerImage;
        [SerializeField] private ToggleObject target;

        private bool _stop = true;

        #endregion

        #region Functions
        
        private void Start()
        {
            StartTimer();
        }

        public void SetTimeLeft(float value)
        {
            _currentTime = value;
        }

        public void StartTimer()
        {
            _stop = false;
            StartCoroutine(UpdateCoroutine());
        }

        private void Update() 
        {
            if(_stop) return;
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= timeLeft)) return;
            _stop = true;
            target.ToggleObjects();
            ScoreDisplay.instance.DisplayScore();
        }
 
        private IEnumerator UpdateCoroutine()
        {
            while(!_stop)
            {
                timerImage.fillAmount = _currentTime / timeLeft;
                yield return new WaitForSeconds(0.2f);
            }
        }
        
        #endregion
    }
}
