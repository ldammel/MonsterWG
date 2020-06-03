using Cinemachine;
using Game.UI;
using Game.Utility;
using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    public BoxDissolve roomTarget;
    private Renderer outlineRenderer;
    [SerializeField] private UIBehaviour behaviour;
    private static readonly int FirstOutlineColor = Shader.PropertyToID("_FirstOutlineColor");

    void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial);
    }

    Renderer CreateOutline(Material outlineMat){

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
        outlineRenderer.enabled = true;
    }

    public void Select(Color color)
    {
        outlineMaterial.SetColor(FirstOutlineColor, color );
        outlineRenderer.enabled = true;
    }
    
    public void Deselect()
    {
        outlineRenderer.enabled = false;
    }

    public void Activate()
    {
        if (!outlineRenderer.enabled) return;
        behaviour.Execute();
    }

    private void  OnTriggerExit(Collider other)
    {
        if (roomTarget)
        {
            if (!roomTarget.RoomCleared) return;
        }
        if (other.CompareTag("Untagged")) return;
        outlineRenderer.enabled = false;
    }
}

