using UnityEngine;
using System.Collections;

public class TurretTestEnemy : MonoBehaviour {

	//This class starts an agent on a path and once close to the destination 
	//it will reset to its start position and start again
	public NavMeshAgent navAgent;
	public float distance = 0.0f, restartDistance = 2.0f, health = 200.0f, speed = 20.0f;
	public Transform destination, startPoint;
	public bool isAlive = true;
	//private int numberOfTicks = 0;

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
		yield return new WaitForSeconds(0.5f);
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
		navAgent.speed = (speed / 2);

		if(health <= 0.0f)
		{
			Die();
		}

		duration--;
		yield return new WaitForSeconds(0.5f);
		if(duration > 0)
		{
			StartCoroutine(ApplyDOT(damage, duration));
		}
		else
		{
			if(health > 0.0f)
			{
				navAgent.speed = speed;
			}
		}
	}

	public void Die()
	{
		isAlive = false;
		Destroy(gameObject);
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
