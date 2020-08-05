using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Game.Utility
{
    public class VFXPlayer : MonoBehaviour
    {
        public void Play()
        {
            foreach(var vfx in GetComponentsInChildren<VisualEffect>())
            {
                vfx.Play();
            }
        }

        public void Stop()
        {
            foreach (var vfx in GetComponentsInChildren<VisualEffect>())
            {
                vfx.Stop();
            }
        }
    }
}
