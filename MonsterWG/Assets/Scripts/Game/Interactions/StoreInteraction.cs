using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.Quests;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactions
{
    public class StoreInteraction : MonoBehaviour
    {
        #region Variables
        [FoldoutGroup("Base")]
        [Header("How many objects can we store:")]
        [SerializeField] private int limit;
        [FoldoutGroup("Base")]
        [SerializeField] private List<GameObject> storedObjects;
        #endregion

        #region Events
        [FoldoutGroup("Events")]
        [Header("OnStored Events --> Size == Limit! - used for model swap")]
        [SerializeField] private bool useEvents;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent[] onStored;
        [FoldoutGroup("Events")]
        [SerializeField] private int storedObjectsAmount = 0;
        #endregion
        
        #region Quest
        [FoldoutGroup("Quests")]
        [Header("Is the Storage part of a Quest (e.g. Clean Dishes)")]
        public bool isQuestStorage;
        [FoldoutGroup("Quests")]
        [SerializeField] private QuestType questType;
        #endregion
        
        #region Explosion
        [FoldoutGroup("Explosion")]
        [SerializeField] private Transform[] ExplosionTransforms;
        [FoldoutGroup("Explosion")]
        [SerializeField] private UnityEvent onExplosion;
        #endregion

        public void AddObject(QuestAssign o)
        {
            var pick = o.gameObject.GetComponent<Pickup>();
            pick.CancelPickUp();
            pick._isPickedUp = false;
            o.gameObject.GetComponentInChildren<InteractionStateBehaviour>().ResetStates();
            StartCoroutine(WaitTime(o.gameObject));
            if (isQuestStorage)
            {
                storedObjectsAmount++;   
                storedObjects.Add(o.gameObject);
                if (o.quest.questType == questType)
                {
                    o.quest.CheckDone();
                    if(useEvents)onStored[storedObjectsAmount].Invoke();
                }
                else
                {
                    o.quest.hasCheated = true;
                    o.quest.CheckDone();
                }
            }
            else
            {
                if(useEvents)onStored[storedObjectsAmount].Invoke();
                storedObjects.Add(o.gameObject);
                storedObjectsAmount++; 
                
                if (storedObjectsAmount >= limit) RemoveObjects();
            }

        }

        private void RemoveObjects()
        {
            if(ExplosionTransforms.IsNullOrEmpty()) return;
            
            for (int i = 0; i < storedObjects.Count; i++)
            {
                storedObjects[i].GetComponent<Pickup>().CancelPickUp();
                storedObjects[i].SetActive(true);
                storedObjects[i].transform.position = ExplosionTransforms[i].position;
            }
            storedObjectsAmount = 0;
            storedObjects.Clear();
            onExplosion.Invoke();
            enabled = false;
            gameObject.SetActive(false);
        }

        IEnumerator WaitTime(GameObject o)
        {
            yield return new WaitForSeconds(0.1f);
            o.SetActive(false);
        }
    }
}
