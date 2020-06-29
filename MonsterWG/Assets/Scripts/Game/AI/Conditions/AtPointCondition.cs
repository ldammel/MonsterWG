using Game.Character;
using UnityEngine;

namespace Game.AI
{
    public class AtPointCondition : StateTransitionCondition
    {
        [SerializeField] private NavMeshWalker walker;

        public override bool IsMet()
        {
            return walker._agent.isStopped;
        }

        public void GoToNextPoint()
        {
            walker._agent.isStopped = false;
        }
    }
}