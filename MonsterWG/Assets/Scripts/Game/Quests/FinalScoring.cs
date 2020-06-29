using System.Collections.Generic;
using Game.UI;
using UnityEngine;

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
        public float timeRateOfDecay;

        private bool _start;

        private void Update()
        {
            if (!_start) return;
            if (timeTillEnd > 0)
            {
                timeTillEnd -= Time.deltaTime * timeRateOfDecay;
            }
            else
            {
                //finish
            }
        }

        public void StartFinalScoring()
        {
            previousPointAmount = ScoreDisplay.instance.Score;
            timeTillEnd = previousPointAmount * 0.3f;
            foreach (var q in miniQuests)
            {
                q.miniQuestReward = (timeTillEnd / miniQuests.Count);
            }
            _start = true;
        }

        public void AddScore(int score)
        {
            finalScore += score;
        }
    }
}
