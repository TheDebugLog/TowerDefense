using UnityEngine;
using System.Collections;

namespace TDL
{
    public class MainMenuSceneController : MonoBehaviour
    {
        public SceneManager _sceneManager;

        public void GoToScene(string sceneName)
        {
            switch (sceneName)
            {
                case "Level1":
                {
                    _sceneManager.LoadScene(Scene.Level1);
                    break;
                }
                default:
                    break;
            }
        }
    }
}

