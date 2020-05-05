using Game.Character;
using Game.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Game.Interactions
{
    public class Pickup : BaseInteraction
    {
        [SerializeField] private Transform[] handGrabPosition;
        [SerializeField] private GameObject spawnPoint;
        [SerializeField] private Transform baseParent;
        [SerializeField] private Sprite itemIcon;
        [SerializeField] private Image[] playerInvImage;
        [SerializeField] private bool respawn;
        [SerializeField] private GameObject interactionTarget;
        public UnityEvent onPickUp;
        private Transform _handGrabPosition;
        private bool _isInHand = false;
        private int _currentPlayer;

        public override void PickUp(int player)
        {
            if (isPlayerOne && player != 1) return;
            if (!isPlayerOne && player != 2) return;
            if (_isInHand) CancelPickUp(_currentPlayer);
            if (!isInTrigger && !_isInHand) return;
            _currentPlayer = isPlayerOne ? 1 : 2;
            _handGrabPosition = isPlayerOne ? handGrabPosition[0] : handGrabPosition[1];
            playerInvImage[_currentPlayer-1].sprite = itemIcon;
            playerInvImage[_currentPlayer-1].gameObject.SetActive(true);
            interactionTarget.GetComponent<Rigidbody>().useGravity = false;
            interactionTarget.transform.position = _handGrabPosition.position;
            interactionTarget.transform.parent = _handGrabPosition;
            _isInHand = true;
            onPickUp.Invoke();
        }

        public void CancelPickUp(int player)
        {
            playerInvImage[player-1].sprite = null;
            playerInvImage[player-1].gameObject.SetActive(false);
            if (_currentPlayer != player) return;
            if (!isInTrigger && !_isInHand) return;
            interactionTarget.GetComponent<Rigidbody>().useGravity = true;
            interactionTarget.transform.parent = baseParent;
            _isInHand = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Untagged")) return;
            if (other.CompareTag("PickupDest") && !_isInHand)
            {
                if (respawn)
                {
                    ResetPosition();
                }
                other.gameObject.GetComponent<ItemCollector>().collectedItems.Add(this);
                ScoreDisplay.instance.AddScore(scoreGain);
                onComplete.Invoke();
            }

            isPlayerOne = other.CompareTag("Player");
            interactImage.SetActive(true);
            isInTrigger = true;
        }

        public void ResetPosition()
        {
            interactionTarget.transform.position = spawnPoint.transform.position;
            interactionTarget.transform.rotation = spawnPoint.transform.rotation;
        }

        private void OnTriggerExit(Collider other)
        {
            interactImage.SetActive(false);
            isInTrigger = false;
        }
    }
}
