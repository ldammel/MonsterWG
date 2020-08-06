using System.Collections.Generic;
using Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Interactions
{
    public class JoyStickMiniGameRotate : JoyStickMiniGame
    {
        public enum Start
        {
            Top, Left, Bottom, Right
        }

        public Image circleImage;
        public Start startPoint;
        public bool rotateClockwise;
        public int checkAngle = 45;
        public float checkThreshold = 0.5f;
        public int neededRotations;

        private List<Vector2> _positions = new List<Vector2>();

        private int _currentIndex;
        private int _fullRotations;

        protected override void OnStart()
        {
            int start = 0;

            switch (startPoint)
            {
                case Start.Top:
                    start = 90;
                    circleImage.fillOrigin = 2;
                    break;
                case Start.Left:
                    start = 180;
                    circleImage.fillOrigin = 3;
                    break;
                case Start.Bottom:
                    start = 270;
                    circleImage.fillOrigin = 0;
                    break;
                case Start.Right:
                    start = 0;
                    circleImage.fillOrigin = 1;
                    break;
            }

            if (rotateClockwise)
            {
                for (int i = 360 + start -1; i >= start; i -= checkAngle)
                {
                    _positions.Add(new Vector2(checkThreshold * Mathf.Cos(Mathf.Deg2Rad * i), checkThreshold * Mathf.Sin(Mathf.Deg2Rad * i)));
                }
            }
            else
            {
                for (int i = start; i < 360 + start; i += checkAngle)
                {
                    _positions.Add(new Vector2(checkThreshold * Mathf.Cos(Mathf.Deg2Rad * i), checkThreshold * Mathf.Sin(Mathf.Deg2Rad * i)));
                }
            }
            circleImage.fillClockwise = rotateClockwise;

            _currentIndex = 0;
            _fullRotations = 0;
        }

        public override void StartMiniGame()
        {
            base.StartMiniGame();
            _currentIndex = 0;
            _fullRotations = 0;
        }

        private void Update()
        {
            if (!_start) return;

            if(_interaction.player == null)
            {
                EndMiniGame(false);
                return;
            }

            Vector2 input = _interaction.player.InputMove;

            bool interactionCounted = true;
            while (interactionCounted)
            {
                interactionCounted = CheckInput(input);

                circleImage.fillAmount = (float)(_currentIndex - 1) / (float)(_positions.Count - 1);
                progressBar.fillAmount = (float)_fullRotations / (float)neededRotations;

                if (_currentIndex >= _positions.Count)
                {
                    _fullRotations++;
                    _currentIndex = 0;
                    if(bad)SoundManager.Instance.Play(gameObject, SoundManager.Sounds.BadReinigen);
                    else if(geschirr)SoundManager.Instance.Play(gameObject, SoundManager.Sounds.GeschirrSpülen);
                }

                if (_fullRotations >= neededRotations)
                {
                    EndMiniGame(true);
                }
            }
        }

        private bool CheckInput(Vector2 input)
        {
            Vector2 nextPos = _positions[_currentIndex];

            if (nextPos.x < 0 && nextPos.y < 0)
            {
                if (input.x <= nextPos.x && input.y <= nextPos.y)
                {
                    _currentIndex++;
                    return true;
                }
            }
            else if (nextPos.x >= 0 && nextPos.y >= 0)
            {
                if (input.x >= nextPos.x && input.y >= nextPos.y)
                {
                    _currentIndex++;
                    return true;
                }
            }
            else if (nextPos.x < 0 && nextPos.y >= 0)
            {
                if (input.x <= nextPos.x && input.y >= nextPos.y)
                {
                    _currentIndex++;
                    return true;
                }
            }
            else if (nextPos.x >= 0 && nextPos.y < 0)
            {
                if (input.x >= nextPos.x && input.y <= nextPos.y)
                {
                    _currentIndex++;
                    return true;
                }
            }

            return false;
        }
    }
}
