using System;
using System.Collections;
using System.Collections.Generic;
using Game.Interactions;
using Game.Utility;
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

    public Interaction.InteractionResult WouldMetCondition(PlayerInteractionController player)
    {
        return CheckConditionForPlayer(player, false);
    }

    public Interaction.InteractionResult IsConditionMet()
    {
        _controller = GetComponent<Interaction>().player;
        return CheckConditionForPlayer(_controller, true);
    }

    private Interaction.InteractionResult CheckConditionForPlayer(PlayerInteractionController player, bool log)
    {
        // Player in range?
        if (!player) { if (log) { Debug.Log("No player in range!", gameObject); } return Interaction.InteractionResult.Failed; }

        // no item needed?
        if (!NeedsItem && player.CurrentItem){
            if (log)
            {
                Debug.Log("Player needs free hands!", gameObject);
            } 
            return Interaction.InteractionResult.NeedsFreeHands; 
        }
        if (!NeedsItem && !player.CurrentItem) return Interaction.InteractionResult.Success;

        // item needed?
        if (!player.CurrentItem){
            if (log)
            {
                Debug.Log("Player has no item", gameObject);
            } 
            return Interaction.InteractionResult.NeedsItem; 
        } 

        // item has correct tag?
        if (!player.CurrentItem.gameObject.CompareTag(cleaningObjectTag)) {
            if (log)
            {
                Debug.Log("Item has incorrect tag!", gameObject);
            } 
            return Interaction.InteractionResult.WrongType; 
        }

        // watered item needed?
        if (NeedsWater && player.CurrentItem.CurrentWaterAmount < NeededWaterAmount){
            if (log)
            {
                Debug.Log("Item has not enough water!", gameObject);
            } 
            SoundManager.Instance.Play(gameObject, SoundManager.Sounds.InputWrong);
            return Interaction.InteractionResult.NeedsWater;
        } 

        return Interaction.InteractionResult.Success;
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
        if (!other.CompareTag("Player") || !other.CompareTag("Player2")) return;

        IsMet = false;
        _controller = null;
    }
}
