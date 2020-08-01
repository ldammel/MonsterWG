using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monster_WG
{
    public class WallVisibilityManager: MonoBehaviour
    {
        private static WallVisibilityManager _mgr;

        private int _roomMask = int.MaxValue;
        
        [SerializeField]
        private List<Wall> walls = new List<Wall>(18);

        private void Awake()
        {
            _mgr = this;
        }

        public static void Refresh()
        {
            //Debug.Log(Convert.ToString(_mgr._roomMask, 2));
            //Debug.Log(IsBad());
            //Debug.Log(IsWohnzimmer());
            _mgr.walls[0].wall.SetActive(IsBad());
            _mgr.walls[1].wall.SetActive(IsBad()&&IsWohnzimmer());
            _mgr.walls[2].wall.SetActive(IsBalkon());
            _mgr.walls[3].wall.SetActive(IsBalkon()&&IsBad());
            _mgr.walls[4].wall.SetActive(IsEsszimmer());
            _mgr.walls[5].wall.SetActive(IsEsszimmer()&&IsKueche());
            _mgr.walls[6].wall.SetActive(IsFlur());
            _mgr.walls[7].wall.SetActive(IsJimmy());
            _mgr.walls[8].wall.SetActive(IsJimmy()&&IsFlur());
            _mgr.walls[9].wall.SetActive(IsJimmy()&&IsWohnzimmer());
            _mgr.walls[10].wall.SetActive(IsKammer());
            _mgr.walls[11].wall.SetActive(IsKammer()&&IsFlur());
            _mgr.walls[12].wall.SetActive(IsKueche());
            _mgr.walls[13].wall.SetActive(IsRoy());
            _mgr.walls[14].wall.SetActive(IsRoy()&&IsKammer());
            _mgr.walls[15].wall.SetActive(IsWohnzimmer());
            _mgr.walls[16].wall.SetActive(IsWohnzimmer()&&IsBalkon());
            _mgr.walls[17].wall.SetActive(IsWohnzimmer()&&IsEsszimmer());

        }

        private static bool IsBad()
        {
            return (_mgr._roomMask & 1) == 1;
        }
        private static bool IsBalkon()
        {
            return (_mgr._roomMask & 2) == 2;
        }
        private static bool IsWohnzimmer()
        {
            return (_mgr._roomMask & 4) == 4;
        }
        private static bool IsEsszimmer()
        {
            return (_mgr._roomMask & 8) == 8;
        }
        private static bool IsJimmy()
        {
            return (_mgr._roomMask & 16) == 16;
        }
        private static bool IsRoy()
        {
            return (_mgr._roomMask & 32) == 32;
        }
        private static bool IsKueche()
        {
            return (_mgr._roomMask & 64) == 64;
        }
        private static bool IsFlur()
        {
            return (_mgr._roomMask & 128) == 128;
        }
        private static bool IsKammer()
        {
            return (_mgr._roomMask & 256) == 256;
        }

        public static void SetRoomMaskAtIndex(int index)
        {
            _mgr._roomMask |= 1 << index;
        }
        
        public static void ResetRoomMaskAtIndex(int index)
        {
            _mgr._roomMask &= ~(1 << index);
        }
    }

    public static class Helper
    {
        public static bool IsPlayer(GameObject obj)
        {
            return obj.CompareTag("Player") || obj.CompareTag("Player2");
        }
    }
}