using System;
using UnityEngine;
using System.Collections.Generic;

namespace TDL {
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMotion : MonoBehaviour {
        public GameObject beaconList;
        public float speed = 10.0f;
        public NavMeshAgent agent = null;
        public int currentTarget = 0;


        void Start() {
            agent = GetComponent<NavMeshAgent>();
            beaconList = GameObject.FindWithTag("BeaconList");
        }

        void Update() {
            agent.SetDestination(beaconList.transform.GetChild(currentTarget).position);
        }

        protected void OnTriggerEnter(Collider collision) {
            //Debug.Log("Agent handler");
            if (collision.gameObject.tag == "Beacon" && (currentTarget+1 < beaconList.transform.GetChildCount())) {
                currentTarget++;
                //Debug.Log("Moving to beacon " + currentTarget);
            }
        }
    }
}
