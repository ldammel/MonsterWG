using Game.Character;
using UnityEngine;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject settingsMenu;

        private CharacterMovement _character;
        private CharacterMovement _character1;

        private void Start()
        {
            _character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
            _character1 = GameObject.FindGameObjectWithTag("Player2").GetComponent<CharacterMovement>();
            _character.controls.Player.Menu.performed += _ => OnMenuPressed();
            _character1.controls.Player2.Menu.performed += _ => OnMenuPressed();
        }

        private void OnMenuPressed()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            settingsMenu.SetActive(false);
        }
    }
}
