using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Interactions
{
    public class Interaction : BaseInteraction
    {
        [SerializeField] private float duration = 5f;
        [SerializeField] private Image timerImage;
        [SerializeField] private List<GameObject> preRequisiteGameObjects;
        [SerializeField] private int scoreGain = 10000;
        private float _startTime;
        private bool _stop = true;

        public override void Interact()
        {
            if (!_isInTrigger) return;
            if (preRequisiteGameObjects.Any(go => !go.activeSelf))
            {
                return;
            }
            StartTimer();
        }

        private void Update()
        {
            if(_stop) return;
            _startTime += Time.deltaTime;
            if (!(_startTime >= duration)) return;
            _stop = true;
            ScoreDisplay.instance.AddScore(scoreGain);
        }

        public void StartTimer()
        {
            _stop = false;
            StartCoroutine(UpdateCoroutine());
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Untagged")) return;

            interactImage.SetActive(true);
            _isInTrigger = true;
        }
        private void OnTriggerExit(Collider other)
        {
            interactImage.SetActive(false);
            _isInTrigger = false;
        }
        
        private IEnumerator UpdateCoroutine()
        {
            while(!_stop)
            {
                timerImage.fillAmount = _startTime / duration;
                yield return new WaitForSeconds(0.2f);
            }
        }

    }
}
