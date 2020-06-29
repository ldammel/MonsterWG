using UnityEngine;

namespace Game.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quests")]
    public class Quest : ScriptableObject
    {
        public TaskDisplay taskDisplay;
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

        private int _hasAmount;
        
        public void CheckDone()
        {
            if (_hasAmount != doneAmount)
            {
                _hasAmount++;
            }
            else
            {
                isDone = true;
                QuestDisplay.instance.FinishQuest(this);
            }
        }
    }
}