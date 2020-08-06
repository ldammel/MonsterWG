using UnityEngine;

namespace Game.Utility
{
    public class TextScroll : MonoBehaviour
    {
        [SerializeField] private GameObject textObject;
        [SerializeField] private float scrollSpeed;

        private bool _start;
        private float _time;

        private void Update()
        {
            if (!_start) return;
            _time += Time.deltaTime;
            if(_time <= 50)textObject.transform.localPosition = new Vector3(0,textObject.transform.localPosition.y  + scrollSpeed*Time.deltaTime,0);
        }

        private void OnEnable()
        {
            _start = true;
            _time = 0;
            textObject.transform.localPosition = Vector3.zero;
        }

        private void OnDisable()
        {
            _start = false;
            _time = 0;
            textObject.transform.localPosition = Vector3.zero;
        }
    }
}
