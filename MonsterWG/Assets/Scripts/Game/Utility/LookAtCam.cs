using System;
using Cinemachine;
using UnityEngine;

namespace Game.Utility
{
    public class LookAtCam : MonoBehaviour
    {
        public GameObject target;

        private void Start()
        {
            target = FindObjectOfType<CinemachineVirtualCamera>().gameObject;
        }

        void LateUpdate()
        {
            transform.LookAt(target.transform);
        }
    }
}
