using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TDL {

	public class Projectile : MonoBehaviour {

		public GameObject target;
		public float distance, speed, killTimer, damage;
		public int turretLevel = 1;
		public float killDistance = 0.5f, areaEffectRange = 2.0f;
		//public TurretTestEnemy myEnemy;
		public Minion myEnemy;
		public Minion[] enemies;
		public enum TurretType{
			
			DPS,
			FIRE,
			ICE,
		}
		public TurretType turretType;

		// Use this for initialization
		void Start () 
		{
			
		}

		void ApplyDamage()
		{
			enemies = FindObjectsOfType<Minion> ();

			if(myEnemy != null)
			{
				switch(turretType)
				{
					default:
						myEnemy.Damage(damage, "DPS", turretLevel);
						break;
					case TurretType.DPS:
						myEnemy.Damage(damage, "DPS", turretLevel);
						break;
					case TurretType.FIRE:
						FindEnemiesInRangeOfFire();
						myEnemy.Damage(damage, "FIRE", turretLevel);
						break;
					case TurretType.ICE:
						FindEnemiesInRangeOfIce();
						myEnemy.Damage(damage, "ICE", turretLevel);
						break;
				}
			}

			KillThySelf();
			
		}

		public void FindEnemiesInRangeOfFire()
		{
			List<Minion> enemiesInRange = new List<Minion>();
			for(int i = 0; i < enemies.Length; i++)
			{
				float enemyDistance = Vector3.Distance (enemies[i].gameObject.transform.position, target.transform.position);
				if(enemyDistance <= areaEffectRange)
				{
					enemies[i].Damage(damage, "FIRE", turretLevel);
				}
			}
		}

		public void FindEnemiesInRangeOfIce()
		{
			List<Minion> enemiesInRange = new List<Minion>();
			for(int i = 0; i < enemies.Length; i++)
			{
				float enemyDistance = Vector3.Distance (enemies[i].gameObject.transform.position, target.transform.position);
				if(enemyDistance <= areaEffectRange)
				{
					enemies[i].Damage(damage, "ICE", turretLevel);
				}
			}
		}

		public void KillThySelf()
		{
			Destroy(gameObject);
		}
		
		// Update is called once per frame
		void Update () 
		{
			if(target != null)
			{
				transform.LookAt(target.transform);
				transform.Translate(Vector3.forward*speed*Time.deltaTime); 
				float dist = Vector3.Distance (transform.position, target.transform.position);
				if(dist <= killDistance)
				{
					ApplyDamage();
					//KillThySelf();
				}
			}
			else
			{
				KillThySelf();
			}

		}
	}
}
