using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TDL {
    public class WaveManager : MonoBehaviour {
        //The current wave we are on
        private int _currentWaveIndex = -1;
        private Wave _currentWave = null;

        //Public for debugging.  Allows us to see hierarchy
        public List<Wave> _waves;

        //Get all our children of type wave
        void Awake() {
            _waves = new List<Wave>();
            for (int i = 0; i < transform.childCount; i++) {
                _waves.Add(transform.GetChild(i).GetComponent<Wave>());
            }
        }

        //Begin the waves starting from the 0th
        void Start() {
            NextWave();
        }

        //Get the current wave and tell it to start spawning.  Hook up
        //event handler to listen for wave completion
        private void NextWave() {
            _currentWaveIndex++;
            if (_currentWaveIndex < _waves.Count) {
                Debug.Log("Begin wave " + _currentWaveIndex);
                _currentWave = _waves[_currentWaveIndex];
                _currentWave.WaveComplete += new Wave.WaveCompleteEventHandler(OnWaveComplete);
                _currentWave.BeginSpawning();
            } else {
                Debug.Log("Waves complete.  GAME OVER");
            }
        }

        //On wave completion stop listening for the wave complete event
        //and move to the next wave
        private void OnWaveComplete(object sender, EventArgs args) {
            Debug.Log("Wave complete!");
            _currentWave.WaveComplete -= OnWaveComplete;
            NextWave();
        }
    }
}
