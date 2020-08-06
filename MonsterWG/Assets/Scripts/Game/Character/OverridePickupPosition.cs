using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverridePickupPosition : MonoBehaviour
{
    [BoxGroup("Transform")]
    [SerializeField]
    public Vector3 positionOffsetJimmy;

    [BoxGroup("Transform")]
    [SerializeField]
    public Quaternion rotationJimmy;

    [BoxGroup("Transform")]
    [SerializeField]
    public Vector3 scaleJimmy;


    [BoxGroup("Transform")]
    [SerializeField]
    public Vector3 positionOffsetRoy;

    [BoxGroup("Transform")]
    [SerializeField]
    public Quaternion rotationRoy;

    [BoxGroup("Transform")]
    [SerializeField]
    public Vector3 scaleRoy;


    public Vector3 GetPosition(bool playerOne)
    {
        return playerOne ? positionOffsetJimmy : positionOffsetRoy;
    }

    public Quaternion GetRotation(bool playerOne)
    {
        return playerOne ? rotationJimmy : rotationRoy;
    }

    public Vector3 GetScale(bool playerOne)
    {
        return playerOne ? scaleJimmy : scaleRoy;
    }
}
