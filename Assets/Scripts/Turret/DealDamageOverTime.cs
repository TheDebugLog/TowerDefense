using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TDL {

	[RequireComponent (typeof (Minion))]
	public class DealDamageOverTime : MonoBehaviour {

		public Minion myController;
		public int myTurretID = 0;
		// Use this for initialization
		void Start () 
		{
			myController = gameObject.GetComponent<Minion>();
		}

		public IEnumerator ApplyDOT(float damage, int duration)
	    {
	    	myController.DoDamage(damage);
	        duration--;
	        yield return new WaitForSeconds(1f);
	        if(duration > 0)
	        {
	            StartCoroutine(ApplyDOT(damage, duration));
	        }
	        else
	        {
	        	Destroy(this);
	        }
	    }
	}
}
