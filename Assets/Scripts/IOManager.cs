using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDL
{
    class IOManager
    {
        const string LEADERBOARD_NAME = "Leaderboard";

        /// <summary>
        /// Saves the leaderboard to harddrive
        /// </summary>
        /// <param name="leaderboard">Leaderboard to save</param>
        public static void SaveLeaderboard(List<Statistics> leaderboard)
        {
            PlayerPrefs.SetString(LEADERBOARD_NAME, Parser.GetLeaderboardText(leaderboard));
        }

        public static List<Statistics> ReadLeaderboard()
        {
            return Parser.ParseLeaderboard(PlayerPrefs.GetString(LEADERBOARD_NAME));
        }
    }
}
