using Game.Interactions;

namespace Game.AI.Conditions
{
    public class InteractCondition : StateTransitionCondition
    {
        public Interaction interaction;
        public bool timerEnd;
        public bool useTimer;

        public override bool IsMet()
        {
            if(useTimer) return interaction.endHold && timerEnd && interaction.pressedButton;
            else return interaction.endHold && interaction.pressedButton;
        }

        public void StartTimer()
        {
            timerEnd = false;
        }

        public void EndTimer()
        {
            timerEnd = false;
        }

    }
}
