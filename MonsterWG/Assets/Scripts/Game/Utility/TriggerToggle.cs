using System;
using UnityEngine;

namespace Game.Utility
{
    public class TriggerToggle : MonoBehaviour
    {
        [SerializeField] private GameObject[] toggleObjects;
        [SerializeField] private bool[] toggles;
        [SerializeField] private bool enter;
        [SerializeField] private bool exit;

        private void Toggle()
        {
            for (int i = 0; i < toggleObjects.Length; i++)
            {
                toggleObjects[i].SetActive(toggles[i]);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Untagged")) return;
            if (!enter) return;
            Toggle();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!exit) return;
            Toggle();
        }
    }
}
