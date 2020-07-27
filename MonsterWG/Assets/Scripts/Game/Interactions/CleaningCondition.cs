using System;
using System.Collections;
using System.Collections.Generic;
using Game.Interactions;
using UnityEngine;

public class CleaningCondition : MonoBehaviour
{
    public bool IsMet { get; private set; }
    private PlayerInteractionController _controller;
    [SerializeField] private string cleaningObjectTag;

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
