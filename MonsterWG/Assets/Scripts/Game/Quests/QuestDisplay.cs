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
        [SerializeField] private GameObject[] questDisplays;
        [SerializeField] private List<Quest> activeQuests;

        private void Start()
        {
            ListQuests();
        }

        public void AddActiveQuest(Quest quest)
        {
            if (activeQuests.Contains(quest)) return;
            activeQuests.Add(quest);
            ListQuests();
        }

        public void FinishQuest(Quest quest)
        {
            if (!activeQuests.Contains(quest)) return;
            activeQuests.Remove(quest);
            activeQuests.Add(null);
            ListQuests();
            ScoreDisplay.instance.AddScore(quest.questReward);
        }

        public void ListQuests()
        {
            for (int i = 0; i < questDisplays.Length; i++)
            {
                var texts = questDisplays[i].GetComponentsInChildren<TextMeshProUGUI>();
                if (activeQuests[i] == null)
                {
                    texts[1].text = "";
                    texts[0].text = "";
                }
                else
                {
                    texts[1].text = activeQuests[i].questTitle;
                    texts[0].text = activeQuests[i].questDescription;
                }
            }
        }
    }
}