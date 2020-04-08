using UnityEngine;

namespace Game.Interactions
{
    public class BaseInteraction : MonoBehaviour
    {
        public GameObject interactionTarget;

        public virtual void Interact(){}
        
    }
}
