using UnityEngine;
using UnityEngine.Events;

namespace Game.Utility
{
    public class TriggerToggle : MonoBehaviour
    {
        public UnityEvent enterEvents;
        public UnityEvent exitEvents;
        public bool disable;

        private void OnTriggerEnter(Collider other)
        {
            if (disable) return;
            if (other.CompareTag("Untagged")) return;
            enterEvents.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (disable) return;
            exitEvents.Invoke();
        }

        public void SetBool(bool b)
        {
            disable = b;
        }
    }
}
