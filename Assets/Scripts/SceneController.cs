using UnityEngine;
using System.Collections;
using System;

namespace TDL
{
    public class SceneController : MonoBehaviour
    {
        public void GoToScene(string sceneName)
        {
            Scene sceneToLoad = (Scene)Enum.Parse(typeof(Scene), sceneName);
            SceneManager.Instance.LoadScene(sceneToLoad);
        }
    }

}
