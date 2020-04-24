using System.Collections;
using UnityEngine;

namespace Game.Utility
{
    public class SceneManager : MonoBehaviour
    {
        public void Restart()
        {
            StartCoroutine(StartGame());
        }
        
        public void MainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public void Quit()
        {
            Application.Quit();
        }

        private IEnumerator StartGame()
        {
            yield return new WaitForSeconds(3);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}
