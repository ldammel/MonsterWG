using UnityEngine;

namespace Game.Utility
{
    public class SceneManager : MonoBehaviour
    {
        public void Restart()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        
        public void MainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
