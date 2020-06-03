using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Game.Quests
{
    public class RoomDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private Image fillImage;
        public int ActiveQuestAmount { get; set; }
        public int DoneQuestAmount { get; set; }
        public bool startRoom;
        private TaskDisplay[] _taskDisplays;
        public bool IsInitialized { get; set; }

        private void Start()
        {
            if (!startRoom)return;
            InitializeRoom();
        }

        public void InitializeRoom()
        {
            _taskDisplays = gameObject.GetComponentsInChildren<TaskDisplay>();
            foreach (var t in _taskDisplays)
            {
                ActiveQuestAmount += t.ActiveQuestAmount;
            }

            IsInitialized = true;
        }

        private void LateUpdate()
        {
            if (!IsInitialized)
            {
                displayText.text = "? / ?";
                return;
            }
            displayText.text = DoneQuestAmount + " / " + ActiveQuestAmount;
            if(ActiveQuestAmount != 0)fillImage.fillAmount = (float)DoneQuestAmount / (float)ActiveQuestAmount;
        }
    }
}
