using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Interactions
{
    public class JoyStickMiniGame : MonoBehaviour
    {
        [SerializeField] private Image progressBar;
        [SerializeField] private GameObject leftArrow;
        [SerializeField] private GameObject rightArrow;
        [SerializeField] private GameObject uiObject;
        public UnityEvent onFinish;
        private Interaction _interaction;
        private float _currentValue;
        private bool _start;
        private bool _left;

        private void Start()
        {
            _left = false;
            _start = false;
            uiObject.SetActive(false);
            _currentValue = 0;
            _interaction = gameObject.GetComponent<Interaction>();
        }

        private void Update()
        {
            if (!_start) return;
            if(Input.GetKeyDown(KeyCode.Escape)) EndMiniGame();
            if (_left)
            {
                leftArrow.SetActive(true);
                rightArrow.SetActive(false);
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    _currentValue += 0.1f;
                    _left = false;
                }
                else if(_currentValue > 0)
                {
                    _currentValue -= 0.01f * Time.deltaTime;
                }
            }
            else
            {             
                leftArrow.SetActive(false);
                rightArrow.SetActive(true);
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    _currentValue += 0.1f;
                    _left = true;
                }
                else if(_currentValue > 0)
                {
                    _currentValue -= 0.01f * Time.deltaTime;
                }
            }
            progressBar.fillAmount = 1 / _currentValue;

            if (!(_currentValue >= 1)) return;
            EndMiniGame();
            onFinish.Invoke();
        }

        public void StartMiniGame()
        {
            _start = true;
            _currentValue = 0;
            _interaction.player.character.canMove = false;
            uiObject.SetActive(true);
        }
        
        public void EndMiniGame()
        {
            _start = false;
            uiObject.SetActive(false);
            _interaction.player.character.canMove = true;
        }
    }
}
