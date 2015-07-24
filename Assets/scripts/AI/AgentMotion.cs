using System;
using UnityEngine;
using System.Collections.Generic;

namespace TDL {
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMotion : MonoBehaviour {
        public GameObject beaconList;

        private NavMeshAgent _agent = null;
        private int currentTarget = 0;

        void Start() {
            _agent = GetComponent<NavMeshAgent>();
            beaconList = GameObject.FindWithTag("BeaconList");
        }

        void Update() {
            _agent.SetDestination(beaconList.transform.GetChild(currentTarget).position);
        }

        void OnTriggerEnter(Collider collision) {
            if (collision.gameObject.tag == "Beacon" && (currentTarget+1 < beaconList.transform.GetChildCount())) {
                currentTarget++;
                Debug.Log("Moving to beacon " + currentTarget);
            }
        }
    }
}
