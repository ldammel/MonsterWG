using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactions
{
    public class TVMiniGame : MonoBehaviour
    {
        [SerializeField] private GameObject imageContainer;
        [SerializeField] private GameObject buttonOneColorImage;
        [SerializeField] private GameObject buttonTwoColorImage;
        [SerializeField] private GameObject buttonThreeColorImage;
        
        public UnityEvent onSuccess;
        public UnityEvent onClose;

        private bool _pressedPlaceButton;
        private bool _pressedCallButton;
        private PlayerInteractionController _player;
        private bool _active;
        private int _correctGuesses;
        private bool _started;
        private bool _closed;
        private bool _closing;
        
        void Start()
        {
            _correctGuesses = 0;
            imageContainer.SetActive(false);
        }

        void Update()
        {
            if(_closed) CloseMiniGame();
            if (!_active) return;

            if (_player.InputPickUp >= 1 && !_pressedPlaceButton)
            {
                _pressedPlaceButton = true;
                switch (_correctGuesses)
                {
                    case 0:
                        buttonOneColorImage.SetActive(false);
                        _correctGuesses++;
                        break;
                    case 1:
                        buttonOneColorImage.SetActive(true);
                        buttonTwoColorImage.SetActive(true);
                        buttonThreeColorImage.SetActive(true);
                        _correctGuesses = 0;
                        break;
                    case 2 :
                        buttonThreeColorImage.SetActive(false);
                        EndMiniGame();
                        break;
                    default:
                        break;
                }
            }
            else if(_player.InputPickUp <= 0 && _pressedPlaceButton) _pressedPlaceButton = false;
            

            if (_player.InputCall >= 1 && !_pressedCallButton)
            {
                _pressedCallButton = true;
                switch (_correctGuesses)
                {
                    case 0:
                        buttonOneColorImage.SetActive(true);
                        buttonTwoColorImage.SetActive(true);
                        buttonThreeColorImage.SetActive(true);
                        _correctGuesses = 0;
                        break;
                    case 1:
                        buttonTwoColorImage.SetActive(false);
                        _correctGuesses++;
                        break;
                    case 2 :
                        buttonOneColorImage.SetActive(true);
                        buttonTwoColorImage.SetActive(true);
                        buttonThreeColorImage.SetActive(true);
                        _correctGuesses = 0;
                        break;
                    default:
                        break;
                }
            }
            else if(_player.InputCall <= 0 && _pressedCallButton) _pressedCallButton = false;

            if (_player.InputInteraction >= 1 && _closing)
            {
                CloseMiniGame();
            }
        }

        public void StartMiniGame()
        {
            if (_started || _closed || _closing) return;
            StartCoroutine(Starting());
            _started = true;
            _active = true;
            _pressedPlaceButton = false;
            _pressedCallButton = false;
            _correctGuesses = 0;
            imageContainer.SetActive(true);
            if(_player)_player.character.canMove = false;
        }

        private void CloseMiniGame()
        {
            _active = false;
            _correctGuesses = 0;
            buttonOneColorImage.SetActive(true);
            buttonTwoColorImage.SetActive(true);
            buttonThreeColorImage.SetActive(true);
            imageContainer.SetActive(false);
            if(_player)_player.character.canMove = true;
            StartCoroutine(Closing());
        }

        public void EndMiniGame()
        {
            _active = false;
            _correctGuesses = 0;
            onSuccess.Invoke();
            imageContainer.SetActive(false);
            if(_player)_player.character.canMove = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerInteractionController>())
                _player = other.gameObject.GetComponent<PlayerInteractionController>();
        }
        
        private void OnTriggerExit(Collider other)
        {
            _player = null;
        }

        IEnumerator Closing()
        {
            _closed = true;
            yield return new WaitForSeconds(1);
            onClose.Invoke();
            _closed = false;
            _closing = false;
            _started = false;
        }
        
        IEnumerator Starting()
        {
            yield return new WaitForSeconds(1);
            _closing = true;
        }
        
    }
}
