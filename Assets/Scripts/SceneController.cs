using UnityEngine;
using System.Collections;
using System;

namespace TDL
{
    //TODO add description   Eduardo Castillo Fernandez  8/8/2015
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
