using Game.UI;
using UnityEngine;


namespace Game.Interactions
{
    public class Pickup : BaseInteraction
    {
        private Transform _handGrabPosition;
        [SerializeField] private Transform handGrabPosition1;
        [SerializeField] private Transform handGrabPosition2;
        [SerializeField] private GameObject spawnPoint;
        [SerializeField] private int scoreGain = 10000;
        private bool _isInHand = false;
        private Transform _baseParent;

        public override void Interact()
        {
            if (!_isInTrigger && !_isInHand) return;
            if (!_isInHand)
            {
                PickupObject();
            }
            else
            {
                PutDown();
            }

            _isInHand = !_isInHand;
        }

        private void PickupObject()
        {
            _baseParent = interactionTarget.transform.parent;
            interactionTarget.GetComponent<Rigidbody>().useGravity = false;
            interactionTarget.transform.position = _handGrabPosition.position;
            interactionTarget.transform.parent = _handGrabPosition;
        }

        private void PutDown()
        {
            interactionTarget.GetComponent<Rigidbody>().useGravity = true;
            interactionTarget.transform.parent = _baseParent;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PickupDest") && !_isInHand)
            {
                interactionTarget.transform.position = spawnPoint.transform.position;
                interactionTarget.transform.rotation = spawnPoint.transform.rotation;
                ScoreDisplay.instance.AddScore(scoreGain);
            }

            if (other.CompareTag("Untagged")) return;
            
            _handGrabPosition = other.CompareTag("Player2") ? handGrabPosition2 : handGrabPosition1;
            interactImage.SetActive(true);
            _isInTrigger = true;
        }
        private void OnTriggerExit(Collider other)
        {
            interactImage.SetActive(false);
            _isInTrigger = false;
        }
    }
}
