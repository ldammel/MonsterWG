using System.Linq;
using Game.Quests;
using Game.Utility;
using UnityEngine;

namespace Game.AI
{
    public class FoundObjectsCondition : StateTransitionCondition
    {
        [SerializeField] private FieldOfView fov;
        [SerializeField] private FinalScoring score;
        private bool _evaluated;
        
        public override bool IsMet()
        {
            if (score.currentQuest)
            {
                if (score.currentQuest.isRewarded)
                {
                    return true;
                }
                else return false;
            }
            else
            {
                return _evaluated;
            }

        }

        public void Reset()
        {
            _evaluated = false;
        }

        public void EvaluateObjects()
        {
            if (fov.visibleTargets.Count > 0)
            {
                foreach (var obj in fov.visibleTargets)
                {
                    if (obj.GetComponent<QuestAssign>())
                    {
                        var quest = obj.GetComponent<QuestAssign>().quest;
                        if (quest.isDone) return;
                        if (!obj.activeSelf) return;
                        if (quest.hasCheated) score.timeRateOfDecay += 0.5f;
                        quest.hasFailed = true;
                        quest.isRewarded = true;
                        obj.gameObject.SetActive(false);
                    }
                }
                fov.visibleTargets.Clear();
            }

            if (fov.room.roomDisplay)
            {
                if (fov.room.roomDisplay.roomQuests.Count > 0)
                {
                    var roomQuests = fov.room.roomDisplay.roomQuests;
                    foreach (var quest in roomQuests.Where(quest => !quest.isDone))
                    {
                        quest.hasFailed = true;
                        quest.isRewarded = true;
                    }
                }
            }

            _evaluated = true;
        }
    }
}