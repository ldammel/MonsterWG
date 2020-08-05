using System.Collections;
using System.Collections.Generic;
using Game.AI;
using Game.Utility;
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

        public bool cheatStorage;

        public bool isFull => storedObjectsAmount >= limit;
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
        [SerializeField] private bool useTiming;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent timingEvent;
        [FoldoutGroup("Events")]
        [Header("Ab wann beginnt das mashing minigame")]
        [SerializeField] private int mashinglimit = 5;
        [FoldoutGroup("Events")]
        [SerializeField] private bool useMashing;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent mashingEvent;
        #endregion
        
        #region Explosion
        [FoldoutGroup("Explosion")]
        [SerializeField] private Transform[] explosionTransforms;
        [FoldoutGroup("Explosion")]
        [SerializeField] private UnityEvent onExplosion;
        #endregion

        public void AddObject(GameObject o)
        {
            if (isFull) return;

            var pick = o.gameObject.GetComponent<Pickup>();
            if (pick.canNotBeStored) return;
            if (storedObjects.Contains(o)) return;
            pick.ForceCancelPickUp();
            pick._isPickedUp = false;
            o.gameObject.GetComponentInChildren<InteractionStateBehaviour>().ResetStates();
            StartCoroutine(WaitTime(o.gameObject));
            if (!cheatStorage)
            {
                SoundManager.Instance.Play(gameObject, SoundManager.Sounds.MüllWegwerfen);
                storedObjectsAmount++;   
                storedObjects.Add(o.gameObject);
                if (useEvents)
                {
                    if (storedObjectsAmount < timinglimit) return;
                    else if (storedObjectsAmount >= timinglimit && storedObjectsAmount < mashinglimit) timingEvent.Invoke();
                    else if (storedObjectsAmount >= mashinglimit) mashingEvent.Invoke();
                }
            }
            else
            {
                SoundManager.Instance.Play(gameObject, SoundManager.Sounds.SchrankStopfen);
                storedObjectsAmount++; 
                storedObjects.Add(o.gameObject);

                if (storedObjectsAmount >= limit)
                {
                    RemoveObjects();
                    return;
                }

                if(useEvents)onStored[storedObjectsAmount].Invoke();
                if(useMashing && storedObjectsAmount >= mashinglimit) mashingEvent.Invoke();
            }

        }

        private void RemoveObjects()
        {
            if(explosionTransforms.IsNullOrEmpty()) return;
            SoundManager.Instance.Play(gameObject, SoundManager.Sounds.SchrankKnallen);
            
            for (int i = 0; i < storedObjects.Count; i++)
            {
                storedObjects[i].GetComponent<Pickup>().ForceCancelPickUp();
                storedObjects[i].SetActive(true);
                storedObjects[i].transform.position = explosionTransforms[i].position;
            }
            //storedObjectsAmount = 0;
            storedObjects.Clear();
            onExplosion.Invoke();
            enabled = false;
            GetComponent<Collider>().enabled = false;
            //gameObject.SetActive(false);
        }

        IEnumerator WaitTime(GameObject o)
        {
            yield return new WaitForSeconds(0.1f);

            if (storedObjects.Contains(o))
            {
                o.SetActive(false);
            }
        }
    }
}
