using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TDL {
    public class NextWaveEventArgs : EventArgs {
        public bool GameOver { get; set; }
    }

    public class WaveManager : MonoBehaviour {
        //Our delegate and events to notify the wave as complete
        public delegate void NextWaveEventHandler(object sender, NextWaveEventArgs args);
        public event NextWaveEventHandler NextWaveEvent;

        //Public for debugging.  Allows us to see hierarchy
        public List<Wave> _waves;

        public static WaveManager Instance {
            get { return _instance; }
        }

        private static WaveManager _instance;

        //The current wave we are on
        private int _currentWaveIndex = -1;
        private Wave _currentWave = null;

        //Get all our children of type wave
        void Awake() {
            _instance = this;

            _waves = new List<Wave>();
            for (int i = 0; i < transform.childCount; i++) {
                _waves.Add(transform.GetChild(i).GetComponent<Wave>());
            }
        }

        //Begin the waves starting from the 0th
        void Start() {
        }

        public Wave CurrentWave {
            get{ return _currentWave; }
        }

        //Get the current wave and tell it to start spawning.  Hook up
        //event handler to listen for wave completion
        public void NextWave() {
            _currentWaveIndex++;
            if (_currentWaveIndex < _waves.Count) {
                Debug.Log("Begin wave " + _currentWaveIndex);
                _currentWave = _waves[_currentWaveIndex];
                _currentWave.WaveCompleteEvent += new Wave.WaveCompleteEventHandler(OnWaveComplete);
                _currentWave.BeginSpawning();
            } else {
                Debug.Log("Waves complete.  GAME OVER");
            }
        }

        //On wave completion stop listening for the wave complete event
        //and move to the next wave
        private void OnWaveComplete(object sender, EventArgs args) {
            Debug.Log("Wave complete!");
            _currentWave.WaveCompleteEvent -= OnWaveComplete;

            NextWaveEventArgs nextWaveArgs = new NextWaveEventArgs();
            if (_currentWaveIndex + 1 >= _waves.Count) {
                nextWaveArgs.GameOver = true;
            } else {
                nextWaveArgs.GameOver = false;
            }

            NextWaveEvent(this, nextWaveArgs);
        }
    }
}
