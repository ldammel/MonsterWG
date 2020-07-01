using System;
using System.Collections.Generic;
using System.Linq;
using Game.UI;
using UnityEngine;

namespace Game.Quests
{
    public class QuestDisplay : MonoBehaviour
    {
        public static QuestDisplay instance;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("There can only be one instance of QuestDisplay in the Scene!");
                Application.Quit();
            }
            instance = this;
        }


        public List<Quest> activeQuests;
        public List<MainQuest> mainQuests;
        
        private void Start()
        {
            foreach (var q in activeQuests.Where(q => q != null))
            {
                q.hasAmount = 0;
                q.isDone = false;
                q.hasFailed = false;
                q.hasCheated = false;
                q.isRewarded = false;
                q.taskDisplay.Clear();
            }
            foreach (var q in mainQuests.Where(q => q != null))
            {
                q.isDone = false;
                q.hasFailed = false;
            }
        }
        
        public void AddActiveQuest(Quest quest)
        {
            if (activeQuests.Contains(quest)) return;
            for (int i = 0; i < activeQuests.Count; i++)
            {
                if (activeQuests[i] != null) continue;
                activeQuests[i] = quest;
                return;
            }
            activeQuests.Add(quest);
        }

        public void FinishQuest(Quest quest)
        {
            if (!activeQuests.Contains(quest)) return;
            if (!quest.isDone) return;
            foreach (var t in quest.taskDisplay)
            {
                t.DoneQuestAmount++;
                t.AddRoomDisplayDone();
            }
            activeQuests.Remove(quest);
            activeQuests.Add(null);
            ScoreDisplay.instance.AddScore(quest.questReward);
        }
    }
}