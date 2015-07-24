using UnityEngine;
using System.Collections;

public class TurretTestEnemy : MonoBehaviour {

	//This class starts an agent on a path and once close to the destination 
	//it will reset to its start position and start again
	public NavMeshAgent navAgent;
	public float distance = 0.0f, restartDistance = 2.0f, health = 200.0f;
	public Transform destination, startPoint;
	public bool isAlive = true;

	// Use this for initialization
	void Start () 
	{
		if (navAgent != null && destination != null) 
		{
			navAgent.SetDestination(destination.position);
		}
	}

	float FindDistance()
	{
		distance = Vector3.Distance(destination.position, gameObject.transform.position);
		return distance;
	}

	public void Damage(float damage, string damageType)
	{
		switch(damageType)
		{
			case "default":
				health -= damage;
				break;
		}
		//health -= damage;
		if(health <= 0.0f)
		{
			Die();
		}
	}

	public void Die()
	{

	}

	// Update is called once per frame
	void Update () 
	{
		FindDistance ();
		if(distance < restartDistance)
		{
			navAgent.enabled = false;
			gameObject.transform.position = startPoint.position;
			navAgent.enabled = true;
			navAgent.SetDestination(destination.position);
		}
	}
}
