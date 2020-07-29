namespace Game.AI.Conditions
{
    public class WaitCondition : StateTransitionCondition
    {
        private bool _timerEnded;

        public override bool IsMet()
        {
            return _timerEnded;
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
