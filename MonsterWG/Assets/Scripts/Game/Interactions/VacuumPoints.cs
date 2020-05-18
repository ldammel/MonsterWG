using System;
using UnityEngine;

namespace Game.Interactions
{
    public class VacuumPoints : MonoBehaviour
    {
        [SerializeField] private GameObject nextPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Untagged")) return;
            nextPoint.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
