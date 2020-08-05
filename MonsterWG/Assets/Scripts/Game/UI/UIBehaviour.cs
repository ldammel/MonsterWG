using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class UIBehaviour : MonoBehaviour , IPointerEnterHandler , ISelectHandler 
    {
        public UnityEvent onActivate;
        public void Execute()
        {
            onActivate.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Execute();
        }

        public void OnSelect(BaseEventData eventData)
        {
            Execute();
        }
    }
}
