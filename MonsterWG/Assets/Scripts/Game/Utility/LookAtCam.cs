using UnityEngine;

namespace Game.Utility
{
    public class LookAtCam : MonoBehaviour
    {
        public GameObject target;

        void Update()
        {
            transform.LookAt(target.transform);
        }
    }
}
