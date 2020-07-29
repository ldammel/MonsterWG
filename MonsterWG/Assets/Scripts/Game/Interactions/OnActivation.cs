using Game.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactions
{
    public class OnActivation : MonoBehaviour
    {
        public UnityEvent onActivation;
        [HideInInspector]public PlayerInteractionController player;
        private Outline _outline;

        private void Start()
        {
            _outline = GetComponentInChildren<Outline>();
        }

        public void PickUp()
        {
            onActivation.Invoke();
        }
    }
}
