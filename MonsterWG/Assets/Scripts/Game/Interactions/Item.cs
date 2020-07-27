using UnityEngine;

namespace Game.Interactions
{
    public class Item : MonoBehaviour
    {
        public ItemStates itemState;

        private void Start()
        {
            itemState = ItemStates.Dirty;
        }

        public void SetItemState(ItemStates state)
        {
            itemState = state;
        }
    }
}