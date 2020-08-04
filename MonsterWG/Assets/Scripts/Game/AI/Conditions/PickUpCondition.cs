using Game.Interactions;

namespace Game.AI.Conditions
{
    public class PickUpCondition : StateTransitionCondition
    {
        public Pickup pickup;
        private bool _inHand;
        public override bool IsMet()
        {
            if (pickup.player) _inHand = pickup.player.CurrentItem;
            return pickup.inTrigger && pickup.pressedButton && !_inHand;
        }
    }
}
