using System;
using System.Collections.Generic;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Quests
{
    public class QuestDisplay : MonoBehaviour
    {
        [SerializeField] private List<Quest> activeQuests;
        
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
            quest.taskDisplay.DoneQuestAmount++;
            quest.taskDisplay.AddRoomDisplayDone();
            activeQuests.Remove(quest);
            activeQuests.Add(null);
            ScoreDisplay.instance.AddScore(quest.questReward);
        }
    }
}