using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace TDL
{
    //Todo Add description 
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
        private int _score = 0;
        private int _kills = 0;
        private int _wave = 0;
        private int _turret = 0;
        private RaycastHit hit;
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
            scoreText.text = ScoreText;
        }

        public void IncreaseKills()
        {
            _kills++;
            killsText.text = KillText;
        }

        public void IncreaseWave()
        {
            _wave++;
            waveText.text = WaveText;
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
            scoreText.text = ScoreText;
            killsText.text = KillText;
            waveText.text = WaveText;
        }

        public string KillText {
            get {
                return "Kills: " + _kills.ToString();
            }
        }

        public string ScoreText {
            get {
                return "Score: " + _score.ToString();
            }
        }

        public string WaveText {
            get {
                return "Wave: " + _wave.ToString();
            }
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
               /* Vector3 fwd = sceneCamera.gameObject.transform.TransformDirection(Vector3.forward);
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
                }*/
                if(Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Mouse button is pressed");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit = new RaycastHit();
                    if(Physics.Raycast(ray,out hit))
                    {
                        if(hit.collider.tag =="OccupiableSpace")
                        {
                            OccupiableSpace placementCube = hit.collider.gameObject.GetComponent<OccupiableSpace>();
                            if(placementCube.isOccupied == false)
                            {
                                Vector3 hitPosition = hit.collider.gameObject.transform.position;
                                GameObject spawnedTurret = Instantiate(turretPrefab, new Vector3(hitPosition.x, (hitPosition.y + 2f ), hitPosition.z),Quaternion.identity) as GameObject;
                                spawnedTurret.transform.localScale = new Vector3(0.004f, 0.004f, 0.004f);
                                placementCube.isOccupied = true;
                                addTurretMode = false;
                            }
                            
                        }
                        else
                        {
                            Debug.Log("Not a good placement or space already occupied");
                        }
                    }
                }
            }
        }
        #endregion

        #region EventHandlers
        public void OnMinionDeath(object sender, MinionDeathArgs args) {
            _kills++;
            killsText.text = KillText;
            _score += args.Points;
            scoreText.text = ScoreText;
            //Debug.Log("A minion died!");
        }
        #endregion
    }
}

