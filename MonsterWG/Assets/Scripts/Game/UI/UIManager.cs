using System;
using System.Collections;
using Game.Character;
using Game.Interactions;
using Game.Utility;
using UnityEngine;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private CountdownTimer timer;

        private PlayerInteractionController _character;
        private PlayerInteractionController _character1;
        private bool _pressed;
        private bool _open;

        private void Start()
        {
            _character = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInteractionController>();
            _character1 = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<PlayerInteractionController>();
        }

        private void Update()
        {
            if (!_pressed)
            {
                if (_character.InputMenu >= 1 && !_open)
                {
                    OnMenuPressed(true);
                    _open = true;
                }
                else if (_character.InputMenu <= 0 && _open)
                {
                    _pressed = true;
                    _open = false;
                }
            }
            else if (_pressed)
            {
                if (_character.InputMenu >= 1 && !_open)
                {
                    OnMenuPressed(false);
                    _open = true;
                }
                else if (_character.InputMenu <= 0 && _open)
                {
                    _pressed = false;
                    _open = false;
                }
            }
        }


        private void OnMenuPressed(bool value)
        {
            pauseMenu.SetActive(value);
            _character.character.canMove = !value;
            _character1.character.canMove = !value;
            if(_character.character.canMove) timer.StartTimer();
            else timer.StopTimer();
        }
    }
}
