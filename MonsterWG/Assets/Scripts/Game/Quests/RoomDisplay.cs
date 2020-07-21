using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Quests
{
    public class RoomDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private Image fillImage;
        [SerializeField] private Image greyImage;
        public int ActiveQuestAmount { get; set; }
        public int DoneQuestAmount { get; set; }
        public List<Quest> roomQuests;
        public bool startRoom;
        public bool IsInitialized;

        private void Start()
        {
            if (!startRoom)return;
            InitializeRoom();
        }

        public void InitializeRoom()
        {
            ActiveQuestAmount = roomQuests.Count;
            IsInitialized = true;
            greyImage.gameObject.SetActive(false);
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
