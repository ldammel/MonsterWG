using System;
using System.Collections.Generic;
using System.Linq;
using Game.Quests;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactions
{
    public class StoreInteraction : MonoBehaviour
    {
        #region Variables
        [FoldoutGroup("Limit")]
        [Header("How many objects can we store:")]
        [SerializeField] private int limit;

        [FoldoutGroup("Events")]
        [Header("OnStored Events --> Size == Limit! - used for model swap")]
        [SerializeField] private bool useEvents;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent[] onStored;
        [FoldoutGroup("Events")]
        [SerializeField] private int storedObjectsAmount = 0;
        
        [FoldoutGroup("Quests")]
        [Header("Is the Storage part of a Quest (e.g. Clean Dishes)")]
        public bool isQuestStorage;
        [FoldoutGroup("Quests")]
        [SerializeField] private QuestType questType;
        
        [FoldoutGroup("Explosion")]
        [Header("Explosion Events")]
        [SerializeField] private DirtPile dirtPile;
        [FoldoutGroup("Explosion")]
        [SerializeField] private UnityEvent onExplosion;
        #endregion
        
        public void AddObject(QuestAssign o)
        {
            if (isQuestStorage)
            {
                storedObjectsAmount++;   
                if(o.quest.questType == questType)o.quest.CheckDone();
                else
                {
                    o.quest.hasCheated = true;
                    o.quest.CheckDone();
                }
            }
            else
            {
                if(useEvents)onStored[storedObjectsAmount].Invoke();
                storedObjectsAmount++; 
                if (storedObjectsAmount >= limit) RemoveObjects();
            }
            Destroy(o.gameObject);
        }

        private void RemoveObjects()
        {
            storedObjectsAmount = 0;
            onExplosion.Invoke();
            if(dirtPile)dirtPile.PileUp(limit);
            enabled = false;
        }
    }
}
