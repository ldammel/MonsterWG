using Cinemachine;
using Game.UI;
using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    private Renderer outlineRenderer;
    [SerializeField] private UIBehaviour behaviour;

    void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial);
    }

    Renderer CreateOutline(Material outlineMat){

        GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation ,transform);
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
        if (other.CompareTag("Untagged")) return;
        outlineRenderer.enabled = true;
    }

    private void OnMouseEnter()
    {
        outlineRenderer.enabled = true;
    }

    private void OnMouseDown()
    {
        if (!outlineRenderer.enabled) return;
        behaviour.Execute();
    }

    private void OnMouseExit()
    {
        outlineRenderer.enabled = false;
    }

    private void  OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Untagged")) return;
        outlineRenderer.enabled = false;
    }
}

