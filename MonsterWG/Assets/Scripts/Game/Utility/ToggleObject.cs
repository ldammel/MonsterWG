using UnityEngine;

namespace Game.Utility
{
    public class ToggleObject : MonoBehaviour
    {
        [SerializeField] private GameObject[] targets;

        public void ToggleObjects()
        {
            foreach (var target in targets)
            {
                target.SetActive(!target.activeSelf);
            }
        }
    }
}
