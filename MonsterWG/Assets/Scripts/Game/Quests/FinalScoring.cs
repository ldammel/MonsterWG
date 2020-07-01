using System;
using System.Collections.Generic;
using Game.UI;
using Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Quests
{
    public class FinalScoring : MonoBehaviour
    {
        #region Singleton
        public static FinalScoring instance;
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("There can only be one instance of FinalScoring in the Scene!");
                Application.Quit();
            }
            instance = this;
        }
        #endregion
    
        [SerializeField] private int finalScore;
        [SerializeField] private int bonusPoints;
        [SerializeField] private float timeTillEnd;
        [SerializeField] private float pointRateOfDecay;
        [SerializeField] private int previousPointAmount;
        [SerializeField] private List<Quest> miniQuests;
        [SerializeField] private Image timerImage;
        [SerializeField] private CountdownTimer timer;
        public float timeRateOfDecay;
        
        private float _startTime;
        private bool _start;

        private void Start()
        {
            StartFinalScoring();
        }

        private void Update()
        {
            if (!_start) return;
            if (timeTillEnd > 0)
            {
                timeTillEnd -= Time.deltaTime * timeRateOfDecay;
                timerImage.fillAmount = timeTillEnd / _startTime;
            }
            else
            {
                EndScoring();
            }
        }

        public void StartFinalScoring()
        {
            previousPointAmount = ScoreDisplay.instance.Score;
            timeTillEnd = previousPointAmount * 0.3f;
            _startTime = timeTillEnd;
            foreach (var q in miniQuests)
            {
                q.miniQuestReward = (timeTillEnd / miniQuests.Count);
            }
            _start = true;
        }

        public void EndScoring()
        {
            if (timeTillEnd > 0)
            {
                finalScore = Mathf.RoundToInt(timeTillEnd) + bonusPoints;
                ScoreDisplay.instance.AddScore(Mathf.RoundToInt(finalScore));
            }

            timer.currentTime = timer.timeLeft;
            _start = false;
        }
    }
}
