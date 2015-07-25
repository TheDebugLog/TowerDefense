using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TDL {
    public class Wave : MonoBehaviour {
        //Our delegate and events to notify the wave as complete
        public delegate void WaveCompleteEventHandler(object sender, EventArgs args);
        public event WaveCompleteEventHandler WaveComplete;

        //Our list of spawners to create
        public List<Spawner> _spawners;        

        //The wave's current spawner
        private int _currentSpawnerIndex = -1;
        private Spawner _currentSpawner = null;

        void Awake() {
            _spawners = new List<Spawner>();
            for (int i = 0; i < transform.childCount; i++) {
                _spawners.Add(transform.GetChild(i).GetComponent<Spawner>());
            }
        }

        void Start() {
        }

        //Find the next Spawner and activate it.
        public void BeginSpawning() {
            if (_spawners.Count > 0) {
                NextSpawner();
            } else {
                Debug.LogWarning("There are no waves defined!");
            }
        }

        //Tell the spawn to start spawning and listen for the event 
        //when it completes
        private void NextSpawner() {
            _currentSpawnerIndex++;
            if (_currentSpawnerIndex < _spawners.Count) {
                _currentSpawner = _spawners[_currentSpawnerIndex];
                _currentSpawner.SpawnComplete += new Spawner.SpawnerCompleteEventHandler(OnSpawnComplete);
                _currentSpawner.BeginSpawn();
            } else {
                WaveComplete(this, EventArgs.Empty);
            }
        }

        //Spawn complete event handler.  Stop listening for event and move
        //to next spawner.
        private void OnSpawnComplete(object sender, EventArgs args) {
            Debug.Log("Spawning complete!");
            _currentSpawner.SpawnComplete -= OnSpawnComplete;
            NextSpawner();
        }
    }
}
