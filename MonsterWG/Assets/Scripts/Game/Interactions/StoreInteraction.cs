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
        [Header("0 = Timing MiniGame - 1 = Button Mash")]
        [SerializeField] private bool useEvents;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent[] onStored;
        [FoldoutGroup("Events")]
        [SerializeField] private int storedObjectsAmount = 0;
        [FoldoutGroup("Events")]
        [Header("Ab wann beginnt das timing minigame")]
        [SerializeField] private int timinglimit = 3;
        [FoldoutGroup("Events")]
        [Header("Ab wann beginnt das mashing minigame")]
        [SerializeField] private int mashinglimit = 5;
        #endregion
        
        #region Explosion
        [FoldoutGroup("Explosion")]
        [SerializeField] private Transform[] explosionTransforms;
        [FoldoutGroup("Explosion")]
        [SerializeField] private UnityEvent onExplosion;
        #endregion

        public void AddObject(GameObject o)
        {
            var pick = o.gameObject.GetComponent<Pickup>();
            if (!pick.canBeStored) return;
            pick.CancelPickUp();
            pick._isPickedUp = false;
            o.gameObject.GetComponentInChildren<InteractionStateBehaviour>().ResetStates();
            StartCoroutine(WaitTime(o.gameObject));
            if (!cheatStorage)
            {
                storedObjectsAmount++;   
                storedObjects.Add(o.gameObject);
                if (useEvents)
                {
                    if (storedObjectsAmount < timinglimit) return;
                    else if (storedObjectsAmount >= timinglimit && storedObjectsAmount < mashinglimit) onStored[0].Invoke();
                    else if (storedObjectsAmount >= mashinglimit) onStored[1].Invoke();
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
            if(explosionTransforms.IsNullOrEmpty()) return;
            
            for (int i = 0; i < storedObjects.Count; i++)
            {
                storedObjects[i].GetComponent<Pickup>().CancelPickUp();
                storedObjects[i].SetActive(true);
                storedObjects[i].transform.position = explosionTransforms[i].position;
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
