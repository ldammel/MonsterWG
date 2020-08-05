using System.Collections;
using Game.Utility;
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
        public bool bad;
        public bool geschirr;
        public bool bett;
        public bool wischen;

        protected Interaction _interaction;
        protected float _currentValue;
        protected bool _start;
        protected bool _canClose;

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
            if (_canClose) return;
            _start = true;
            _currentValue = 0;
            _interaction.player.character.canMove = false;
            uiObject.SetActive(true);
        }


        public virtual void EndMiniGame(bool success)
        {
            SoundManager.Instance.Play(gameObject, SoundManager.Sounds.InputCorrect);
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
        
        protected IEnumerator CheckClose(bool value)
        {
            yield return new WaitForSeconds(0.3f);
            _canClose = value;
        }

        protected virtual void OnStart() { }

    }
}
