using System;
using System.Collections;
using Game.Interactions;
using UnityEngine;

namespace Game.Utility
{
    public class PlanStartUp : MonoBehaviour
    {
        public static PlanStartUp Instance;

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField] private GameObject planCanvas;
        private PlayerInteractionController _player1;
        private PlayerInteractionController _player2;
        private bool _open;

        private void Update()
        {
            if (!_open) return;
            if (_player1.InputInteraction <= 0) return;
            _open = false;
            planCanvas.SetActive(false);
            _player1.character.canMove = true;
            _player2.character.canMove = true;
        }

        public void Startup(PlayerInteractionController player)
        {
            StartCoroutine(Starting(player));
        }

        private IEnumerator Starting(PlayerInteractionController player)
        {
            yield return new WaitForSeconds(0.3f);
            if (!_player1) _player1 = player;
            else _player2 = player;
            _player1.character.canMove = false;
            if(_player2)_player2.character.canMove = false;
            planCanvas.SetActive(true);
            _open = true;
        }
    }
}
