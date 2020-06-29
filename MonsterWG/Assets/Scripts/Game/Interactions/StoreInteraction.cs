using System;
using System.Collections.Generic;
using System.Linq;
using Game.Quests;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactions
{
    public class StoreInteraction : MonoBehaviour
    {
        [Header("How many objects can we store:")]
        [SerializeField] private int limit;
        [Header("OnStored Events --> Size == Limit! - used for model swap")]
        public UnityEvent[] onStored;
        public int storedObjectsAmount = 0;
        [Header("Is the Storage part of a Quest (e.g. Clean Dishes)")]
        public bool isQuestStorage;
        public Quest storageQuest;
        [Header("Explosion Events")]
        public DirtPile dirtPile;
        public UnityEvent onExplosion;

        private QuestDisplay _display;
        private PlayerInteractionController[] _players;

        private void Start()
        {
            _display = FindObjectOfType<QuestDisplay>();
            _players = FindObjectsOfType<PlayerInteractionController>();
        }

        public void AddObject(Pickup o)
        {
            foreach (var p in _players)
            {
                if (p.pickups.Contains(o)) p.pickups.Remove(o);
            }
            Destroy(o.gameObject);
            if (isQuestStorage)
            {
                storedObjectsAmount++;   
                if (storedObjectsAmount >= limit) _display.FinishQuest(storageQuest);
            }
            else
            {
                onStored[storedObjectsAmount].Invoke();
                storedObjectsAmount++; 
                if (storedObjectsAmount >= limit) RemoveObjects();
            }
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
