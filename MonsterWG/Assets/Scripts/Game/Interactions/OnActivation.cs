using Game.Character;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactions
{
    public class OnActivation : MonoBehaviour
    {
        public UnityEvent onActivation;
        public void PickUp()
        {
            onActivation.Invoke();
        }
    }
}
