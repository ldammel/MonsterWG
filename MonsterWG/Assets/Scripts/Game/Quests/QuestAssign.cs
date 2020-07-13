using UnityEngine;

namespace Game.Quests
{
    public class QuestAssign : MonoBehaviour
    {
        public Quest quest;

        public void SwitchQuest(Quest _quest)
        {
            quest = _quest;
        }
    }
}
