using UnityEngine;
using System.Collections;

namespace TDL
{
    /// <summary>
    /// Contains the logic of the applicaiton. 
    /// Determines where to go after an event occurs. 
    /// Decides when to load and save the data. 
    /// Contains the state of the game and the content of it.
    /// 
    /// Changes from one screen to the other.
    /// Decides when to load and save the data.
    /// </summary>
    public class SceneManager : MonoBehaviour
    {
        #region Variables
        Scene _currentScene;
        #endregion


        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            _currentScene = Scene.MainMenu;
        }

        public void LoadScene(Scene sceneToLoad)
        {
            switch (_currentScene)
            {
                case Scene.MainMenu:
                    {
                        GoFromMainMenuSceneTo(sceneToLoad);
                        break;
                    }
            
                default:
                    {
                        Debug.Log("The scene " + _currentScene.ToString() + "doesn't know how to go to " + sceneToLoad.ToString());
                    }
                    break;
            }
        }

        void GoFromMainMenuSceneTo(Scene sceneToLoad)
        {
            switch (sceneToLoad)
            {
                case Scene.Level1:
                    {
                        GoToScene(sceneToLoad);
                        break;
                    }

                default:
                    {
                        Debug.Log("You cannot go from Challenge Selection scene to " + sceneToLoad.ToString());
                        break;
                    }
            }
        }

        void GoToScene(Scene sceneToLoad)
        {
            _currentScene = sceneToLoad;
            Application.LoadLevel(sceneToLoad.ToString());
        }
    }
}

