using System;
using UnityEngine;

namespace Game.Utility
{
    public class WallHider : MonoBehaviour
    {
        [SerializeField] private Transform rayTarget;
        [SerializeField] private LayerMask layerMask;
        private Transform _hitTarget;

        private void Start()
        {
            _hitTarget = gameObject.transform;
        }

        private void Update()
        {
            RaycastHit hitWall;
 
            Vector3 toCamera;
            toCamera = rayTarget.position - transform.position;

            Ray checkRay = new Ray(transform.position, toCamera);
            if (Physics.Raycast(checkRay, out hitWall, 100, layerMask))
            {
                if (_hitTarget != hitWall.collider.gameObject.transform)
                {
                    if(_hitTarget.gameObject.GetComponent<MeshRenderer>())_hitTarget.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    _hitTarget = hitWall.collider.gameObject.transform;
                }
                if(_hitTarget.gameObject.GetComponent<MeshRenderer>())_hitTarget.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
            else
            {
                if(_hitTarget.gameObject.GetComponent<MeshRenderer>())_hitTarget.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
    }
}
