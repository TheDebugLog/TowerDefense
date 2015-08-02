using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            GameStats = new Statistics(0, 0, 0);
          //call the IOManager to get the information from the player settings
            Leaderboard = IOManager.ReadLeaderboard();
        }


        public bool TryUpdateLeaderBoard()
        {
            bool leaderboardChanged = false;

            //if the statistic is under the leaderboard you will return true.
            if (Leaderboard.Count < 5 || Leaderboard[Leaderboard.Count-1].Score < GameStats.Score)
            {
                if (Leaderboard.Count < 5)
                {
                    Leaderboard.Add(GameStats);
                }
                else
                {
                    Leaderboard[Leaderboard.Count-1] = GameStats;
                }
                Leaderboard.Sort();
                Leaderboard.Reverse();
               
                leaderboardChanged = true;
            }

            return leaderboardChanged;
        }

        void OnApplicationQuit()
        {
            //Leaderboard = new List<Statistics>();
            IOManager.SaveLeaderboard(Leaderboard);
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

        public List<Statistics> Leaderboard {get; private set;}
        #endregion
    }
}

