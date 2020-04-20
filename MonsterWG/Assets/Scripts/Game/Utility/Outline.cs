using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    private Renderer outlineRenderer;

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

    private void OnMouseOver()
    {
        transform.Rotate(Vector3.up, 1f, Space.World);
    }

    private void  OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Untagged")) return;
        outlineRenderer.enabled = false;
    }
}

