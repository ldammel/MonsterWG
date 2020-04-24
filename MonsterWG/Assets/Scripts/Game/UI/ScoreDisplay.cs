using System;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        public static ScoreDisplay instance;
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("There can only be one instance of ScoreDisplay!");
                Application.Quit();
            }

            instance = this;
        }

        [SerializeField] private int score;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject[] fullStars = new GameObject[5];
        [SerializeField] private int[] neededStarScore = new int[5];

        private void OnEnable()
        {
            foreach (var star in fullStars)
            {
                star.SetActive(false);
            }
        }

        public void AddScore(int scoreAdd)
        {
            score += scoreAdd;
        }

        public void DisplayScore()
        {
            scoreText.text = score.ToString();
            for (int i = 0; i < neededStarScore.Length; i++)
            {
                if (score >= neededStarScore[i])
                {
                    fullStars[i].SetActive(true);
                }
                else return;
            }
        }
    }
}
