using ShopGame.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ShopGame.UIScreens
{
    public abstract class GameStateUIScreen : UIScreen
    {
        public void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
