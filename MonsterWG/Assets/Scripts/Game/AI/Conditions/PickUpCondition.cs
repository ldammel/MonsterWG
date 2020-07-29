using Game.Interactions;

namespace Game.AI.Conditions
{
    public class PickUpCondition : StateTransitionCondition
    {
        public Pickup pickup;
        public override bool IsMet()
        {
            return pickup.inTrigger && pickup.pressedButton && !pickup.isInHand;
        }
    }
}
