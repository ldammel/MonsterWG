using System.Collections;
using Game.Interactions;

namespace Game.AI.Conditions
{
    public class PutDownCondition : StateTransitionCondition
    {
        public Pickup pickup;
        private bool _timerEnded;
        public override bool IsMet()
        {
            if (!_timerEnded) return false;
            if(pickup.player)if(pickup.player.StoreInteraction) return false;
            if (pickup.player.HasInteraction) return false;
            return pickup.isInHand && pickup.pressedButton && !pickup.inTrigger;
        }

        public void StartTimer()
        {
            _timerEnded = false;
        }

        public void EndTimer()
        {
            _timerEnded = true;
        }
    }
}
