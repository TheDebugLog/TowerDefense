using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public GameObject target;
	public float distance, speed, killTimer, damage;
	public int turretLevel = 1;
	public TurretTestEnemy myEnemy;
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
					myEnemy.Damage(damage, "FIRE", turretLevel);
					break;
				case TurretType.ICE:
					myEnemy.Damage(damage, "ICE", turretLevel);
					break;
			}
		}

		KillThySelf();
		
	}

	public void KillThySelf()
	{
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt(target.transform);
		transform.Translate(Vector3.forward*speed*Time.deltaTime); 
		float dist = Vector3.Distance (transform.position, target.transform.position);
		if(dist <= 0.3f)
		{
			ApplyDamage();
			//KillThySelf();
		}

	}
}
