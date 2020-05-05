using Game.UI;
using UnityEngine;

namespace Game.Interactions
{
    public class OnActivation : BaseInteraction
    {
        private bool _isInTrigger;
        public override void PickUp(int player)
        {
            if (isPlayerOne && player != 1) return;
            if (!isPlayerOne && player != 2) return;
            if (!isInTrigger) return;
            onComplete.Invoke();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Untagged")) return;
            isPlayerOne = other.CompareTag("Player");
            interactImage.SetActive(true);
            isInTrigger = true;
        }
        private void OnTriggerExit(Collider other)
        {
            interactImage.SetActive(false);
            isInTrigger = false;
        }
    }
}
