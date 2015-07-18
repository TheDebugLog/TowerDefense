using System;
using UnityEngine;

namespace TDL {
    class Spawner : MonoBehaviour {
        public int minionCount = 0;
        public float waitTime = 0f;

        public UnityEngine.Object prefab = null;

        private float _lastSpawnTime = 0f;
        private int _numSpawned = 0;

        void Awake() {
            _lastSpawnTime = Time.time;
        }

        void Start() {

        }

        void Update() {
            if (Time.time - _lastSpawnTime > waitTime && _numSpawned < minionCount) {
                _numSpawned++;
                _lastSpawnTime = Time.time;
                Debug.Log("SPAWN! "+_numSpawned.ToString());
                GameObject minion = (GameObject)GameObject.Instantiate(prefab);
                minion.transform.position = this.transform.position;
            }
        }

    }
}
