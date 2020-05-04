using System.Collections;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Interactions
{
    public class Interaction : BaseInteraction
    {
        [SerializeField] private float duration = 5f;
        [SerializeField] private Image timerImage;
        [SerializeField] private GameObject timerbase;
        [SerializeField] private bool saveProgress;
        private float _startTime;
        private bool _stop = true;
        private bool _isDone;


        public override void Interact(int player)
        {
            if (_isDone) return;
            if (isPlayerOne && player != 1) return;
            if (!isPlayerOne && player != 2) return;
            if (!isInTrigger) return;
            StartTimer();
            timerbase.SetActive(true);
        }

        public override void Cancel(int player)
        {
            if (isPlayerOne && player != 1) return;
            if (!isPlayerOne && player != 2) return;
            _stop = true;
            if (!saveProgress)
            {
                _startTime = 0;
            }
            timerbase.SetActive(false);
        }

        private void Update()
        {
            if(_stop) return;
            _startTime += Time.deltaTime;
            if (!(_startTime >= duration)) return;
            _stop = true;
            ScoreDisplay.instance.AddScore(scoreGain);
            timerbase.SetActive(false);
            onComplete.Invoke();
            _isDone = true;
        }

        public void StartTimer()
        {
            _stop = false;
            StartCoroutine(UpdateCoroutine());
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Untagged")) return;
            isPlayerOne = other.CompareTag("Player");
            interactImage.SetActive(true);
            isInTrigger = true;
        }
        private void OnTriggerExit(Collider other)
        {
            interactImage.SetActive(false);
            isInTrigger = false;
            timerbase.SetActive(false);
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
