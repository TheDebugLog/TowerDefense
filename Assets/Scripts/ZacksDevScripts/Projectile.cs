using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public GameObject target;
	public bool launch = false;
	public float distance, speed;
	// Use this for initialization
	void Start () 
	{
	
	}

	//public IEnumerator KillThySelf(float deathTime)
	//{
		//yield return new WaitForSeconds (deathTime);
		//Destroy(gameObject);
	//}

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
		//if(launch)
		//{
			//launch = false;
			//iTween.MoveTo(gameObject, iTween.Hash("position", target.transform.position, "easeType", "linear", "time", 0.4f));
			//StartCoroutine(KillThySelf(0.4f));
		//}
		if(dist <= 0.3f)
		{
			KillThySelf();
		}

	}
}
