using System;
using UnityEngine;

namespace Game.AI
{
    public class TimerCondition : StateTransitionCondition
    {
        [HideInInspector]public bool timerEnded; 
        [SerializeField] private float rotationSpeed;
        private bool _rotate;
        [SerializeField] private GameObject objectToRotate;
        public override bool IsMet()
        {
            return timerEnded;
        }

        public void Rotate()
        {
            timerEnded = false;
            _rotate = true;
        }

        public void StopRotation()
        {
            _rotate = false;
            timerEnded = true;
        }

        private void Update()
        {
            if (_rotate)
            {
                objectToRotate.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            }
        }
    }
}