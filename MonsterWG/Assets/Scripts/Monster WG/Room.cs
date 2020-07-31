using System;
using System.Collections;
using System.Collections.Generic;
using Monster_WG;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Room : MonoBehaviour
{
    public string name;
    public int id;
    private int visitors;

    public int Visitors => visitors;

    private void OnTriggerEnter(Collider other)
    {
        if (Helper.IsPlayer(other.gameObject))
        {
            AddVisitor();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (Helper.IsPlayer(other.gameObject))
        {
            RemoveVisitor();
        }
    }

    private void AddVisitor()
    {
        visitors++;
        Update();
        WallVisibilityManager.Refresh();
    }

    private void RemoveVisitor()
    {
        visitors--;
        Update();
        WallVisibilityManager.Refresh();
    }

    private void Update()
    {
        if (visitors > 0)
        {
            WallVisibilityManager.ResetRoomMaskAtIndex(id);
        }
        else
        {
            WallVisibilityManager.SetRoomMaskAtIndex(id);
        }
    }
}
