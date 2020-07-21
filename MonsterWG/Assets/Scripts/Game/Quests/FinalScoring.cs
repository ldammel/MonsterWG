using System;
using System.Collections.Generic;
using Game.Character;
using Game.UI;
using Game.Utility;
using UnityEngine;
using UnityEngine.Events;
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
        [SerializeField] private GameObject timerBar;
        public float timeRateOfDecay;
        
        private float _startTime;
        private bool _start;
        public Quest currentQuest; 
        

        private void Start()
        {
            StartFinalScoring();
            foreach (var q in miniQuests)
            {
                q.isDone = false;
                q.hasCheated = false;
                q.hasFailed = false;
                q.isRewarded = false;
                q.hasAmount = 0;
            }
        }

        private void Update()
        {
            if (!_start) return;
            if (timeTillEnd > 0)
            {
                timeTillEnd -= Time.deltaTime * timeRateOfDecay;
                timerImage.fillAmount = timeTillEnd / _startTime;
                if (currentQuest.isDone)
                {
                    EndScoring();
                }
            }
            else
            {
                QuestFailed();
            }
        }

        public void StartMiniQuest()
        {
            _start = true;
            timeTillEnd = previousPointAmount * 0.3f;
            _startTime = timeTillEnd;
            currentQuest = miniQuests[0];
            timerBar.SetActive(true);
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
        }

        public void QuestFailed()
        {
            timer.currentTime = timer.timeLeft;
            _start = false;
            currentQuest.hasFailed = true;
            currentQuest.isRewarded = true;
            timerBar.SetActive(false);
            NavMeshWalker.instance.MiniQuest = false;
        }

        public void EndScoring()
        {
            if (timeTillEnd > 0)
            {
                finalScore = Mathf.RoundToInt(timeTillEnd) + bonusPoints;
                ScoreDisplay.instance.AddScore(Mathf.RoundToInt(finalScore));
            }
            timerBar.SetActive(false);
            timer.currentTime = timer.timeLeft;
            _start = false;
            NavMeshWalker.instance.MiniQuest = false;
        }
    }
}
