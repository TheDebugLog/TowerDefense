using UnityEngine;
using System.Collections;
using System;

namespace TDL
{
    public class SceneController : MonoBehaviour
    {
        public virtual void GoToScene(string sceneName)
        {
            Scene sceneToLoad = (Scene)Enum.Parse(typeof(Scene), sceneName);
            SceneManager.Instance.LoadScene(sceneToLoad);
        }

        public virtual void GoToScene(Scene sceneToLoad)
        {
            SceneManager.Instance.LoadScene(sceneToLoad);
        }

        public virtual void GoToPreviousScene()
        {
            SceneManager.Instance.GoToPreviousScene();
        }

        public virtual void Quit()
        {
            Application.Quit();
        }
    }

}
