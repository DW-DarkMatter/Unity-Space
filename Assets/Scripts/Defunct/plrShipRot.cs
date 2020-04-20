using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlrShipRot : MonoBehaviour {

	//Variables
	CharacterController SpaceShip;													//Reference for the controller of this object
	Vector3 v3RotThrust;
	Vector3 v3Rotation;															//Vector for applying rotation to this object

	float normalizedInputX, normalizedInputY, normalizedInputZ;						//Input minus controllerDeadzone
	float normalizedInputPercentX, normalizedInputPercentY, normalizedInputPercentZ;//Percentage of normalized Input

	//Game Settings
	public float xRotThrust = 0.5f, yRotThrust = 0.5f, zRotThrust = 0.5f;			//Thrust/acceleration for pitch/yaw/roll manueveres
	public float xRotSpeedCap = 10f, yRotSpeedCap = 10f, zRotSpeedCap = 10f;		//Thrust cap for pitch/yaw/roll manueveres
	float availableInputRange; 

	//Player Settings
	public float controllerDeadzone = 0.1f;											//Deadzone under which analogue input will be ignored
	public bool rotCounterThrustON = true;											//If enabled, provides an automatic counter-force to rotation when input is released



	void Start () {
		SpaceShip = this.GetComponent<CharacterController>();
		InitializeSettings ();
	}



	public void InitializeSettings () {
		controllerDeadzone = Mathf.Clamp (controllerDeadzone , 0, 0.99f);
		availableInputRange = 1 - controllerDeadzone;
	}



	void Update () {

//Controlling rotation around the X, Y, and Z axes:
//X-axis rotation (pitch up/down)
		if (Mathf.Abs (Input.GetAxis ("Pitch")) > controllerDeadzone) {
			v3Rotation.x = Mathf.Lerp (v3Rotation.x, xRotSpeedCap * Input.GetAxisRaw ("Pitch"), xRotThrust * Time.deltaTime);
		}


/*
		if (Mathf.Abs (Input.GetAxis ("Pitch")) > controllerDeadzone) {
			normalizedInputX = Mathf.Abs (Input.GetAxis ("Pitch") - controllerDeadzone);
			normalizedInputPercentX = (normalizedInputX/availableInputRange) * 100;

			v3RotThrust.x = (Input.GetAxisRaw ("Pitch") * normalizedInputPercentX * xRotThrust) / 100;
			//v3CounterThrust.x = 0;
		}
		//v3RotationX += v3RotThrustX; v3RotationX -= v3RotCThrust; transform.rotate (v3Rotation);

*/

//Arrest rotation around the X-axis 
		else if (rotCounterThrustON == true || Input.GetButton ("AllStop")) {
			v3Rotation.x = Mathf.Lerp (v3Rotation.x, 0, xRotThrust * Time.deltaTime);
		}
		//v3CounterThrust.y = xRotThrust;


//Y-axis rotation (yaw right/left)
		if (Mathf.Abs (Input.GetAxis ("Yaw")) > controllerDeadzone) {
			v3Rotation.y = Mathf.Lerp (v3Rotation.y, yRotSpeedCap * Input.GetAxisRaw ("Yaw"), yRotThrust * Time.deltaTime);
		}
//Arrest rotation around the Y-axis 
		else if (rotCounterThrustON == true || Input.GetButton ("AllStop")) {
			v3Rotation.y = Mathf.Lerp (v3Rotation.y, 0, yRotThrust * Time.deltaTime);
		}

//Z-axis rotation (roll right/left)
		if (Mathf.Abs (Input.GetAxis ("Roll")) > controllerDeadzone) {
			v3Rotation.z = Mathf.Lerp (v3Rotation.z, zRotSpeedCap * Input.GetAxisRaw ("Roll"), zRotThrust * Time.deltaTime);
		}
//Arrest rotation around the Z-axis
		else if (rotCounterThrustON == true || Input.GetButton ("AllStop")) {
			v3Rotation.z = Mathf.Lerp (v3Rotation.z, 0, zRotThrust * Time.deltaTime);
		}

//Apply rotation to this object
		transform.Rotate (v3Rotation);
	}


//fin
}