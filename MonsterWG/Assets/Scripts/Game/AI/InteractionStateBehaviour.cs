using Sirenix.Utilities;
using UnityEngine;
namespace Game.AI
{
    public class InteractionStateBehaviour : MonoBehaviour
    {
        [SerializeField] private State startingState = null;
        [SerializeField] private GameObject[] stateObjects;

        private StateMachine _stateMachine;
        private StateMachine StateMachine
        {
            get
            {
                if (_stateMachine != null) { return _stateMachine; }
                _stateMachine = new StateMachine(startingState);
                return _stateMachine;
            }
        }

        private void Update()
        {
            StateMachine.Tick();
        }

        public void ChangeState(State state) => StateMachine.ChangeState(state);

        public void ResetStates()
        {
            for (int i = 0; i < stateObjects.Length; i++)
            { 
                stateObjects[i].SetActive(i == 0);
            }
            ChangeState(stateObjects[0].GetComponent<State>());
        }
    }
}
