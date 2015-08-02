using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace TDL
{
    public class GameOverSceneController : SceneController
    {
        #region Variables
        public Text scoreText;
        public Text killsText;
        public Text waveText;
        public Text[] leaderboard;
        #endregion

        void Start()
        {
            SetUpStatistics(Session.Instance.GameStats);
            Session.Instance.TryUpdateLeaderBoard();
            SetUpLeaderboard(Session.Instance.Leaderboard);
        }

        void SetUpLeaderboard(List<Statistics> leaderboardInfo)
        {
            for (int i = 0; i < leaderboard.Length; i++)
            {
                if (leaderboardInfo.Count < i + 1)
                {
                    leaderboard[i].text = "";
                }
                else
                {
                    leaderboard[i].text = "Score: " + leaderboardInfo[i].Score;
                }
            }
        }

        void SetUpStatistics(Statistics gameStats)
        {
            scoreText.text = "Score: " + gameStats.Score.ToString();
            killsText.text = "Kills: " + gameStats.Kills.ToString();
            waveText.text = "Waves: " + gameStats.Wave.ToString();
        }

        public override void GoToPreviousScene()
        {

            //Resetting the statistics
            Session.Instance.GameStats = new Statistics();

 	        base.GoToPreviousScene();
        }
    }
}
