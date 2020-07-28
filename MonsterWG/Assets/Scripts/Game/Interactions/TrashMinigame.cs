using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Interactions
{
    public class TrashMinigame : MonoBehaviour
    {

        [FoldoutGroup("Timing MiniGame")] [SerializeField] private GameObject timingObject;
        [FoldoutGroup("Timing MiniGame")] [SerializeField] private Image fillBar;
        [FoldoutGroup("Timing MiniGame")] [SerializeField] private float fillSpeed;
        
        [FoldoutGroup("Mashing MiniGame")] [SerializeField] private int neededMashAmount;
        [FoldoutGroup("Mashing MiniGame")] [SerializeField] private int mashedAmount;
        [FoldoutGroup("Mashing MiniGame")] [SerializeField] private GameObject buttonObject;

        public UnityEvent onSuccess;
        
        private bool _active;
        private float _currentValue = 0;
        private PlayerInteractionController _player;
        private bool _left;
        private bool _timing;
        private bool _pressed;

        private void Start()
        {
            _active = false;
            _currentValue = 0;
            mashedAmount = 0;
            buttonObject.SetActive(false);
            timingObject.SetActive(false);
        }

        private void Update()
        {
            if (!_active || !_player) return;
            if(_player.InputMenu >= 1) EndMiniGame();
            if (_timing)
            {
                if (_left)
                {
                    _currentValue -= fillSpeed * Time.deltaTime;
                    fillBar.fillAmount = _currentValue;
                    if (_currentValue <= 0) _left = false;
                }
                else
                {
                    _currentValue += fillSpeed * Time.deltaTime;
                    fillBar.fillAmount = _currentValue;
                    if (_currentValue >= 1) _left = true;
                }

                if (_player.InputI >= 1f && !_pressed)
                {
                    _pressed = true;
                    if (_currentValue >= 0.5f && _currentValue <= 0.75f) EndMiniGame();
                    else
                    {
                        _currentValue = 0;
                        _left = false;
                        _pressed = false;
                    }
                }
            }
            else
            {
                if (_player.InputI >= 1f && !_pressed)
                {
                    _pressed = true;
                    if (mashedAmount <= neededMashAmount)mashedAmount++;
                    else EndMiniGame();
                }
                else if(_player.InputI <= 0f && _pressed)
                {
                    _pressed = false;
                }
            }
        }

        public void StartTimingMiniGame()
        {
            _active = true;
            _currentValue = 0;
            _timing = true;
            _left = false;
            _pressed = false;
            timingObject.SetActive(true);
            _player.character.canMove = false;
        }
        
        public void StartMashingMiniGame()
        {
            _active = true;
            buttonObject.SetActive(true);
            mashedAmount = 0;
            _timing = false;
            _pressed = false;
            _player.character.canMove = false;
        }
        
        public void EndMiniGame()
        {
            _active = false;
            _timing = false;
            onSuccess.Invoke();
            buttonObject.SetActive(false);
            timingObject.SetActive(false);
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
