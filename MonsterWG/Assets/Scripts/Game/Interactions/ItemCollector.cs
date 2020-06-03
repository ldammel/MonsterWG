using System.Collections.Generic;
using UnityEngine;

namespace Game.Interactions
{
    public class ItemCollector : MonoBehaviour
    {
        public List<Pickup> collectedItems;
        private PlayerInteractionController[] _players;
        
        private void Start()
        {
            _players = FindObjectsOfType<PlayerInteractionController>();
        }

        public void InsertItem(Pickup o)
        {
            foreach (var p in _players)
            {
                if (p.pickups.Contains(o)) p.pickups.Remove(o);
            }
            collectedItems.Add(o);
        }

        public void DisableItems()
        {
            collectedItems.ForEach(x => x.gameObject.SetActive(false));
        }
    }
}
