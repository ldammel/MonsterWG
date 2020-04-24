using UnityEngine;

namespace Game.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private Outline[] menuObjects;
        [SerializeField] private UIBehaviour backToMainBehaviour;
        private InputMaster _controls = null;
        private int _count;
        private void Awake() => _controls = new InputMaster();
        
        private void OnEnable()
        {
            _controls.Player.Enable();
            _controls.Player1.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
            _controls.Player1.Disable();
        }

        private void Start()
        {
            _controls.Player.Move.performed += _ => MoveElement(Color.blue);            
            _controls.Player1.Move.performed += _ => MoveElement(Color.green);
            
            _controls.Player.Select.performed += _ => SelectUIElement();
            _controls.Player1.Select.performed += _ => SelectUIElement();
            
            _controls.Player.Menu.performed += _ => BackToMenu();            
            _controls.Player1.Menu.performed += _ => BackToMenu();
        }

        private void BackToMenu()
        {
            backToMainBehaviour.Execute();
        }


        public void MoveElement(Color color)
        {
            var movementInput = _controls.Player.Move.ReadValue<Vector2>(); 
            var movementInput2 = _controls.Player1.Move.ReadValue<Vector2>();
            
            var movement = new Vector3
            {
                x = movementInput.x + movementInput2.x,
                z = movementInput.y + movementInput2.y
            };
            if (movement.x >= 1)
            {
                Debug.Log(_count);
                if (_count == menuObjects.Length-1)
                {
                    menuObjects[_count].Deselect();
                    _count = 0;
                    menuObjects[_count].Select(color);
                }
                else
                {
                    menuObjects[_count].Deselect();
                    _count++;
                    menuObjects[_count].Select(color);
                }
            }
            if (movement.x <= -1)
            {
                Debug.Log(_count);
                if (_count == 0)
                {
                    menuObjects[_count].Deselect();
                    _count = menuObjects.Length-1;
                    menuObjects[_count].Select(color);
                }
                else
                {
                    menuObjects[_count].Deselect();
                    _count--;
                    menuObjects[_count].Select(color);
                }
            }

        }

        private void SelectUIElement()
        {
            Debug.Log("Execute");
            menuObjects[_count].Activate();
        }
    }
}
