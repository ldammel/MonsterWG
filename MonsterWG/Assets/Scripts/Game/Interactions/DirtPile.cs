using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
