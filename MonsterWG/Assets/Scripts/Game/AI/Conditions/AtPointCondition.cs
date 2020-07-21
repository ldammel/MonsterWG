using Game.Character;
using UnityEngine;

namespace Game.AI.Conditions
{
    public class AtPointCondition : StateTransitionCondition
    {
        [SerializeField] private NavMeshWalker walker;

        public override bool IsMet()
        {
            return walker.agent.isStopped && walker.canContinue && !walker.FinalWayPoint;
        }

        public void GoToNextPoint()
        {
            walker.agent.isStopped = false;
        }
    }
}