using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TDL {

	[RequireComponent (typeof (AudioSource))]
	public class BaseTurret : MonoBehaviour {

		//This class will control the turrets
		public int simulatedUpgradeLevel = 1;
		public string simulatedTurretType = "DPS";
		public float damage = 10.0f;
		public float range = 20.0f, distanceToTarget = 0.0f;
		public float fireTimer = 0.0f;
		public enum FireRate{
			
			HalfSecond,
			ThreeQuarterSecond,
			OneSecond,
			OneAndQuarterSecond,
			OneAndHalfSecond,
		}
		public enum UpgradeLevel{
			
			LevelOne,
			LevelTwo,
			LevelThree,
			LevelFour,
			LevelFive,
		}
		public enum TurretType{
			
			DPS,
			FIRE,
			ICE,
		}
		public FireRate fireRate;
		public UpgradeLevel upgradeLevel;
		public TurretType turretType;
		public GameObject target = null;
		public GameObject levelOneComponent, levelTwoComponent, levelThreeComponent,
		levelFourComponent, levelFiveComponent;
		public GameObject horizontalRotComponent, verticalRotComponent, turnCompForLerp;
		public Transform barrelPosition;
		public Animation animComponent;
		public GameObject projectilePrefab;
		//public TurretTestEnemy[] enemies;
		public Minion[] enemies;
		//public List<Minion> enemiesFarthestAlong = new List<Minion>();
		public AnimationClip halfSecond, threeQuarterSecond, oneSecond, oneAndQuarterSecond, oneAndHalfSecond, idle;
		public AudioClip shootSound;

		private AudioSource soundSource;
		private int myLevel = 1;
		private Quaternion _startRotation, _endRotation;
		private float _rotMultiplier = 0.0f, _initialAngleDiffference = 0.0f;
		private Vector3 _currentAngle;
		private Minion _closestEnemyToTarget, _checkClosestEnemy;
		private bool _targetActive = false, _canFire = true, _runFireRateTimer = false, _isIdle = true,
		_rotatingToTarget = false, _lockedOnTarget = false;
		// Use this for initialization
		void Start () 
		{
			soundSource = GetComponent<AudioSource> ();
			if(simulatedUpgradeLevel != 1)
			{
				UpgradeTurretLevel(simulatedUpgradeLevel);
				UpgradeTurretType(simulatedTurretType);
			}
		}

		public void UpgradeTurretLevel(int targetLevel)
		{
			switch(targetLevel)
			{
				default:
					upgradeLevel = UpgradeLevel.LevelOne;
					fireRate = FireRate.OneAndHalfSecond;
					myLevel = 1;
					levelOneComponent.SetActive(true);
					levelTwoComponent.SetActive(false);
					levelThreeComponent.SetActive(false);
					levelFourComponent.SetActive(false);
					levelFiveComponent.SetActive(false);
					damage = 10.0f;
					break;
				case 1:
					upgradeLevel = UpgradeLevel.LevelOne;
					fireRate = FireRate.OneAndHalfSecond;
					myLevel = 1;
					levelOneComponent.SetActive(true);
					levelTwoComponent.SetActive(false);
					levelThreeComponent.SetActive(false);
					levelFourComponent.SetActive(false);
					levelFiveComponent.SetActive(false);
					damage = 10.0f;
					break;
				case 2:
					upgradeLevel = UpgradeLevel.LevelTwo;
					fireRate = FireRate.OneAndQuarterSecond;
					myLevel = 2;
					levelOneComponent.SetActive(false);
					levelTwoComponent.SetActive(true);
					levelThreeComponent.SetActive(false);
					levelFourComponent.SetActive(false);
					levelFiveComponent.SetActive(false);
					damage = 20.0f;
					break;
				case 3:
					upgradeLevel = UpgradeLevel.LevelThree;
					fireRate = FireRate.OneSecond;
					myLevel = 3;
					levelOneComponent.SetActive(false);
					levelTwoComponent.SetActive(false);
					levelThreeComponent.SetActive(true);
					levelFourComponent.SetActive(false);
					levelFiveComponent.SetActive(false);
					damage = 30.0f;
					break;
				case 4:
					upgradeLevel = UpgradeLevel.LevelFour;
					fireRate = FireRate.ThreeQuarterSecond;
					myLevel = 4;
					levelOneComponent.SetActive(false);
					levelTwoComponent.SetActive(false);
					levelThreeComponent.SetActive(false);
					levelFourComponent.SetActive(true);
					levelFiveComponent.SetActive(false);
					damage = 40.0f;
					break;
				case 5:
					upgradeLevel = UpgradeLevel.LevelFive;
					fireRate = FireRate.HalfSecond;
					myLevel = 5;
					levelOneComponent.SetActive(false);
					levelTwoComponent.SetActive(false);
					levelThreeComponent.SetActive(false);
					levelFourComponent.SetActive(false);
					levelFiveComponent.SetActive(true);
					damage = 50.0f;
					break;
			}
		}

		public void UpgradeTurretType(string targetTurretType)
		{
			switch(targetTurretType)
			{
				default:
					turretType = TurretType.DPS;
					break;
				case "DPS":
					turretType = TurretType.DPS;
					break;
				case "FIRE":
					turretType = TurretType.FIRE;
					break;
				case "ICE":
					turretType = TurretType.ICE;
					break;
			}
		}

		void AcquireTarget()
		{
			//Debug.Log ("Called AcquireTarget");
			//----If registar Enemies are in a session object then loop through it not this----//
			//enemies = FindObjectsOfType<TurretTestEnemy> ();
			enemies = FindObjectsOfType<Minion> ();
			_closestEnemyToTarget = null;

			if(enemies != null)
			{
				//int numEnemiesInRange = 0;
				//int nextEnemyIndex = 0;
				Vector3 farthestEnemyInRange = new Vector3(0f,0f,0f);
				float closestEnemyDistance = Mathf.Infinity;

				List<Minion> enemiesFarthestAlong = new List<Minion>();
				int highestChildIndex = 0;

				//Finds the highest child index in the enemies
				for(int i = 0; i < enemies.Length; i++)
				{
					if(enemies[i].isAlive == false)
					{
						continue;
					}

					if(enemies[i].currentTarget > highestChildIndex)
					{
						highestChildIndex = enemies[i].currentTarget;
					}
					else
					{
						continue;
					}
				}

				//builds the enemy List with enemy's whos target is the highestChildIndex
				for(int j = 0; j < enemies.Length; j++)
				{
					if(enemies[j].currentTarget == highestChildIndex)
					{
						enemiesFarthestAlong.Add(enemies[j]);
					}
				}

				//Looks for the enemy that is closest to the farthest waypoint
				for(int p = 0; p < enemiesFarthestAlong.Count; p++)
				{
					if(enemiesFarthestAlong[p] != null && enemiesFarthestAlong[p].beaconList != null)
					{
						float distanceToTarget = Vector3.Distance(enemiesFarthestAlong[p].gameObject.transform.position, enemiesFarthestAlong[p].beaconList.transform.GetChild(highestChildIndex).position);
						if(distanceToTarget <= closestEnemyDistance)
						{
							closestEnemyDistance = distanceToTarget;
							_closestEnemyToTarget = enemiesFarthestAlong[p];
						}
					}
				}
				//For Debug
				//distanceToTarget = Vector3.Distance(enemies[0].gameObject.transform.position, gameObject.transform.position);

				/*for(int index = 0; index < enemies.Length; index++)
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
						}
						else if(enemies[index].gameObject.transform.position.z == farthestEnemyInRange.z)
						{
							if(enemies[index].gameObject.transform.position.x > farthestEnemyInRange.x)
							{
								farthestEnemyInRange = enemies[index].gameObject.transform.position;
								nextEnemyIndex = index;
							}
						}

					}
					else 
					{
						continue;
					}
				}*/

				//if(numEnemiesInRange > 0)
				if(_closestEnemyToTarget != null)
				{
					//target = enemies[nextEnemyIndex].gameObject;
					target = _closestEnemyToTarget.gameObject;
					//iTween.Stop();
					animComponent.Stop ();
					_targetActive = true;
				}
				else
				{
					target = null;
					_targetActive = false;
					_lockedOnTarget = false;
					StartCoroutine( PlayIdle());
				}
			}
			else
			{
				//Debug.Log ("No Enemies");
				target = null;
				_targetActive = false;
				_lockedOnTarget = false;
				StartCoroutine( PlayIdle());
			}

			//--------------------------------------------------------------------------------//
		}

		public void CheckTargetsProgress()
		{
			enemies = FindObjectsOfType<Minion> ();
			_checkClosestEnemy = null;
			if(enemies != null)
			{
				Vector3 farthestEnemyInRange = new Vector3(0f,0f,0f);
				float closestEnemyDistance = Mathf.Infinity;

				List<Minion> enemiesFarthestAlong = new List<Minion>();
				int highestChildIndex = 0;

				//Finds the highest child index in the enemies
				for(int i = 0; i < enemies.Length; i++)
				{
					if(enemies[i].isAlive == false)
					{
						continue;
					}

					if(enemies[i].currentTarget > highestChildIndex)
					{
						highestChildIndex = enemies[i].currentTarget;
					}
					else
					{
						continue;
					}
				}

				//builds the enemy List with enemy's whos target is the highestChildIndex
				for(int j = 0; j < enemies.Length; j++)
				{
					if(enemies[j].currentTarget == highestChildIndex)
					{
						enemiesFarthestAlong.Add(enemies[j]);
					}
				}

				//Looks for the enemy that is closest to the farthest waypoint
				for(int p = 0; p < enemiesFarthestAlong.Count; p++)
				{
					float distanceToTarget = Vector3.Distance(enemiesFarthestAlong[p].gameObject.transform.position, enemiesFarthestAlong[p].beaconList.transform.GetChild(highestChildIndex).position);
					if(distanceToTarget <= closestEnemyDistance)
					{
						closestEnemyDistance = distanceToTarget;
						_checkClosestEnemy = enemiesFarthestAlong[p];
					}
				}

				//if(numEnemiesInRange > 0)
				if(_checkClosestEnemy != null)
				{
					if(_checkClosestEnemy != _closestEnemyToTarget)
					{
						target = _checkClosestEnemy.gameObject;
					}
					else
					{
						return;
					}
					
				}
				else
				{
					return;
				}
			}
		}

		IEnumerator PlayIdle()
		{
			if(_isIdle)
			{
				_isIdle = false;
				iTween.RotateTo(horizontalRotComponent, iTween.Hash("rotation", new Vector3(0.0f, 0.0f, 0.0f), "isLocal", true, "easeType", "linear", "time", 1f));
				yield return new WaitForSeconds(1f);
				animComponent.clip = idle;
				animComponent.Play ();
			}
		}

		IEnumerator RotateToTarget()
		{
			//CheckTargetsProgress();
			Vector3 targetDirectionY = target.transform.position - transform.position;
			//targetDirectionY = new Vector3 (0.0f, targetDirectionY.y, 0.0f);
			Vector3 forwardY = horizontalRotComponent.transform.forward;
			//forwardY = new Vector3 (0.0f, forwardY.y, 0.0f);
			float angleY = Vector3.Angle(targetDirectionY, forwardY);
			float eularRotationY = horizontalRotComponent.transform.localRotation.eulerAngles.y;
			Vector3 relativePoint = barrelPosition.transform.InverseTransformPoint(target.transform.position); 
			if (relativePoint.x < 0.0f) 
			{
				Debug.Log("Object is to the left");
				eularRotationY -= angleY;
			} 
			else if(relativePoint.x > 0.0) 
			{
				Debug.Log("Object is to the right");
				eularRotationY += angleY;
			}
			
			Vector3 VerticalEnemyTarget = new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z);
			turnCompForLerp.transform.LookAt (VerticalEnemyTarget);
			//Debug.Log ("--------------------------------- OldY = " + horizontalRotComponent.transform.localRotation.eulerAngles.y);
			//Debug.Log ("--------------------------------- NewY = " + eularRotationY);
			//Debug.Log ("--------------------------------- AngleY = " + angleY);
			//Debug.Log ("--------------------------------- VreticalComp EularX = " + turnCompForLerp.transform.localRotation.eulerAngles.x);
			Vector3 targetRotationY = new Vector3 (horizontalRotComponent.transform.localRotation.eulerAngles.x, eularRotationY, horizontalRotComponent.transform.localRotation.eulerAngles.z);
			Vector3 targetRotationX = new Vector3 (turnCompForLerp.transform.localRotation.eulerAngles.x, 0.0f, verticalRotComponent.transform.localRotation.eulerAngles.z);
			iTween.RotateTo(horizontalRotComponent, iTween.Hash("rotation", targetRotationY, "easeType", "linear", "time", 0.5f));
			iTween.RotateTo(verticalRotComponent, iTween.Hash("rotation", targetRotationX, "isLocal", true, "easeType", "linear", "time", 0.5f));
			yield return new WaitForSeconds (0.5f);
			_lockedOnTarget = true;
			_rotatingToTarget = false;
		}

		void TrackTarget()
		{

			//Debug.Log ("Called TrackTarget");
			//---------------------------------------Rotates The Turret--------------------------------------------------------------------//
			//----Rotating the turret horizontally----//
			Vector3 HorizontalEnemyTarget = new Vector3 (target.transform.position.x, transform.position.y, target.transform.position.z);
			horizontalRotComponent.transform.LookAt (HorizontalEnemyTarget);
			//----------------------------------------//
			//----Rotating the turret vertically----//
			Vector3 VerticalEnemyTarget = new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z);
			verticalRotComponent.transform.LookAt (VerticalEnemyTarget);
			//--------------------------------------//
			//----------------------------------------------------------------------------------------------------------------------------//
		}

		void Fire()
		{

			if(_canFire == true)
			{
				_canFire = false;
				//Debug.Log ("Called Fire");
				switch(fireRate)
				{
					case FireRate.HalfSecond:
						fireTimer = 0.5f;
						animComponent.clip = halfSecond;
						animComponent.Play ();
						break;
					case FireRate.ThreeQuarterSecond:
						fireTimer = 0.75f;
						animComponent.clip = threeQuarterSecond;
						animComponent.Play ();
						break;
					case FireRate.OneSecond:
						fireTimer = 1f;
						animComponent.clip = oneSecond;
						animComponent.Play ();
						break;
					case FireRate.OneAndQuarterSecond:
						fireTimer = 1.25f;
						animComponent.clip = oneAndQuarterSecond;
						animComponent.Play ();
						break;
					case FireRate.OneAndHalfSecond:
						fireTimer = 1.5f;
						animComponent.clip = oneAndHalfSecond;
						animComponent.Play ();
						break;
				}
				_runFireRateTimer = true;
				StartCoroutine(AnimationStopper(animComponent.clip));
				if(shootSound != null)
				{
					soundSource.PlayOneShot(shootSound);
				}
				CreateProjectile();
			}
		}

		public void CreateProjectile()
		{
			if(target != null)
			{
				//TurretTestEnemy myEnemy = target.GetComponent<TurretTestEnemy> ();
				Minion myEnemy = target.GetComponent<Minion> ();
				GameObject bullet = Instantiate(projectilePrefab, barrelPosition.position, barrelPosition.rotation) as GameObject;
				Projectile bulletScript = bullet.GetComponent<Projectile> ();
				bulletScript.target = target;
				bulletScript.myEnemy = myEnemy;
				bulletScript.turretLevel = myLevel;
				bulletScript.killTimer = fireTimer;
				bulletScript.damage = damage;
				bulletScript.speed = 60f;

				switch(turretType)
				{
					default:
						bulletScript.turretType = Projectile.TurretType.DPS;
						break;
					case TurretType.DPS:
						bulletScript.turretType = Projectile.TurretType.DPS;
						break;
					case TurretType.FIRE:
						bulletScript.turretType = Projectile.TurretType.FIRE;
						break;
					case TurretType.ICE:
						bulletScript.turretType = Projectile.TurretType.ICE;
						break;
				}
			}
		}

		IEnumerator AnimationStopper(AnimationClip fireClip)
		{
			//Stops fire sequence from looping
			//Set to loop so crossfade will work
			yield return new WaitForSeconds (fireClip.length - 0.01f);
			animComponent.Stop();
			_isIdle = true;
		}

		// Update is called once per frame
		void Update ()
		{
			if(_runFireRateTimer)
			{
				if(fireTimer > 0.0f)
				{
					fireTimer -= Time.deltaTime;
				}
				else
				{
					_canFire = true;
					_runFireRateTimer = false;
				}
			}


			if (target == null) 
			{
				AcquireTarget ();
			}
			else
			{
				distanceToTarget = Vector3.Distance(target.transform.position, gameObject.transform.position);
				if(distanceToTarget <= range)
				{
					if(_lockedOnTarget)
					{
						TrackTarget();
						Fire ();
					}
					else
					{
						if(_rotatingToTarget == false)
						{
							_rotatingToTarget = true;
							StartCoroutine(RotateToTarget());
						}
					}

				}
				else
				{
					target = null;
					_targetActive = false;
				}

			}

		}
	}
}
