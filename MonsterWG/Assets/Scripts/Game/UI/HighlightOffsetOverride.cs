using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class HighlightOffsetOverride : MonoBehaviour
    {
        [BoxGroup("Transform")]
        [SerializeField]
        public Vector3 positionOffset = new Vector3(0, 0.1f, 0);

        [BoxGroup("Transform")]
        [SerializeField]
        public Quaternion rotation = Quaternion.Euler(60, 220, 0);
    
        [BoxGroup("Transform")]
        [SerializeField]
        public Vector3 scale = new Vector3(0.005f, 0.005f, 0.005f);
    }
}
