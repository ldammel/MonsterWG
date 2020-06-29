using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class IconManager : MonoBehaviour
    {
        #region Singleton
        public static IconManager instance;
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("There can only be one instance of FinalScoring in the Scene!");
                Application.Quit();
            }
            instance = this;
        }
        #endregion
        
        [SerializeField] private Image image;
        [SerializeField] private float displayTime;

        private void Start()
        {
            image.enabled = false;
        }

        public void DisplayIcon(Sprite icon)
        {
            StartCoroutine(SwapSprite(icon));
        }

        private IEnumerator SwapSprite(Sprite icon)
        {
            image.sprite = icon;
            image.enabled = true;
            yield return new WaitForSeconds(displayTime);
            image.enabled = false;
        }
    }
}
