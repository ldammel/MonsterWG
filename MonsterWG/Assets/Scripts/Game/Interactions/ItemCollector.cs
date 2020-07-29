using System.Collections.Generic;
using UnityEngine;

namespace Game.Interactions
{
    public class ItemCollector : MonoBehaviour
    {
        public List<Item> collectedItems;
        public Mesh cleanMesh;
        public Material cleanMaterial;
        public DishDisplay dishDisplay;
        
        public void InsertItem(Item o)
        {
            if (o.itemState == ItemStates.Folded)
            {
                if(collectedItems.Contains(o))collectedItems.Remove(o);
                return;
            }

            if (dishDisplay)
            {
                dishDisplay.AddDisplay();
                o.gameObject.SetActive(false);
            }
            collectedItems.Add(o);
        }

        public void DisableItems()
        {
            collectedItems.ForEach(x => x.gameObject.SetActive(false));
        }

        public void EnableItems()
        {
            collectedItems.ForEach(x => x.gameObject.SetActive(true));
            collectedItems.ForEach(x => x.itemState = ItemStates.Folded);
            collectedItems.ForEach(x => x.gameObject.GetComponent<MeshFilter>().mesh = cleanMesh);
            collectedItems.ForEach(x => x.gameObject.GetComponent<MeshRenderer>().material = cleanMaterial);
        }
    }
}
