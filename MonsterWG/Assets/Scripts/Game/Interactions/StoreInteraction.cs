using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactions
{
    public class StoreInteraction : MonoBehaviour
    {
        [Header("How many objects can we store:")]
        [SerializeField] private int limit;
        public List<GameObject> storedObjects;
        [Space]
        [Header("OnStored Events --> Size == Limit!")]
        public UnityEvent[] onStored;
        [Space]
        [Header("Explosion Events")]
        public UnityEvent onExplosion;

        public void AddObject(GameObject o)
        {
            if (storedObjects.Contains(o)) return;
            storedObjects.Add(o);
            o.SetActive(false);
            onStored[storedObjects.Count-1].Invoke();
            if(storedObjects.Count >= limit) RemoveObjects();
        }

        private void RemoveObjects()
        {
            storedObjects.Clear();
            onExplosion.Invoke();
            enabled = false;
        }
    }
}
