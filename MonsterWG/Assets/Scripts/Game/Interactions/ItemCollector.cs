using System.Collections.Generic;
using UnityEngine;

namespace Game.Interactions
{
    public class ItemCollector : MonoBehaviour
    {
        public List<Pickup> collectedItems;

        public void DisableItems()
        {
            collectedItems.ForEach(x => x.gameObject.SetActive(false));
        }
    }
}
