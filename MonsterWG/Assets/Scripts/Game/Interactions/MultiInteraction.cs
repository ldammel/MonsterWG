using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interactions
{
    public class MultiInteraction : MonoBehaviour
    {
        public enum Order
        {
            Linear,
            Parallel
        }

        public Order InteractionOrder;

        public List<Interaction> interactions = new List<Interaction>();

        private int _currentIndex = 0;


        private void Start()
        {
            _currentIndex = 0;

            if(InteractionOrder == Order.Linear)
            {
                // Reset all coliders
                for (int i = 0; i < interactions.Count; i++)
                {
                    interactions[i].GetComponent<BoxCollider>().enabled = i == _currentIndex;
                }
            }
        }

        public void ActivateNextInteraction()
        {
            if (InteractionOrder == Order.Parallel)
            {
                Debug.LogError("Tried to increase parallel multi interaction!");
                return;
            }

            _currentIndex++;
            if(_currentIndex < interactions.Count)
            {
                // Deactivate current collider and activate next one
                interactions[_currentIndex - 1].GetComponent<BoxCollider>().enabled = false;
                interactions[_currentIndex].GetComponent<BoxCollider>().enabled = true;
            }
        }
    }
}
