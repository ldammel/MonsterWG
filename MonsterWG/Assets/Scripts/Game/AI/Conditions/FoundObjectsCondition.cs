using System.Linq;
using Game.Quests;
using Game.Utility;
using UnityEngine;

namespace Game.AI
{
    public class FoundObjectsCondition : StateTransitionCondition
    {
        [SerializeField] private FieldOfView fov;
        private bool _evaluated;
        public override bool IsMet()
        {
            return _evaluated;
        }

        public void Reset()
        {
            _evaluated = false;
        }

        public void EvaluateObjects()
        {
            _evaluated = true;
            if (fov.visibleTargets.Count <= 0) return;
            foreach (var obj in fov.visibleTargets)
            {
                var quest = obj.GetComponent<QuestAssign>().quest;
                if (quest.isDone) return;
                if (!obj.activeSelf) return;
                quest.hasFailed = true;
                quest.isRewarded = true;
                obj.gameObject.SetActive(false);
            }

            if (!fov.room.roomDisplay) return;
            if (fov.room.roomDisplay.roomQuests.Count <= 0) return;
            var roomQuests = fov.room.roomDisplay.roomQuests;
            foreach (var quest in roomQuests.Where(quest => !quest.isDone))
            {
                quest.hasFailed = true;
                quest.isRewarded = true;
            }
        }
    }
}