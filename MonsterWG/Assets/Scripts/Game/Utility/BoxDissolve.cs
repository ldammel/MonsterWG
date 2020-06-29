using System.Collections;
using Game.Character;
using Game.Quests;
using UnityEngine;

namespace Game.Utility
{
    public class BoxDissolve : MonoBehaviour
    {
        [SerializeField] private MeshRenderer target;
        [SerializeField] private float increaseDuration;
        [SerializeField] private float reduceDuration;
        [SerializeField] private float fogDelay;

        public RoomDisplay roomDisplay;
        public bool RoomCleared { get; set; }

        private float _dur;
        private static readonly int Density = Shader.PropertyToID("_fogDensity");
        private float _op;
        private bool _increase;
        private bool _reduce;
        private int _inArea;

        private void Start()
        {
            _dur = reduceDuration;
        }

        private void Update()
        {
            if (_reduce) ReduceFog();;
            if(_increase) IncreaseFog();
        }

        private void ReduceFog()
        {
            _dur -= Time.deltaTime;
            _op = _dur / reduceDuration;
            target.material.SetFloat(Density, _op);
            if (_dur <= 0)
            {
                _reduce = false;
                if (RoomCleared) return;
                RoomCleared = true;
                if (!roomDisplay) return;
                if(!roomDisplay.IsInitialized)roomDisplay.InitializeRoom();
            }
        }
        
        private void IncreaseFog()
        {
            _dur += Time.deltaTime;
            _op = _dur / increaseDuration;
            target.material.SetFloat(Density, _op);
            if(_dur >= increaseDuration) _increase = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Player2") || other.CompareTag("Mom"))
            {
                _inArea++;
                if (_dur > reduceDuration) _dur = reduceDuration;
                _reduce = true;
                _increase = false;
            }

            if (other.CompareTag("Mom")) other.GetComponent<FieldOfView>().room = this;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Player2") || other.CompareTag("Mom"))
            {
                _inArea--;
                if (_inArea > 0) return;
                StartCoroutine(WaitTime(fogDelay));
            }
        }

        private IEnumerator WaitTime(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (_inArea <= 0)
            {
                _increase = true;
                _reduce = false;
            }
        }
    }
}
