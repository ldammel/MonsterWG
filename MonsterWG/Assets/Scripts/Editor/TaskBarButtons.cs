using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TaskBarButtons : MonoBehaviour
{
    [MenuItem("FindObjectsWithTag/Water")]
    public static void FindObjectsWithTagWater()
    {
        FindObjectsWithTag("Water");
    }

    private static void FindObjectsWithTag(string tag)
    {
        foreach(var item in GameObject.FindGameObjectsWithTag(tag))
        {
            Debug.Log(string.Format("Found asset with tag {0}", tag), item);
        }
    }
}
