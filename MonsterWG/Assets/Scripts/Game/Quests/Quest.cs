using System.Collections.Generic;
using Game.UI;
using UnityEngine;

namespace Game.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quests")]
    public class Quest : ScriptableObject
    {
        public List<TaskDisplay> taskDisplay;
        public QuestType questType;
        public int doneAmount;
        public int questReward;
        public float miniQuestReward;
        public Sprite taskImage;
        public Sprite taskFailedImage;
        public Sprite taskSuccessImage;
        public bool isDone;
        public bool hasFailed;
        public bool hasCheated;
        public bool isRewarded;

        public int hasAmount;
        
        public void CheckDone()
        {
            if (isDone || isRewarded) return;
            hasAmount++;
            ScoreDisplay.instance.AddScore(questReward);
            if (hasAmount < doneAmount) return;
            isDone = true;
            QuestDisplay.instance.FinishQuest(this);
        }
    }

    public enum QuestType
    {
        CollectTrash,
        CleanDishes,
        CleanLaundry,
        CleanEnvironment
    }
}