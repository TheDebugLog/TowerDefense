using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDL
{
    /// <summary>
    /// Convert game elements to string and vice versa
    /// </summary>
    class Parser
    {
        const char STATISTICS_SEPARATOR = '|';
        const char STATISTICS_ELEMENTS_SEPARATOR = ',';

        public static List<Statistics> ParseLeaderboard(string leaderboardText)
        {
            List<Statistics> leaderboard = new List<Statistics>();

            if (String.IsNullOrEmpty(leaderboardText))
            {
                return leaderboard;
            }

            //Separates the statistics by splitting the string with |
            string[] statistics = leaderboardText.Split(STATISTICS_SEPARATOR);
          
            //separates the parts of a statistic 
            try
            {
                for (int i = 0; i < statistics.Length; i++)
                {
                    string[] statisticElements = statistics[i].Split(STATISTICS_ELEMENTS_SEPARATOR);
                    leaderboard.Add(new Statistics(int.Parse(statisticElements[0]),
                                                   int.Parse(statisticElements[1]),
                                                   int.Parse(statisticElements[2])));
                }
            }
            catch (InvalidCastException ex)
            {
                Debug.Log("The leaderboard got corrupted. The exception message is: " + ex.Message);
            }
   
            return leaderboard;
        }

        public static string GetLeaderboardText( List<Statistics> leaderboard)
        {
            StringBuilder leaderboardText = new StringBuilder();
            for (int i = 0; i < leaderboard.Count; i++)
            {
                leaderboardText.Append(leaderboard[i].Score);
                leaderboardText.Append(STATISTICS_ELEMENTS_SEPARATOR);
                leaderboardText.Append(leaderboard[i].Kills);
                leaderboardText.Append(STATISTICS_ELEMENTS_SEPARATOR);
                leaderboardText.Append(leaderboard[i].Wave);

                if (i < leaderboard.Count - 1)
                {
                    leaderboardText.Append(STATISTICS_SEPARATOR);
                }
            }

            return leaderboardText.ToString();
        }
    }
}
