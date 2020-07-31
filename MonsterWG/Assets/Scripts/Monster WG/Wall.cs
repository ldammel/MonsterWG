using System;
using UnityEngine;


[Serializable]
public struct Wall
{
   public string name;
   public GameObject wall;

   public void Show()
   {
      wall.SetActive(true);
   }

   public void Hide()
   {
      wall.SetActive(false);
   }
}

