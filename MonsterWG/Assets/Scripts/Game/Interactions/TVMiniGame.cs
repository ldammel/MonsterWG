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
        
        private bool _pressedPlaceButton;
        private bool _pressedCallButton;
        private PlayerInteractionController _player;
        private bool _active;
        private int _correctGuesses;
        
        void Start()
        {
            _correctGuesses = 0;
            imageContainer.SetActive(false);
        }

        void Update()
        {
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

        }

        public void StartMiniGame()
        {
            _active = true;
            _pressedPlaceButton = false;
            _pressedCallButton = false;
            _correctGuesses = 0;
            imageContainer.SetActive(true);
            _player.character.canMove = false;
        }
        
        public void EndMiniGame()
        {
            _active = false;
            _correctGuesses = 0;
            onSuccess.Invoke();
            imageContainer.SetActive(false);
            _player.character.canMove = true;
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
    }
}
