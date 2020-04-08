using System;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
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

            CalculateScore();
        }

        private void CalculateScore()
        {
            //DoCalculation
            DisplayScore();
        }

        private void DisplayScore()
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
