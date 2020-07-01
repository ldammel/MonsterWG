using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Quests
{
    public class TaskDisplay : MonoBehaviour
    {
        [SerializeField] private RoomDisplay roomDisplay;
        public Quest[] quests;
        public int ActiveQuestAmount { get; set; }
        public int DoneQuestAmount { get; set; }
        private TextMeshProUGUI _displayText;

        private void Awake()
        {
            if (quests == null) return;
            _displayText = gameObject.GetComponent<TextMeshProUGUI>();
            ActiveQuestAmount = quests.Length;
            foreach (var q in quests)
            {
                q.taskDisplay.Add(this);
            }
        }

        private void LateUpdate()
        {
            _displayText.text = DoneQuestAmount + " / " + ActiveQuestAmount;
            if (DoneQuestAmount >= ActiveQuestAmount)
            {
                _displayText.color = Color.green;
            }
        }

        public void AddRoomDisplayDone()
        {
            roomDisplay.DoneQuestAmount++;
        }
    }
}
