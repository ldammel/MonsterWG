using Game.Character;
using Game.Utility;
using UnityEngine;

namespace Game.AI
{
    public class StateMachineBehaviour : MonoBehaviour
    {
        [SerializeField] private State startingState = null;
        public bool isCalled;
        [SerializeField] private bool canCall = true;
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
            if (isCalled) return;
            StateMachine.Tick();
        }

        public void Call(Transform target, float duration)
        {
            if (!canCall) return;
            isCalled = true;
            canCall = false;
            callTimer.duration = duration;
            if (!NavMeshWalker.instance.MiniQuest)walker.agent.isStopped = true;
            walker.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            callTimer.StartTimer();
            walker.gameObject.transform.LookAt(target);
        }

        public void StopCall()
        {
            if (!NavMeshWalker.instance.MiniQuest)walker.agent.isStopped = false;
            callTimer.EndTimer();
            walker.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            isCalled = false;
            canCallTimer.duration = 5;
            canCall = false;
            canCallTimer.StartTimer();
        }

        public void CanCall()
        {
            canCallTimer.EndTimer();
            canCall = true;
        }

        public void ChangeState(State state) => StateMachine.ChangeState(state);
    }
}