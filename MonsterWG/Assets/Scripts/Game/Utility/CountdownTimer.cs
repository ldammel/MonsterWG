﻿using System;
using System.Collections;
using System.Timers;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
        public UnityEvent onFinish;

        private bool _stop = true;

        #endregion

        #region Functions
        
        private void OnEnable()
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

        public void ResetTimer()
        {
            _currentTime = 0;
        }

        private void Update() 
        {
            if(_stop) return;
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= timeLeft)) return;
            _stop = true;
            if(target)target.ToggleObjects();
            onFinish.Invoke();
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
