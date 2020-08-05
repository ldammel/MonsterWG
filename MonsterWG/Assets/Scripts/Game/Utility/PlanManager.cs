using System.Collections;
using Game.Interactions;
using UnityEngine;

namespace Game.Utility
{
    public class PlanManager : MonoBehaviour
    {
        [SerializeField] private GameObject planCanvas;
        [SerializeField] private float canvasStartWaitTime;

        public PlayerInteractionController CurPlayer { get; set; }
        private bool _active;

        public void EnablePlanMovement()
        {
            CurPlayer.character.canMove = false;
            EnableCanvas();
        }

        public void DisablePlanMovement()
        {
            CurPlayer.character.canMove = true;
            DisableCanvas();
        }

        public void EnableCanvas()
        {
            _active = true;
            StartCoroutine(ToggleCanvas( canvasStartWaitTime));
        }
        
        public void DisableCanvas()
        {
            _active = false;
            StartCoroutine(ToggleCanvas( 0.1f));
        }

        private IEnumerator ToggleCanvas(float time)
        {
            yield return new WaitForSeconds(time);
            planCanvas.SetActive(_active);
        }
    }
}
