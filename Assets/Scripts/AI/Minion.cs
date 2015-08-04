using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//Basic minion with health that moves on navmesh
namespace TDL {
    public class Minion : AgentMotion {

        public int health = 100;

        void Awake() {
        }

        public int Health {
            get { return health; }
        }

        public int TakeDamage(int damage) {
            health = health - damage;

            if (health <= 0) {
                Die();
            }

            return health;
        }

        private void Die() {
            GameObject.Destroy(this.gameObject);
        }
        
        void OnTriggerEnter(Collider collision) {
            //Debug.Log("Minion handler");
            base.OnTriggerEnter(collision);
            if (collision.gameObject.tag == "Base") {
                Die();
            }
        }
         
    }
}
