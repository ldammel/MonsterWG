using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Interactions
{
    public class JoyStickMiniGame : MonoBehaviour
    {
        [SerializeField] protected Image progressBar;

        [SerializeField] protected GameObject uiObject;
        [SerializeField] protected float inputThreshold = 0.5f;
        public UnityEvent onFinish;

        protected Interaction _interaction;
        protected float _currentValue;
        protected bool _start;

        private CleaningCondition _condition;

        private void Start()
        {
            _start = false;
            uiObject.SetActive(false);
            _currentValue = 0;
            _interaction = gameObject.GetComponent<Interaction>();
            _condition = gameObject.GetComponent<CleaningCondition>();

            OnStart();
        }
        
        public virtual void StartMiniGame()
        {
            _start = true;
            _currentValue = 0;
            _interaction.player.character.canMove = false;
            uiObject.SetActive(true);
        }


        public virtual void EndMiniGame(bool success)
        {
            _start = false;
            uiObject.SetActive(false);
            _interaction.player.character.canMove = true;
            if (!success)
            {
                return;
            }

            _interaction.SetDone();
            
            if(_interaction.player.CurrentItem && _interaction.player.CurrentItem.NeedsWater)
            {
                _interaction.player.CurrentItem.CurrentWaterAmount -= _condition.NeededWaterAmount;
                Mathf.Clamp(_interaction.player.CurrentItem.CurrentWaterAmount, 0, 100);
            }
            onFinish.Invoke();
        }

        protected virtual void OnStart() { }

    }
}
