using System;
using UnityEngine;

namespace TDL {
    public class Spawner : MonoBehaviour {
        //Our delegate and events to notify the wave as complete
        public delegate void SpawnerCompleteEventHandler(object sender, EventArgs args);
        public event SpawnerCompleteEventHandler SpawnCompleteEvent;
        
        public int minionCount = -1;
        public float waitTime = 0f;

        public UnityEngine.Object prefab = null;

        private float _lastSpawnTime = 0f;
        private int _numSpawned = 0;

        private bool _ready = false;

        void Awake() {
            _lastSpawnTime = Time.time;
        }

        public void BeginSpawn() {
            if (minionCount > 0) {
                _ready = true;
            } else {
                SpawnCompleteEvent(this, EventArgs.Empty);
            }
        }

        void Update() {
            if (_ready) {
                if (Time.time - _lastSpawnTime > waitTime) {
                    NextSpawn();
                }
            }
        }

        void Start() {

        }

        void NextSpawn() {
            if (_numSpawned < minionCount) {
                _numSpawned++;
                _lastSpawnTime = Time.time;
                //Debug.Log("SPAWN! "+_numSpawned.ToString());
                GameObject minion = (GameObject)GameObject.Instantiate(prefab);
                minion.transform.position = this.transform.position;
            } else {
                SpawnCompleteEvent(this, EventArgs.Empty);
                _ready = false;
            }
        }

    }
}
