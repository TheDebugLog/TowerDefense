using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TDL
{
    public class GamePlaySceneController : SceneController
    {
        #region Variables
        public Text creditsText;
        public Text killsText;
        public Text waveText;
        int _credits = 0;
        int _kills = 0;
        int _wave = 1;
        #endregion

        #region Methods
        void Start()
        {
            SetUpStatistics(Session.Instance.GameStats);
        }

        public override void GoToScene(string sceneName)
        {
            //Saves the statistics in the Session object
            Statistics stats = new Statistics(_credits, _kills, _wave);
            Session.Instance.GameStats = stats;
            base.GoToScene(sceneName);
            //GoToScene(Scene.GameOver);
        }

        public void IncreaseCredits()
        {
            _credits++;
            creditsText.text = "Credits: " + _credits.ToString();
        }

        public void IncreaseKills()
        {
            _kills++;
            killsText.text = "Kills: " + _kills.ToString();
        }

        public void IncreaseWave()
        {
            _wave++;
            waveText.text = "Wave: " + _wave.ToString();
        }

        void SetUpStatistics(Statistics gameStats)
        {
            _credits = gameStats.Credits;
            _kills = gameStats.Kills;
            _wave = gameStats.Wave;
            creditsText.text = "Credits: " + _credits.ToString();
            killsText.text = "Kills: " + _kills.ToString();
            waveText.text = "Wave: " + _wave.ToString();
        }
        #endregion
    }
}

