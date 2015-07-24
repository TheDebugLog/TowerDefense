using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class BaseTurret : MonoBehaviour {

	//This class will control the turrets
	public float damage = 10.0f;
	public float range = 20.0f, distanceToTarget = 0.0f, initialTurnSpeed = 0.3f;
	public float fireTimer = 0.0f;
	public enum FireRate{
		
		HalfSecond,
		ThreeQuarterSecond,
		OneSecond,
		OneAndQuarterSecond,
		OneAndHalfSecond,
	}
	public FireRate fireRate;
	public GameObject target = null;
	public bool targetActive = false, canFire = true, runFireRateTimer = false, isIdle = true, rotBack = true, initialTurn = false, startFireSequence = false;
	public GameObject horizontalRotComponent, verticalRotComponent, turnCompForLerp;
	public Transform barrelPosition;
	public Animation animComponent, testAnimComponent;
	public GameObject projectilePrefab;
	public TurretTestEnemy[] enemies;
	public AnimationClip halfSecond, threeQuarterSecond, oneSecond, oneAndQuarterSecond, oneAndHalfSecond, idle;
	public AudioClip shootSound;

	private AudioSource soundSource;
	// Use this for initialization
	void Start () 
	{
		soundSource = GetComponent<AudioSource> ();
	}

	void AcquireTarget()
	{
		Debug.Log ("Called AcquireTarget");
		//----If registar Enemies are in a session object then loop through it not this----//
		enemies = FindObjectsOfType<TurretTestEnemy> ();
		if(enemies != null)
		{
			int numEnemiesInRange = 0;
			int nextEnemyIndex = 0;
			Vector3 farthestEnemyInRange = new Vector3(0f,0f,0f);
			float closestEnemyDistance = Mathf.Infinity;
			//For Debug
			distanceToTarget = Vector3.Distance(enemies[0].gameObject.transform.position, gameObject.transform.position);

			for(int index = 0; index < enemies.Length; index++)
			{
				if(enemies[index].isAlive == false)
				{
					continue;
				}
				float dist = Vector3.Distance(enemies[index].gameObject.transform.position, gameObject.transform.position);
				//if(dist <= range && dist <= closestEnemyDistance)
				if(dist <= range)
				{
					numEnemiesInRange++;
					if(enemies[index].gameObject.transform.position.z > farthestEnemyInRange.z)
					{
						farthestEnemyInRange = enemies[index].gameObject.transform.position;
						nextEnemyIndex = index;
						//closestEnemyDistance = dist;
					}
					else if(enemies[index].gameObject.transform.position.z == farthestEnemyInRange.z)
					{
						if(enemies[index].gameObject.transform.position.x > farthestEnemyInRange.x)
						{
							farthestEnemyInRange = enemies[index].gameObject.transform.position;
							nextEnemyIndex = index;
							//closestEnemyDistance = dist;
						}
					}

				}
				else 
				{
					continue;
				}
			}
			if(numEnemiesInRange > 0)
			{
				target = enemies[nextEnemyIndex].gameObject;
				iTween.Stop();
				targetActive = true;
				isIdle = true;
			}
			else
			{
				target = null;
				targetActive = false;
				//TODO: Call Idle Animation Here
				PlayIdle();
			}
		}
		else
		{
			Debug.Log ("No Enemies");
			target = null;
			targetActive = false;
			//TODO: Call Idle Animation Here
			PlayIdle();
		}

		//--------------------------------------------------------------------------------//
	}

	void PlayIdle()
	{
		if(isIdle)
		{
			isIdle = false;
			if(rotBack)
			{
				iTween.RotateTo(horizontalRotComponent.gameObject, iTween.Hash("rotation", new Vector3(0.0f, 0.0f, 0.0f), "isLocal", true, "easeType", "linear", "time", 2.0f));
			}
			else
			{
				iTween.RotateTo(horizontalRotComponent.gameObject, iTween.Hash("rotation", new Vector3(0.0f, 90.0f, 0.0f), "isLocal", true, "easeType", "linear", "time", 2.0f));
			}
		}
	}

	void RotateToTarget()
	{

		Debug.Log ("Called RotateToTarget");
		//----Rotating the turret horizontally----//
		Vector3 HorizontalEnemyTarget = new Vector3 (target.transform.position.x, transform.position.y, target.transform.position.z);
		horizontalRotComponent.transform.LookAt(HorizontalEnemyTarget);
		//----------------------------------------//
		//----Rotating the turret vertically----//
		Vector3 VerticalEnemyTarget = new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z);
		verticalRotComponent.transform.LookAt(VerticalEnemyTarget);
		//--------------------------------------//

	}

	void Fire()
	{

		if(canFire == true)
		{
			canFire = false;
			Debug.Log ("Called Fire");
			switch(fireRate)
			{
				case FireRate.HalfSecond:
					fireTimer = 0.5f;
					animComponent.clip = halfSecond;
					animComponent.Play();
					break;
				case FireRate.ThreeQuarterSecond:
					fireTimer = 0.75f;
					animComponent.clip = halfSecond;
					animComponent.Play();
					break;
				case FireRate.OneSecond:
					fireTimer = 1f;
					animComponent.clip = halfSecond;
					animComponent.Play();
					break;
				case FireRate.OneAndQuarterSecond:
					fireTimer = 1.25f;
					animComponent.clip = halfSecond;
					animComponent.Play();
					break;
				case FireRate.OneAndHalfSecond:
					fireTimer = 1.5f;
					animComponent.clip = halfSecond;
					animComponent.Play();
					break;
			}
			runFireRateTimer = true;
			if(shootSound != null)
			{
				soundSource.PlayOneShot(shootSound);
			}
			StartCoroutine( CreateProjectile());
		}
	}

	public IEnumerator CreateProjectile()
	{
		if(target != null)
		{
			TurretTestEnemy myEnemy = target.GetComponent<TurretTestEnemy> ();
			GameObject bullet = Instantiate(projectilePrefab, barrelPosition.position, barrelPosition.rotation) as GameObject;
			Projectile bulletScript = bullet.GetComponent<Projectile> ();
			bulletScript.target = target;
			bulletScript.launch = true;
			bulletScript.speed = 60f;
			yield return new WaitForSeconds (fireTimer);
			myEnemy.Damage (damage, "default");
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if(runFireRateTimer)
		{
			if(fireTimer > 0.0f)
			{
				fireTimer -= Time.deltaTime;
			}
			else
			{
				canFire = true;
				runFireRateTimer = false;
			}
		}

		if(initialTurn == true)
		{
			//horizontalRotComponent.transform.localRotation = Quaternion.Lerp(horizontalRotComponent.transform.localRotation, turnCompForLerp.transform.localRotation, Time.time * initialTurnSpeed);

			if(Mathf.Abs(horizontalRotComponent.transform.localRotation.y - turnCompForLerp.transform.localRotation.y) <= 0.1f)
			{

			}
		}

		if (target == null) 
		{
			AcquireTarget ();
			Debug.Log(horizontalRotComponent.transform.localRotation.y);
			if(horizontalRotComponent.transform.localRotation.y == 0.0f && rotBack == true)
			{
				rotBack = false;
				isIdle = true;
			}
			if(horizontalRotComponent.transform.localRotation.y > 0.69f && rotBack == false)
			{
				rotBack = true;
				isIdle = true;
			}
		}
		else
		{
			distanceToTarget = Vector3.Distance(target.transform.position, gameObject.transform.position);
			if(distanceToTarget <= range)
			{
				RotateToTarget();
				Fire ();
			}
			else
			{
				target = null;
				targetActive = false;
			}

		}

	}
}
