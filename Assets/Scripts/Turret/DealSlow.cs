using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TDL {

	[RequireComponent (typeof (Minion))]
	public class DealSlow : MonoBehaviour {

		public Minion myController;
		public int myTurretID = 0;
		private float _speed = 10.0f, _health = 100.0f;
		// Use this for initialization
		void Start () 
		{
			myController = gameObject.GetComponent<Minion>();
			//_speed = _myController.Speed;
			_speed = 10.0f;
		}
		
		public IEnumerator ApplySlow(float damage, int duration)
        {
	    	//myController.DoDamage(damage);
	    	//myController.Slow(_speed / 2);
	    	myController.Slow(5f);
            duration--;
            yield return new WaitForSeconds(1.5f);
            if(duration > 0)
            {
                StartCoroutine(ApplySlow(damage, duration));
            }
            else
            {
            	_health = myController.Health;
                if(_health > 0.0f)
                {
                    //myController.Slow(_speed);
                    myController.Slow(10f);
                    Destroy(this);
                }
            }
        }
	}
}
