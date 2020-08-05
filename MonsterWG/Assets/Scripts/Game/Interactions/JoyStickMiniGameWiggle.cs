using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Interactions
{
    public class JoyStickMiniGameWiggle : JoyStickMiniGame
    {
        [SerializeField] protected GameObject firstArrow;
        [SerializeField] protected GameObject secondArrow;
        [SerializeField] protected bool horizontal;

        private bool _switch;

        protected override void OnStart()
        {
            _switch = false;
        }

        private void Update()
        {
            if (!_start) return;
            float input = _interaction.player.InputMove[horizontal ? 0 : 1];

            if (_switch)
            {
                firstArrow.SetActive(true);
                secondArrow.SetActive(false);

                if (input <= -inputThreshold)
                {
                    _currentValue += 0.1f;
                    _switch = false;
                }
                else if (_currentValue > 0)
                {
                    _currentValue -= 0.01f * Time.deltaTime;
                }
            }
            else
            {
                firstArrow.SetActive(false);
                secondArrow.SetActive(true);
                if (input >= inputThreshold)
                {
                    _currentValue += 0.1f;
                    _switch = true;
                }
                else if (_currentValue > 0)
                {
                    _currentValue -= 0.01f * Time.deltaTime;
                }
            }
            progressBar.fillAmount = 1 * _currentValue;

            if (!(_currentValue >= 1)) return;
            EndMiniGame(true);
        }

        public override void StartMiniGame()
        {
            base.StartMiniGame();
        }
    }
}
