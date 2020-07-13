using System;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Interactions
{
    public class DishDisplay : MonoBehaviour
    {
        public GameObject[] dishes;
        public int displayAmount;
        public Interaction interaction;

        private void Start()
        {
            dishes.ForEach(d => d.SetActive(false));
        }

        private void Update()
        {
            if(interaction)interaction.CanStart = displayAmount >= 1;
        }

        public void AddDisplay()
        {
            displayAmount++;
            DisplayDish();
        }

        public void DisplayDish()
        {
            for (int i = 0; i < displayAmount; i++)
            {
                dishes[i].SetActive(true);
            }
        }
        public void RemoveDishFromDisplay()
        {
            for (int i = dishes.Length-1; i >= 0; i--)
            {
                if (!dishes[i].activeSelf) continue;
                dishes[i].SetActive(false);
                displayAmount--;
                return;
            }
        }
    }
}
