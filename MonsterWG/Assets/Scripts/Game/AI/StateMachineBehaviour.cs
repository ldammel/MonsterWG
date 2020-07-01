using Game.Character;
using Game.Utility;
using UnityEngine;

namespace Game.AI
{
    public class StateMachineBehaviour : MonoBehaviour
    {
        [SerializeField] private State startingState = null;
        [SerializeField] private bool _isCalled;
        [SerializeField] private bool _canCall = true;
        [SerializeField] private TimerBehaviour callTimer;
        [SerializeField] private TimerBehaviour canCallTimer;
        [SerializeField] private NavMeshWalker walker;

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
            if (_isCalled) return;
            StateMachine.Tick();
        }

        public void Call(Transform target, float duration)
        {
            if (!_canCall) return;
            _isCalled = true;
            _canCall = false;
            callTimer.duration = duration;
            walker.agent.isStopped = true;
            callTimer.StartTimer();
            walker.gameObject.transform.LookAt(target);
        }

        public void StopCall()
        {
            callTimer.EndTimer();
            walker.agent.isStopped = false;
            _isCalled = false;
            canCallTimer.duration = 5;
            _canCall = false;
            canCallTimer.StartTimer();
        }

        public void CanCall()
        {
            canCallTimer.EndTimer();
            _canCall = true;
        }

        public void ChangeState(State state) => StateMachine.ChangeState(state);
    }
}