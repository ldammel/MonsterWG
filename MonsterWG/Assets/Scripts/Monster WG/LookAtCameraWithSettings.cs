using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraWithSettings : MonoBehaviour
{
    public Transform camTransform;
    public bool usePosition = false;
    public Vector3 rotationOffset;
    void Awake()
    {
        if(!camTransform)
            camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(camTransform)
        {
            if(usePosition)
            {
                transform.LookAt(camTransform.position);
                transform.Rotate(rotationOffset + new Vector3 (0,180f,0));
            }
            else
            {
                transform.rotation = camTransform.rotation;
                transform.Rotate(rotationOffset);
            }
        }
    }
}
