using Game.Character;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactions
{
    public class OnActivation : MonoBehaviour
    {
        public UnityEvent onActivation;

        private PlayerInteractionController _player;
        private bool _isInTrigger;
        public void PickUp()
        {
            if (_player == null) return;
            if (!_player.isInTrigger) return;
            onActivation.Invoke();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_player != null) return;
            if (other.CompareTag("Untagged")) return;
            _player = other.GetComponent<PlayerInteractionController>();
            if (_player.isPlayerOne)
            {
                _player.character.controls.Player.Select.Enable();
                _player.character.controls.Player.Select.performed += _ => PickUp();
            }
            else
            {
                _player.character.controls.Player2.Select.Enable();
                _player.character.controls.Player2.Select.performed += _ => PickUp();
            }
            _player.interactImage.SetActive(true);
            _player.isInTrigger = true;
        }
        private void OnTriggerExit(Collider other)
        {
            if (_player == null) return;
            _player.character.controls.Player.Select.Disable();
            _player.character.controls.Player2.Select.Disable();
            _player.interactImage.SetActive(false);
            _player.isInTrigger = false;
            _player = null;
        }
    }
}
