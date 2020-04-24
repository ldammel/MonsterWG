using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{

    public class UIBehaviour : MonoBehaviour
    {
        public UnityEvent onActivate;
        public void Execute()
        {
            onActivate.Invoke();
        }
    }
}
