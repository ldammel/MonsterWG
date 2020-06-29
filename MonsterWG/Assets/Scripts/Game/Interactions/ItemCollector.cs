using System.Collections.Generic;
using UnityEngine;

namespace Game.Interactions
{
    public class ItemCollector : MonoBehaviour
    {
        public List<Item> collectedItems;
        
        public void InsertItem(Item o)
        {
            collectedItems.Add(o);
        }

        public void DisableItems()
        {
            collectedItems.ForEach(x => x.gameObject.SetActive(false));
        }
    }
}
