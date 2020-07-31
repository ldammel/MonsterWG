using System;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Interactions
{
    public class DishDisplay : MonoBehaviour
    {
        public GameObject[] dishes;
        [Sirenix.OdinInspector.OnValueChanged(nameof(TestDishDisplay))]
        public int displayAmount;
        public Interaction interaction;
        public bool stayVisible;

        private void Start()
        {
            if(!stayVisible)dishes.ForEach(d => d.SetActive(false));
        }

        private void Update()
        {
            if(interaction)interaction.canStart = displayAmount >= 1;
        }

        private void TestDishDisplay()
        {
            dishes.ForEach(d => d.SetActive(false));
            DisplayDish();
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
