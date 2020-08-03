using System;
using Sirenix.OdinInspector;
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

        [FoldoutGroup("Score")][SerializeField] private int score;
        [FoldoutGroup("TV")][SerializeField] private TextMeshProUGUI tvText;
        [FoldoutGroup("TV")][SerializeField] private int tvCount;
        [FoldoutGroup("TV")][SerializeField] private int tvReward;
        [FoldoutGroup("Müll")][SerializeField] private TextMeshProUGUI müllText;
        [FoldoutGroup("Müll")][SerializeField] private int müllCount;
        [FoldoutGroup("Müll")][SerializeField] private int müllReward;
        [FoldoutGroup("Geschirr")][SerializeField] private TextMeshProUGUI geschirrText;
        [FoldoutGroup("Geschirr")][SerializeField] private int geschirrCount;
        [FoldoutGroup("Geschirr")][SerializeField] private int geschirrReward;
        [FoldoutGroup("Blumen")][SerializeField] private TextMeshProUGUI blumenText;
        [FoldoutGroup("Blumen")][SerializeField] private int blumenCount;
        [FoldoutGroup("Blumen")][SerializeField] private int blumenReward;
        [FoldoutGroup("Bett")][SerializeField] private TextMeshProUGUI bettText;
        [FoldoutGroup("Bett")][SerializeField] private int bettCount;
        [FoldoutGroup("Bett")][SerializeField] private int bettReward;
        [FoldoutGroup("Wischen")][SerializeField] private TextMeshProUGUI wischenText;
        [FoldoutGroup("Wischen")][SerializeField] private int wischenCount;
        [FoldoutGroup("Wischen")][SerializeField] private int wischenReward;
        [FoldoutGroup("Bad")][SerializeField] private TextMeshProUGUI badText;
        [FoldoutGroup("Bad")][SerializeField] private int badCount;
        [FoldoutGroup("Bad")][SerializeField] private int badReward;
        public int Score => score;
        [FoldoutGroup("Score")][SerializeField] private TextMeshProUGUI scoreText;

        private void Start()
        {
            tvText.text = "x" + tvCount;
            müllText.text = "x" + müllCount;
            geschirrText.text = "x" + geschirrCount;
            blumenText.text = "x" + blumenCount;
            wischenText.text = "x" + wischenCount;
            bettText.text = "x" + bettCount;
            badText.text = "x" + badCount;
            scoreText.text = score.ToString();
        }

        public void AddScore(int scoreAdd)
        {
            score += scoreAdd;
        }

        public void FinishQuest(string type)
        {
            switch (type)
            {
                case "TV":
                    if (tvCount != 0)
                    {
                        tvCount--;
                        tvText.text = "x" + tvCount;
                        AddScore(tvReward);
                    }
                    break;
                case "MÜLL":
                    if (müllCount != 0)
                    {
                        müllCount--;
                        müllText.text = "x" + müllCount;
                        AddScore(müllReward);
                    }
                    break;
                case "GESCHIRR":
                    if (geschirrCount != 0)
                    {
                        geschirrCount--;
                        geschirrText.text = "x" + geschirrCount;
                        AddScore(geschirrReward);
                    }
                    break;
                case "BLUMEN":
                    if (blumenCount != 0)
                    {
                        blumenCount--;
                        blumenText.text = "x" + blumenCount;
                        AddScore(blumenReward);
                    }
                    break;
                case "BETT":
                    if (bettCount != 0)
                    {
                        bettCount--;
                        bettText.text = "x" + bettCount;
                        AddScore(bettReward);
                    }
                    break;
                case "WISCHEN":
                    if (wischenCount != 0)
                    {
                        wischenCount--;
                        wischenText.text = "x" + wischenCount;
                        AddScore(wischenReward);
                    }
                    break;
                case "BAD":
                    if (badCount != 0)
                    {
                        badCount--;
                        badText.text = "x" + badCount;
                        AddScore(badReward);
                    }
                    break;
                default:
                    break;
            }
        }

        public void DisplayScore()
        {
            scoreText.text = score.ToString();
        }
    }
}
