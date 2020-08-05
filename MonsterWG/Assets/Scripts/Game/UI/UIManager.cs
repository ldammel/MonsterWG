using Game.Character;
using Game.Utility;
using UnityEngine;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private CountdownTimer timer;

        private CharacterMovement _character;
        private CharacterMovement _character1;

        private void Start()
        {
            _character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
            _character1 = GameObject.FindGameObjectWithTag("Player2").GetComponent<CharacterMovement>();
            _character.controls.Player.Menu.performed += _ => OnMenuPressed();
        }

        private void OnMenuPressed()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            settingsMenu.SetActive(false);
            _character.canMove = !_character.canMove;
            _character1.canMove = !_character1.canMove;
            if(_character.canMove) timer.StartTimer();
            else timer.StopTimer();
        }
    }
}
