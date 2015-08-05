using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//Basic minion with health that moves on navmesh
namespace TDL {
    public class Minion : AgentMotion {

        public bool isAlive = true;
        public float health = 100;

        void Awake() {
        }

        public float Health {
            get { return health; }
        }

        /*public int TakeDamage(int damage) {
            health = health - damage;

            if (health <= 0) {
                Die();
            }

            return health;
        }

        private void Die() {
            GameObject.Destroy(this.gameObject);
        }*/

        public void Damage(float damage, string damageType, int damageDuration)
        {
            switch(damageType)
            {
                case "DPS":
                    health -= damage;
                    break;
                case "FIRE":
                    StartCoroutine(ApplyDOT(damage, damageDuration));
                    break;
                case "ICE":
                    StartCoroutine(ApplySlow(damage, damageDuration));
                    break;
            }
            //health -= damage;
            if(health <= 0.0f)
            {
                Die();
            }
        }

        IEnumerator ApplyDOT(float damage, int duration)
        {
            //TODO: Apply area effect
            Debug.Log("Fire Hit");
            health -= damage;

            if(health <= 0.0f)
            {
                Die();
            }

            duration--;
            yield return new WaitForSeconds(1f);
            if(duration > 0)
            {
                StartCoroutine(ApplyDOT(damage, duration));
            }
        }

        IEnumerator ApplySlow(float damage, int duration)
        {
            //TODO: Apply area effect
            //These are temp numbers for testing, pre balancing
            health -= (damage / 2);
            agent.speed = (speed / 2);

            if(health <= 0.0f)
            {
                Die();
            }

            duration--;
            yield return new WaitForSeconds(1.5f);
            if(duration > 0)
            {
                StartCoroutine(ApplySlow(damage, duration));
            }
            else
            {
                if(health > 0.0f)
                {
                    agent.speed = speed;
                }
            }
        }

        public void Die()
        {
            isAlive = false;
            Destroy(gameObject);
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
