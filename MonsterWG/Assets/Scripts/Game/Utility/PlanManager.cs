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
            StartCoroutine(ToggleCanvas(true, canvasStartWaitTime));
        }
        
        public void DisableCanvas()
        {
            StartCoroutine(ToggleCanvas(false, 0.1f));
        }

        private IEnumerator ToggleCanvas(bool toggle, float time)
        {
            yield return new WaitForSeconds(time);
            planCanvas.SetActive(toggle);
        }
    }
}
