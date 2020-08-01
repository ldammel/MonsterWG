using System;
using System.Collections;
using System.Collections.Generic;
using Game.Interactions;
using Sirenix.OdinInspector;
using UnityEngine;

public class CleaningCondition : MonoBehaviour
{
    public bool IsMet { get; private set; }

    public bool NeedsItem = true;
    [ShowIf(nameof(NeedsItem))]
    [SerializeField] private string cleaningObjectTag;

    [ShowIf(nameof(NeedsItem))]
    public bool NeedsWater = false;
    [ShowIf(nameof(NeedsWater))]
    public int NeededWaterAmount = 10;
    private PlayerInteractionController _controller;

    public bool IsConditionMet()
    {
        // Player in range?
        _controller = GetComponent<Interaction>().player;
        if (!_controller) { Debug.Log("No player in range!", gameObject); return false; }

        // no item needed?
        if (!NeedsItem && _controller.CurrentItem) { Debug.Log("Player needs free hands!", gameObject); return false; }
        if (!NeedsItem && !_controller.CurrentItem) return true;

        // item needed?
        if (!_controller.CurrentItem) { Debug.Log("Player has no item", gameObject); return false; }

        // watered item needed?
        if (NeedsWater && _controller.CurrentItem.CurrentWaterAmount < NeededWaterAmount) { Debug.Log("Item has not enough water!", gameObject); return false; }

        // item has correct tag?
        if(!_controller.CurrentItem.gameObject.CompareTag(cleaningObjectTag)) { Debug.Log("Item has incorrect tag!", gameObject); return false; }

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !other.CompareTag("Player2")) return;
        _controller = other.GetComponentInChildren<PlayerInteractionController>();
        if (!_controller.CurrentItem) return;
        if (_controller.CurrentItem.gameObject.CompareTag(cleaningObjectTag))
        {
            IsMet = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsMet = false;
        _controller = null;
    }
}
