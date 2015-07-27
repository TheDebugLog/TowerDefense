using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace TDL
{
    public class GameOverSceneController : SceneController
    {
        #region Variables
        public Text creditsText;
        public Text killsText;
        public Text waveText;
        #endregion

        void Start()
        {
            //SetUpStatistics(new Statistics(10,5,5));
            SetUpStatistics(Session.Instance.GameStats);
        }

        void SetUpStatistics(Statistics gameStats)
        {
            creditsText.text = "Credits: " + gameStats.Credits.ToString();
            killsText.text = "Kills: " + gameStats.Kills.ToString();
            waveText.text = "Waves: " + gameStats.Wave.ToString();
        }

        public override void GoToPreviousScene()
        {
            //TODO if the stats are high enough add them to the leaderboard

            //Resetting the statistics
            Session.Instance.GameStats = new Statistics();

 	        base.GoToPreviousScene();
        }
    }
}
