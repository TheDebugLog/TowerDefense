using UnityEngine;
using System.Collections;

namespace TDL {
	public class OccupiableSpace : MonoBehaviour {

		public bool isOccupied = false;
		public Material avalible, unavalible, standard;
		private bool turnedOnOnce = true, turnedOffOnce = false;
		GamePlaySceneController gamePlaySceneController;

		// Use this for initialization
		void Start () 
		{
			gamePlaySceneController = (GamePlaySceneController)FindObjectOfType(typeof(GamePlaySceneController));
		}

		void TurnOnDisplayStatus()
		{
			if(isOccupied == false)
			{
				Debug.Log("Here");
				GetComponent<Renderer>().material = avalible;
			}
			else
			{
				GetComponent<Renderer>().material = unavalible;
			}
		}

		void TurnOffDisplayStatus()
		{
			GetComponent<Renderer>().material = standard;
		}

		
		// Update is called once per frame
		void Update ()
		{
			if(gamePlaySceneController.addTurretMode == true)
			{
				if(turnedOnOnce == true)
				{
					turnedOnOnce = false;
					turnedOffOnce = true;
					TurnOnDisplayStatus();
				}
			}
			else
			{
				if(turnedOffOnce == true)
				{
					turnedOffOnce = false;
					turnedOnOnce = true;
					TurnOffDisplayStatus();
				}
			}
		}
	}
}
