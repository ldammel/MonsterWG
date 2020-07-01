using UnityEngine;

namespace Game.AI.Conditions
{
    public class TimerCondition : StateTransitionCondition
    {
        [HideInInspector]public bool timerEnded; 
        [SerializeField] private float rotationSpeed;
        private bool _rotate;
        [SerializeField] private GameObject objectToRotate;
        
        private Quaternion localRotation;
        private bool _rotateLeft;
        public override bool IsMet()
        {
            return timerEnded;
        }

        public void Rotate()
        {
            timerEnded = false;
            _rotate = true;
            localRotation = objectToRotate.transform.localRotation;
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
                if (_rotateLeft)
                {
                    if (objectToRotate.transform.localRotation.eulerAngles.y >= localRotation.eulerAngles.y - 90)
                    {
                        objectToRotate.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
                    }
                    else _rotateLeft = false;
                }
                else
                {
                    if (objectToRotate.transform.localRotation.eulerAngles.y <= localRotation.eulerAngles.y + 90)
                    {
                        objectToRotate.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
                    }
                    else _rotateLeft = true;
                }
            }
        }
        
        
    }
}