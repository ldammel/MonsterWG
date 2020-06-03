using UnityEngine;

namespace Game.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quests")]
    public class Quest : ScriptableObject
    {
        public string questTitle;
        [TextArea]public string questDescription;
        public TaskDisplay taskDisplay;
        public int questReward;
    }
}