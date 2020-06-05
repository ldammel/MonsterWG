using System;
using System.Collections;
using Game.Interactions;
using Game.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Utility
{
    public class PlanManager : MonoBehaviour
    {
        [SerializeField] private GameObject planCanvas;
        [SerializeField] private float canvasStartWaitTime;
        [SerializeField] private GameObject[] menuObjects;
        [SerializeField] private RoomDisplay[] roomdisplays;

        private QuestDisplay _questManager;
        public PlayerInteractionController CurPlayer { get; set; }
        private int _count;
        private bool _triggered;

        private void Start()
        {
            _questManager = FindObjectOfType<QuestDisplay>();
        }

        private void Update()
        {
            if (!CurPlayer) return;
            if (CurPlayer.character.canMove) return;
            if (_triggered) return;
            MoveElement();
        }

        public void EnablePlanMovement()
        {
            CurPlayer.character.canMove = false;
            _count = 0;
        }

        public void DisablePlanMovement()
        {
            CurPlayer.character.canMove = true;
        }

        public void MoveElement()
        {
            _triggered = true;
            if (menuObjects.Length <= 1) return;
            var movementInput = CurPlayer.isPlayerOne
                ? CurPlayer.character.controls.Player.Move.ReadValue<Vector2>()
                : CurPlayer.character.controls.Player2.Move.ReadValue<Vector2>();

            var movement = new Vector3
            {
                x = movementInput.x,
                z = movementInput.y
            };
            if (movement.x >= 1)
            {
                if (_count == menuObjects.Length - 1)
                {
                    menuObjects[_count].SetActive(false);
                    _count = 0;
                    menuObjects[_count].SetActive(true);
                }
                else
                {
                    menuObjects[_count].SetActive(false);
                    _count++;
                    while(!roomdisplays[_count].IsInitialized)
                    {
                        if (_count == menuObjects.Length -1)
                        {
                            return;
                        }
                        _count++;
                    }
                    menuObjects[_count].SetActive(true);
                }
            }

            if (movement.x <= -1)
            {
                if (_count == 0)
                {
                    menuObjects[_count].SetActive(false);
                    _count = menuObjects.Length - 1;
                    menuObjects[_count].SetActive(true);
                }
                else
                {
                    menuObjects[_count].SetActive(false);
                    _count--;
                    while(!roomdisplays[_count].IsInitialized)
                    {
                        if (_count == 0)
                        {
                            return;
                        }
                        _count--;
                    }
                    menuObjects[_count].SetActive(true);
                }
            }
            StartCoroutine(WaitTrigger());
        }

        private IEnumerator WaitTrigger()
        {
            yield return new WaitForSeconds(.1f);
            _triggered = false;
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
