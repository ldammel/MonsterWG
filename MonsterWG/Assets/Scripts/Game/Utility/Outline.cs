using Game.UI;
using UnityEngine;

namespace Game.Utility
{
    public class Outline : MonoBehaviour
    {
        [SerializeField] private Material outlineMaterial;
        public BoxDissolve roomTarget;
        private Renderer _outlineRenderer;
        [SerializeField] private UIBehaviour behaviour;
        private static readonly int FirstOutlineColor = Shader.PropertyToID("_FirstOutlineColor");

        private void Start()
        {
            _outlineRenderer = CreateOutline(outlineMaterial);
        }

        private Renderer CreateOutline(Material outlineMat){

            GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation ,transform);
            outlineObject.transform.localScale = Vector3.one;
            Renderer rend = outlineObject.GetComponent<Renderer>();
            rend.material = outlineMat;
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            outlineObject.GetComponent<Outline>().enabled = false;
            outlineObject.GetComponent<Collider>().enabled = false;
            rend.enabled = false;

            return rend;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (roomTarget)
            {
                if (!roomTarget.RoomCleared) return;
            }
            if (other.CompareTag("Untagged")) return;
            outlineMaterial.SetColor(FirstOutlineColor, other.CompareTag("Player") ? Color.blue : Color.green);
            _outlineRenderer.enabled = true;
        }

        public void Select(Color color)
        {
            outlineMaterial.SetColor(FirstOutlineColor, color );
            _outlineRenderer.enabled = true;
        }
    
        public void Deselect()
        {
            _outlineRenderer.enabled = false;
        }

        public void Activate()
        {
            if (!_outlineRenderer.enabled) return;
            behaviour.Execute();
        }

        private void  OnTriggerExit(Collider other)
        {
            if (roomTarget)
            {
                if (!roomTarget.RoomCleared) return;
            }
            if (other.CompareTag("Untagged")) return;
            _outlineRenderer.enabled = false;
        }
    }
}

