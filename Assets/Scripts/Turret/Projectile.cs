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
		public int turretID = 0;
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
					case TurretType.DPS:
						myEnemy.DoDamage(damage);
						break;
					case TurretType.FIRE:
						FindEnemiesInRangeOfFire();
						break;
					case TurretType.ICE:
						FindEnemiesInRangeOfIce();
						//myEnemy.Damage(damage, "ICE", turretLevel);
						break;
				}
			}

			KillThySelf();
			
		}

		DealDamageOverTime GetDDOT(GameObject enemyGameObject)
		{
			DealDamageOverTime ddot = enemyGameObject.GetComponent<DealDamageOverTime>();
			if(ddot == null)
			{
			 	ddot = enemyGameObject.AddComponent<DealDamageOverTime>() as DealDamageOverTime;
			 	ddot.myTurretID = turretID;
			 	return ddot;
			}
			else
			{
				if(ddot.myTurretID == turretID)
				{
					//ddot.ApplyDOT(damage, turretLevel);
					return ddot;
				}
				else
				{
					ddot = enemyGameObject.AddComponent<DealDamageOverTime>() as DealDamageOverTime;
			 		ddot.myTurretID = turretID;
			 		return ddot;
				}
			}
		}

		DealSlow GetDS(GameObject enemyGameObject)
		{
			DealSlow ds = enemyGameObject.GetComponent<DealSlow>();
			if(ds == null)
			{
			 	ds = enemyGameObject.AddComponent<DealSlow>() as DealSlow;
			 	ds.myTurretID = turretID;
			 	return ds;
			}
			else
			{
				if(ds.myTurretID == turretID)
				{
					//ddas.ApplyDOT(damage, turretLevel);
					return ds;
				}
				else
				{
					ds = enemyGameObject.AddComponent<DealSlow>() as DealSlow;
			 		ds.myTurretID = turretID;
			 		return ds;
				}
			}
		}

		public void FindEnemiesInRangeOfFire()
		{
			for(int i = 0; i < enemies.Length; i++)
			{
				float enemyDistance = Vector3.Distance (enemies[i].gameObject.transform.position, target.transform.position);
				if(enemyDistance <= areaEffectRange)
				{
					DealDamageOverTime ddot = GetDDOT(enemies[i].gameObject);
					ddot.myController = enemies[i];
					StartCoroutine(ddot.ApplyDOT(damage, turretLevel));
				}
			}
		}

		public void FindEnemiesInRangeOfIce()
		{
			for(int i = 0; i < enemies.Length; i++)
			{
				float enemyDistance = Vector3.Distance (enemies[i].gameObject.transform.position, target.transform.position);
				if(enemyDistance <= areaEffectRange)
				{
					DealDamageOverTime ddot = GetDDOT(enemies[i].gameObject);
					ddot.myController = enemies[i];
					StartCoroutine(ddot.ApplyDOT(damage, turretLevel));
					DealSlow ds = GetDS(enemies[i].gameObject);
					ds.myController = enemies[i];
					StartCoroutine(ds.ApplySlow(damage, turretLevel));
					//enemies[i].Damage(damage, "ICE", turretLevel);
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
