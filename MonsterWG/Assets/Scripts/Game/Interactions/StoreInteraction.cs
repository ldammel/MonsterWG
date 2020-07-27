using System.Collections;
using System.Collections.Generic;
using Game.AI;
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

        [SerializeField] private bool cheatStorage;
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
        
        #region Explosion
        [FoldoutGroup("Explosion")]
        [SerializeField] private Transform[] ExplosionTransforms;
        [FoldoutGroup("Explosion")]
        [SerializeField] private UnityEvent onExplosion;
        #endregion

        public void AddObject(GameObject o)
        {
            var pick = o.gameObject.GetComponent<Pickup>();
            pick.CancelPickUp();
            pick._isPickedUp = false;
            o.gameObject.GetComponentInChildren<InteractionStateBehaviour>().ResetStates();
            StartCoroutine(WaitTime(o.gameObject));
            if (!cheatStorage)
            {
                storedObjectsAmount++;   
                storedObjects.Add(o.gameObject);
                if(useEvents)onStored[storedObjectsAmount].Invoke();
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
