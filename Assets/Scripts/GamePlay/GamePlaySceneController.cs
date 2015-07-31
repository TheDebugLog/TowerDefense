using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TDL
{
    public class GamePlaySceneController : SceneController
    {
        #region Variables
        public Text scoreText;
        public Text killsText;
        public Text waveText;
        int _score = 0;
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
            Statistics stats = new Statistics(_score, _kills, _wave);
            Session.Instance.GameStats = stats;
            base.GoToScene(sceneName);
            //GoToScene(Scene.GameOver);
        }

        public void IncreaseCredits()
        {
            _score++;
            scoreText.text = "Score: " + _score.ToString();
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
            _score = gameStats.Score;
            _kills = gameStats.Kills;
            _wave = gameStats.Wave;
            scoreText.text = "Score: " + _score.ToString();
            killsText.text = "Kills: " + _kills.ToString();
            waveText.text = "Wave: " + _wave.ToString();
        }
        #endregion
    }
}

