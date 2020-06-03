using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Utility
{
    public class EnableDisableBehaviour : MonoBehaviour
    {

        public UnityEvent onEnableEvents;
        public UnityEvent onDisableEvents;

        private void OnEnable()
        {
            onEnableEvents.Invoke();
        }

        private void OnDisable()
        {
            onDisableEvents.Invoke();
        }
    }
}
