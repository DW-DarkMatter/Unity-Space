using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlrShipRotV2 : MonoBehaviour {

	//Variables
	CharacterController SpaceShip;												//Reference for the controller of this object
	Vector3 v3RotThrust, v3RotCounterThrust;
	Vector3 v3Rotation;															//Vector for applying rotation to this object
	Vector3 _v3RotDir;

	float normalizedInputX, normalizedInputY, normalizedInputZ;						//Input minus controllerDeadzone
	float normalizedInputPercentX, normalizedInputPercentY, normalizedInputPercentZ;//Percentage of normalized Input

	//Game Settings
	public float xRotThrust = 0.5f, yRotThrust = 0.5f, zRotThrust = 0.5f;			//Thrust/acceleration for pitch/yaw/roll manueveres
	public float xRotCounterThrust = 0.5f, yRotCounterThrust = 0.5f, zRotCounterThrust = 0.5f;
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

		_v3RotDir.x = Input.GetAxisRaw ("Pitch");
		_v3RotDir.y = Input.GetAxisRaw ("Pitch");
		_v3RotDir.z = Input.GetAxisRaw ("Pitch");

//Controlling rotation around the X, Y, and Z axes:
//X-axis rotation (pitch up/down)
		if (Mathf.Abs (Input.GetAxis ("Pitch")) > controllerDeadzone) {
			normalizedInputX = Mathf.Abs (Input.GetAxis ("Pitch") - controllerDeadzone);
			normalizedInputPercentX = (normalizedInputX/availableInputRange) * 100;

			v3RotThrust.x = (Input.GetAxisRaw ("Pitch") * normalizedInputPercentX * xRotThrust) / 100;
			v3RotCounterThrust.x = 0;
		}

//Arrest rotation around the X-axis 
		else if ( (rotCounterThrustON == true  || Input.GetButton ("AllStop")) ) {
			v3RotThrust.x = 0;
			v3RotCounterThrust.x = xRotThrust; 
		}
		else {
			v3RotThrust.x = 0;
			v3RotCounterThrust.x = 0; 
		}


//Y-axis rotation (yaw right/left))
		if (Mathf.Abs (Input.GetAxis ("Yaw")) > controllerDeadzone) {
			_v3RotDir.x = Input.GetAxisRaw ("Yaw");
			normalizedInputY = Mathf.Abs (Input.GetAxis ("Yaw") - controllerDeadzone);
			normalizedInputPercentY = (normalizedInputY/availableInputRange) * 100;

			v3RotThrust.y = (Input.GetAxisRaw ("Yaw") * normalizedInputPercentY * yRotThrust) / 100;
			v3RotCounterThrust.y = 0;
		}

//Arrest rotation around the Y-axis 
		else if (rotCounterThrustON == true || Input.GetButton ("AllStop")) {
			v3RotThrust.y = 0;
			v3RotCounterThrust.y = yRotThrust; 
		}
		else {
			v3RotThrust.y = 0;
			v3RotCounterThrust.y = 0; 
		}


//Z-axis rotation (roll right/left)
		if (Mathf.Abs (Input.GetAxis ("Roll")) > controllerDeadzone) {
			_v3RotDir.x = Input.GetAxisRaw ("Roll");
			normalizedInputZ = Mathf.Abs (Input.GetAxis ("Roll") - controllerDeadzone);
			normalizedInputPercentZ = (normalizedInputZ/availableInputRange) * 100;

			v3RotThrust.z = (Input.GetAxisRaw ("Roll") * normalizedInputPercentZ * zRotThrust) / 100;
			v3RotCounterThrust.z = 0;
		}

//Arrest rotation around the Z-axis 
		else if (rotCounterThrustON == true || Input.GetButton ("AllStop")) {
			v3RotThrust.z = 0;
			v3RotCounterThrust.z = zRotThrust; 
		}
		else {
			v3RotThrust.z = 0;
			v3RotCounterThrust.z = 0; 
		}


//Calculate Rotation vector
		v3Rotation += v3RotThrust * Time.deltaTime;

		v3Rotation.x -= Mathf.Clamp (v3RotCounterThrust.x * Time.deltaTime, 0, Mathf.Abs (v3Rotation.x)) * Mathf.Sign (v3Rotation.x);
		v3Rotation.y -= Mathf.Clamp (v3RotCounterThrust.y * Time.deltaTime, 0, Mathf.Abs (v3Rotation.y)) * Mathf.Sign (v3Rotation.y); 
		v3Rotation.z -= Mathf.Clamp (v3RotCounterThrust.z * Time.deltaTime, 0, Mathf.Abs (v3Rotation.z)) * Mathf.Sign (v3Rotation.z);

//Apply rotation to this object
		transform.Rotate (v3Rotation);
	}


//fin
}