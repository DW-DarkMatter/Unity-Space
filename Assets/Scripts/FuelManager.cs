using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelManager : MonoBehaviour {



	//Variables
	PlayerSettings plrSettings;
	PlrShipMotV2 plrShipMot;
	PlrShipRot plrShipRot;
	bool _motCounterThrustON;
	bool _rotCounterThrustON;
	float _controllerDeadzone;
	float fuel;

	//Game Settings
	public float standardFuelUnit = 0.5f;

	//Player Settings
	public float maxFuel = 1000;


	void Awake () {
		fuel = maxFuel;
	}



	void Start () {
		plrSettings = this.GetComponent <PlayerSettings>();
		InitializeSettings ();
	}



	void InitializeSettings () {
		_motCounterThrustON = plrSettings.motCounterThrustON;
		_rotCounterThrustON = plrSettings.rotCounterThrustON;
		_controllerDeadzone = plrSettings.controllerDeadzone;
	}
	


	void Update () {
		if (fuel > 0) {
			if (Mathf.Abs (Input.GetAxis ("MotionX")) > _controllerDeadzone) {
			}
		}

		//
		fuel = Mathf.Clamp (fuel, 0, maxFuel);
	}


// fin 
}