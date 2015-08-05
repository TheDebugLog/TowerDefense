using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace TDL
{
    public class GamePlaySceneController : SceneController
    {
        #region Variables
        public Text scoreText;
        public Text killsText;
        public Text waveText;
        public Text turretText;
        public Button WaveButton;
        public bool addTurretMode = false;
        public Camera sceneCamera;
        public GameObject turretPrefab;
        int _score = 0;
        int _kills = 0;
        int _wave = 0;
        int _turret = 0;
        RaycastHit hit;
        #endregion

        #region Methods
        void Start()
        {
            SetUpStatistics(Session.Instance.GameStats);
            WaveManager.Instance.NextWaveEvent += new WaveManager.NextWaveEventHandler(OnNextWave);
        }

        public override void GoToScene(string sceneName)
        {
            //Saves the statistics in the Session object
            Statistics stats = new Statistics(_score, _kills, _wave);
            Session.Instance.GameStats = stats;
            base.GoToScene(sceneName);
            //GoToScene(Scene.GameOver);
        }

        public void IncreaseScore()
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
            WaveManager.Instance.NextWave();
            WaveButton.interactable = false;
        }

        public void AddTurret()
        {
            addTurretMode = true;
            _turret++;
            turretText.text = "Turret: " + _turret.ToString();
            //TODO: subtract cost points
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

        private void OnNextWave(object sender, NextWaveEventArgs args) {
            if(args.GameOver) {
                Debug.Log("Received game over message!");
            } else {
                WaveButton.interactable = true;
            }
        }

        void Update()
        {
            if(addTurretMode == true)
            {
                //This needs to be where I click
                Vector3 fwd = sceneCamera.gameObject.transform.TransformDirection(Vector3.forward);
                if(Physics.Raycast(sceneCamera.gameObject.transform.position, fwd, out hit))
                {
                    if(hit.collider.gameObject.tag == "OccupiableSpace")
                    {
                        if(Input.GetMouseButtonDown(0))
                        {
                            OccupiableSpace placementCube = hit.collider.gameObject.GetComponent<OccupiableSpace>();
                            Vector3 hitPosition = hit.collider.gameObject.transform.position;
                            float verticalAdjuster = hit.collider.gameObject.transform.localScale.y / 2;
                            Instantiate(turretPrefab, new Vector3(hitPosition.x, (hitPosition.y + verticalAdjuster), hitPosition.z),Quaternion.identity);
                            placementCube.isOccupied = true;
                            addTurretMode = false;
                        }
                    }
                }
            }
        }
        #endregion
    }
}

