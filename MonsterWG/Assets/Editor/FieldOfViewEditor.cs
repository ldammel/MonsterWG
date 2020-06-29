﻿using Game.Utility;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor (typeof (FieldOfView))]
    public class FieldOfViewEditor : UnityEditor.Editor {

        void OnSceneGUI() {
            FieldOfView fow = (FieldOfView)target;
            Handles.color = Color.white;
            Handles.DrawWireArc (fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
            Vector3 viewAngleA = fow.DirFromAngle (-fow.viewAngle / 2, false);
            Vector3 viewAngleB = fow.DirFromAngle (fow.viewAngle / 2, false);

            Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
            Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

            Handles.color = Color.red;
            foreach (var visibleTarget in fow.visibleTargets) {
                Handles.DrawLine (fow.transform.position, visibleTarget.transform.position);
            }
        }

    }
}