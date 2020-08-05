using System;
using Game.Utility;
using UnityEngine;

namespace Game.Interactions
{
    public class ActivatePlan : MonoBehaviour
    {
        [HideInInspector]public PlayerInteractionController player;
        
        [SerializeField] private float highlightTime = 5;
        [SerializeField] private GameObject vCam;
        private PlanManager _manager;

        private bool _active;

        private void Start()
        {
            _manager = FindObjectOfType<PlanManager>();
        }

        public void Toggle()
        {
            SoundManager.Instance.Play(gameObject, SoundManager.Sounds.Putzplan);
            if (!_active)
            {
                vCam.SetActive(true);
                _manager.CurPlayer = player;
                _manager.EnablePlanMovement();
                _active = true;
            }
            else
            {
                vCam.SetActive(false);
                _manager.DisablePlanMovement();
                _manager.CurPlayer = null;
                _active = false;
                FindObjectOfType<UI.HighlightManager>().HighlightPlayerObjects(player, highlightTime);
            }
        }
    }
}
