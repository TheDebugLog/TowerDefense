using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        static SceneManager _instance = null;
        Dictionary<Scene, List<Scene>> _transitions = new Dictionary<Scene, List<Scene>>()
        {
            {Scene.Splash,new List<Scene>{Scene.MainMenu}},
            {Scene.MainMenu,new List<Scene>{Scene.GamePlay,Scene.Settings}},
            {Scene.Settings, new List<Scene>{Scene.Credits, Scene.GamePlay, Scene.MainMenu}},
            {Scene.Credits, new List<Scene>{Scene.Settings}},
            {Scene.GamePlay, new List<Scene>{Scene.GameOver, Scene.Settings}},
            {Scene.GameOver, new List<Scene>{Scene.GamePlay}}
        };
        Stack<Scene> _previousStack;
        #endregion

        private SceneManager(){}

        #region Methods
        void Awake()
        {
            _instance = this;
        }

        // Use this for initialization
        void Start()
        {
            _previousStack = new Stack<Scene>();
            DontDestroyOnLoad(gameObject);
            CurrentScene = Scene.Splash;
        }

        public void GoToPreviousScene()
        {
                GoToScene(_previousStack.Pop());   
        }

        public void LoadScene(Scene sceneToLoad)
        {
            if (Transitions[CurrentScene].Contains(sceneToLoad))
            {
                _previousStack.Push(CurrentScene);
                GoToScene(sceneToLoad);
            }
            else
            {
                Debug.Log("You cannot go from " + CurrentScene.ToString() + " scene to " + sceneToLoad.ToString());
            }
        }


        void GoToScene(Scene sceneToLoad)
        {
            CurrentScene = sceneToLoad;
            Application.LoadLevel(sceneToLoad.ToString());
        }
        #endregion

        #region Properties
        /// <summary>
        /// Return the only instance of the SceneManager
        /// </summary>
        public static SceneManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public Scene CurrentScene
        {
            get;
            private set;
        }

        public Scene PreviousScene
        {
            get { return _previousStack.Peek(); }
        }

        public Dictionary<Scene, List<Scene>> Transitions
        {
            get
            {
                return _transitions;
            }
        }
        #endregion
    }
}

