using UnityEngine;

namespace Game.Interactions
{
    public class DirtPile : MonoBehaviour
    {

        [SerializeField] private GameObject[] pileObjects;

        public void PileUp(int maxObjects)
        {
            for (int i = 0; i < maxObjects; i++)
            {
                pileObjects[i].SetActive(true);
            }
        }
    }
}
