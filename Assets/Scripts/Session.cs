using UnityEngine;
using System.Collections;

namespace TDL
{   
    /// <summary>
    /// Stores the data of the game.
    /// </summary>
    public class Session : MonoBehaviour
    {
        #region Variables
        static Session _instance = null;

        #endregion

        private Session() { }

        #region Methods
        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            GameStats = new Statistics(0, 0, 1);
            //TODO call the IOManager to get the information from the player settings
        }
        #endregion

        #region Properties
        public static Session Instance
        {
            get { return _instance; }
        }

        public Statistics GameStats
        {
            get;
            set;
        }
        #endregion
    }
}

