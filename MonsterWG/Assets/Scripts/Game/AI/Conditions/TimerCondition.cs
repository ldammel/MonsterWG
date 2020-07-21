using Game.Character;
using UnityEngine;

namespace Game.AI.Conditions
{
    public class TimerCondition : StateTransitionCondition
    {
        [HideInInspector]public bool timerEnded; 
        [SerializeField] private float rotationSpeed;
        private bool _rotate;
        [SerializeField] private GameObject objectToRotate;
        [SerializeField] private StateMachineBehaviour behaviour;
        
        private Quaternion localRotation;
        private bool _rotateLeft;
        public override bool IsMet()
        {
            return timerEnded && !behaviour.isCalled;
        }

        public void Rotate()
        {
            timerEnded = false;
            if(!NavMeshWalker.instance.MiniQuest)_rotate = true;
            localRotation = objectToRotate.transform.localRotation;
        }

        public void StopRotation()
        {
            _rotate = false;
            timerEnded = true;
        }

        private void Update()
        {
            if (_rotate && !behaviour.isCalled && !NavMeshWalker.instance.MiniQuest)
            {
                if (_rotateLeft)
                {
                    if (objectToRotate.transform.localRotation.eulerAngles.y >= localRotation.eulerAngles.y - 60)
                    {
                        objectToRotate.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
                    }
                    else _rotateLeft = false;
                }
                else
                {
                    if (objectToRotate.transform.localRotation.eulerAngles.y <= localRotation.eulerAngles.y + 60)
                    {
                        objectToRotate.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
                    }
                    else _rotateLeft = true;
                }
            }
        }
        
        
    }
}