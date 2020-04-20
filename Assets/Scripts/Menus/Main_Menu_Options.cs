using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Menu_Options : MonoBehaviour {

	//Game Settings
		//useFuelON
		public bool useFuelON;


	//Player Settings
		//motCounterThrustON
		//rotCounterThrustON
		//controllerDeadzone
		//controllerDeadzone = Mathf.Clamp (controllerDeadzone , 0, 0.99f);
	public bool motCounterThrustON = true, rotCounterThrustON = true;
	public float countrollerDeadzone;

	public void FuelUsage (bool useFuelON) {
		Debug.Log ("Fuel Usage " +useFuelON);
	}


}