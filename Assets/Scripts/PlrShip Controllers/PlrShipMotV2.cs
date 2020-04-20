using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlrShipMotV2 : MonoBehaviour {

	//Variables
	CharacterController SpaceShip;													//Reference for the controller of this object
	Vector3 v3Thrust, v3CounterThrust;												//Vectors for adding thrust and counter-thrust to the motion of this object
	Vector3 v3Motion;																//Vector for applying motion to this object along world axes
	Vector3 _v3MotDir;																//Normalized movement vector, for referencing direction of motion of this object
	Quaternion _v3Rotation;															//Container for rotation of this object in world space

	float normalizedInputX, normalizedInputY, normalizedInputZ;						//Input minus controllerDeadzone
	float normalizedInputPercentX, normalizedInputPercentY, normalizedInputPercentZ;//Percentage of normalized Input

	float x_M_FuelUsage, y_M_FuelUsage, z_M_FuelUsage;
	float x_R_FuelUsage, y_R_FuelUsage, z_R_FuelUsage;
	float fuelConsumption;
	float remainingFuel;

	//Game Settings
	public float xThrust = 2f, yThrust = 2f, zThrust = 2f;							//Thrust/acceleration for XYZ motion
	public float xCounterThrust = 2f, yCounterThrust = 2f, zCounterThrust = 2f;		//Counter-thrust/deceleration for XYZ motion
	float availableInputRange;														//Range of input not restriced by deadzone
	public float maxFuel = 1000;
	public float stdFuelUnit = 0.5f;


	//Player Settings
	public bool Settings_Fuel_ON = true;
	public bool motCounterThrustON = true;											//If enabled, provides an automatic counter-force to motion when input is released (XY only)
	public float controllerDeadzone = 0.1f;											//Deadzone under which analogue input will be ignored


	void Start () {
		SpaceShip = this.GetComponent<CharacterController>();
		remainingFuel = maxFuel;
		InitializeSettings ();
	}



	public void InitializeSettings () {
		controllerDeadzone = Mathf.Clamp (controllerDeadzone , 0, 0.99f);
		availableInputRange = 1 - controllerDeadzone;
	}



	void Update () {

//Get direction of motion and local rotation of this object
		_v3MotDir = v3Motion.normalized;
		_v3Rotation = transform.rotation;

//Controlling motion along the X, Y, and Z axes:
//X-axis thrust (local axis)
		if (Mathf.Abs (Input.GetAxis ("MotionX")) > controllerDeadzone) {
			normalizedInputX = Mathf.Abs (Input.GetAxis ("MotionX") - controllerDeadzone);
			normalizedInputPercentX = (normalizedInputX/availableInputRange) * 100;

			v3Thrust.x = (Input.GetAxisRaw ("MotionX") * normalizedInputPercentX * xThrust) / 100;
			v3CounterThrust.x = 0;
		}
//X-axis counter-thrust (local axis)
		else if (Input.GetButton ("AllStop") || motCounterThrustON == true) {
			v3Thrust.x = 0;
			v3CounterThrust.x = Vector3.Dot (transform.right, _v3MotDir) * xCounterThrust;
		}
		else {
			v3Thrust.x = 0;
			v3CounterThrust.x = 0;
		}

//Y-axis thrust (local axis)
		if (Mathf.Abs (Input.GetAxis ("MotionY")) > controllerDeadzone) {
			normalizedInputY = Mathf.Abs (Input.GetAxis ("MotionY") - controllerDeadzone);
			normalizedInputPercentY = (normalizedInputY/availableInputRange) * 100;

			v3Thrust.y = (Input.GetAxisRaw ("MotionY") * normalizedInputPercentY * yThrust) / 100;
			v3CounterThrust.y = 0;
		}
//Y-axis counter-thrust (local axis)
		else if (Input.GetButton ("AllStop") || motCounterThrustON == true) {
			v3Thrust.y = 0;
			v3CounterThrust.y = Vector3.Dot (transform.up, _v3MotDir) * yCounterThrust;
		}
		else {
			v3Thrust.y = 0;
			v3CounterThrust.y = 0;
		}

//Z-axis thrust (local axis)
		if (Mathf.Abs (Input.GetAxis ("MotionZ")) > controllerDeadzone) {
			normalizedInputZ = Mathf.Abs (Input.GetAxis ("MotionZ") - controllerDeadzone);
			normalizedInputPercentZ = (normalizedInputZ/availableInputRange) * 100;

			v3Thrust.z = (Input.GetAxisRaw ("MotionZ") * normalizedInputPercentZ * zThrust) / 100;
			v3CounterThrust.z = 0;
		}
//Z-axis counter-thrust (local axis)
		else if (Input.GetButton ("AllStop") || (motCounterThrustON == true && Vector3.Dot (transform.forward, _v3MotDir) <= 0) ) {
			v3Thrust.z = 0;
			v3CounterThrust.z = Vector3.Dot (transform.forward, _v3MotDir) * zCounterThrust;
		}
		else {
			v3Thrust.z = 0;
			v3CounterThrust.z = 0;
		}

//Rotate thrust and counter-thrust vectors to match the rotation of this object
		v3Thrust = _v3Rotation * v3Thrust;
		v3CounterThrust = _v3Rotation * v3CounterThrust;

//Apply input thrust to the motion of this object
		v3Motion.x += v3Thrust.x * Time.deltaTime;
		v3Motion.y += v3Thrust.y * Time.deltaTime;
		v3Motion.z += v3Thrust.z * Time.deltaTime;

//Apply counter-thrust to the motion of this object
		v3Motion.x -= v3CounterThrust.x * Time.deltaTime;
		v3Motion.y -= v3CounterThrust.y * Time.deltaTime;
		v3Motion.z -= v3CounterThrust.z * Time.deltaTime;

//Apply motion to this object
		SpaceShip.Move (v3Motion * Time.deltaTime);

//Calculate fuel usage
		if (Settings_Fuel_ON == true) {
			fuelConsumption = Mathf.Abs (v3Thrust.x) + Mathf.Abs (v3Thrust.y) + Mathf.Abs (v3Thrust.z);
			remainingFuel -= fuelConsumption * Time.deltaTime;
			remainingFuel = Mathf.Clamp (remainingFuel, 0, maxFuel);
			Debug.Log ("Remaining Fuel: " +remainingFuel);
		}
	}


//fin
}
