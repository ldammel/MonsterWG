using Game.UI;
using UnityEngine;

namespace Game.Quests
{
    [CreateAssetMenu(fileName = "new MainQuest", menuName = "MainQuest")]
    public class MainQuest : ScriptableObject
    {
        public Quest[] subQuests;
        public int questReward;
        public bool isDone;
        public bool hasFailed;

        public void CheckQuestDone()
        {
            if (isDone) return;
            if (hasFailed) return;
            if (subQuests.Length < 1) return;
            foreach (var q in subQuests)
            {
                if (q.hasFailed)
                {
                    hasFailed = true;
                    return;
                }
                if (!q.isDone) return;
            }
            
            isDone = true;
            ScoreDisplay.instance.AddScore(questReward);
        }
    }
}