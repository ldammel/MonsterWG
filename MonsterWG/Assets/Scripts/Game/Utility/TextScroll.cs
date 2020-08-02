using System;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace Game.Utility
{
    public class TextScroll : MonoBehaviour
    {
        [SerializeField] private GameObject textObject;
        [SerializeField] private float scrollSpeed;

        private bool _start;

        private void Update()
        {
            if (!_start) return;
            textObject.transform.localPosition = new Vector3(0,textObject.transform.localPosition.y  + scrollSpeed*Time.deltaTime,0);
        }

        private void OnEnable()
        {
            _start = true;
            textObject.transform.localPosition = Vector3.zero;
        }

        private void OnDisable()
        {
            _start = false;
            textObject.transform.localPosition = Vector3.zero;
        }
    }
}
