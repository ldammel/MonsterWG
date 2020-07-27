using System.Collections;
using Game.Interactions;
using UnityEngine;

namespace Game.Utility
{
    public class PlanManager : MonoBehaviour
    {
        [SerializeField] private GameObject planCanvas;
        [SerializeField] private float canvasStartWaitTime;
        [SerializeField] private GameObject[] menuObjects;
        
        public PlayerInteractionController CurPlayer { get; set; }

        public void EnablePlanMovement()
        {
            CurPlayer.character.canMove = false;
        }

        public void DisablePlanMovement()
        {
            CurPlayer.character.canMove = true;
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
