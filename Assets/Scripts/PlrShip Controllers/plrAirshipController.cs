using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plrAirshipController : MonoBehaviour {

	//Variables
	CharacterController SpaceShip;
	float _xMotion, _yMotion, _zMotion;														//Containers for XYZ velocity values
	float _xRotation, _yRotation, _zRotation;												//Containers for XYZ orientation values
	Vector3 v3Thrust;																		//Vector3 for applying thrust (aligned by _v3Rotation)
	Vector3 v3CounterThrust = Vector3.zero, v3CounterThrustA = Vector3.zero;				//Vector3 for applying counter-thrust with AllStop or motCounterThrustON
//	float xCounterThrustTimer = 0, yCounterThrustTimer = 0;									//Timers for storing and apllying counter-thrust if motCounterThrustON == true
	Vector3 v3Motion;																		//Absolute movement vector (world-aligned)
	Quaternion _v3Rotation;																	//Vector for XYZ rotation

	//Game Settings
	public float xThrust = 0.5f, yThrust = 0.5f, zThrust = 0.5f;				//Thrust delivered by XYZ thruster(s) for orthagonal motion
	public float xSpeedCap = 10f, ySpeedCap = 10f, zSpeedCap = 20f;				//Caps on XYZ motion/rotation velocity
	public float xRotThrust = 0.5f, yRotThrust = 0.5f, zRotThrust = 0.5f;		//Thrust delivered during pitch/yaw/roll manueveres
	public float xRotSpeedCap = 10f, yRotSpeedCap = 10f, zRotSpeedCap = 10f;	//You get the picture ¬_¬

	//Player Settings
	public float controllerDeadzone = 0.1f;								//Dampen excess joystick/analogue stick sensitivity
	public bool motCounterThrustON = true;								//If enabled, provides an automatic counter-force against XY velocity when the XY thrusters cease firing.
	public bool rotCounterThrustON = true;								//If enabled, provides an automatic counter-force against XYZ rotation when XYZ thrusters cease firing.

	public void InitializeSettings () {
		controllerDeadzone = Mathf.Clamp (controllerDeadzone , 0, 0.99f);
	}



	void Awake () {
	}



	void Start () {
		SpaceShip = this.GetComponent<CharacterController>();
		InitializeSettings ();
	}



	void Update () {



//OLD AIRSHIP-STYLE CONTROLS
//This is due to the v3Motion/ thrust adding the lerped up speed values before applying the rotation; in the SPACESHIP-STYLE CONTROLS the rotation of the thrusters is taken into account before adding it to the v3Motion, which doesn't rotate.
//It's still good though, for a final fantasy airship-style game 



//Controlling movement along the Z, Y, and X axes:
//Z-axis thrust (forward/backward)
		if (Mathf.Abs (Input.GetAxis ("MotionZ")) > controllerDeadzone) {
			_zMotion = Mathf.Lerp (_zMotion, zSpeedCap * Input.GetAxisRaw ("MotionZ"), zThrust * Time.deltaTime); 
		}
//Arrest Z movement with AllStop
		else if (Input.GetButton ("AllStop")) {
			_zMotion = Mathf.Lerp (_zMotion, 0, zThrust * Time.deltaTime);
		}

//Y-axis thrust (up/down)
		if (Mathf.Abs (Input.GetAxis ("MotionY")) > controllerDeadzone) {
		//if (Input.GetAxisRaw ("MotionY") != 0) {
			_yMotion = Mathf.Lerp (_yMotion, ySpeedCap * Input.GetAxisRaw ("MotionY"), yThrust * Time.deltaTime);
		}
//Arrest Y movement with  automatic counter-thrust or AllStop
		else if (motCounterThrustON == true || Input.GetButton ("AllStop")) {
			_yMotion = Mathf.Lerp (_yMotion, 0, yThrust * Time.deltaTime);				
		}

//X-axis thrust (right/left)
		if (Mathf.Abs (Input.GetAxis ("MotionX")) > controllerDeadzone) {
		//if (Input.GetAxisRaw ("MotionX") != 0) {
			_xMotion = Mathf.Lerp (_xMotion, xSpeedCap * Input.GetAxisRaw ("MotionX"), xThrust * Time.deltaTime);
		}
//Arrest X movement with automatic counter-thrust or AllStop
		else if (motCounterThrustON == true || Input.GetButton ("AllStop")) {
			_xMotion = Mathf.Lerp (_xMotion, 0, xThrust * Time.deltaTime);
		}

//end
//Controlling rotation around the X, Y, and Z axes:
//X-axis rotation (pitch up/down)
		if (Mathf.Abs (Input.GetAxis ("Pitch")) > controllerDeadzone) {
			_xRotation = Mathf.Lerp (_xRotation, xRotSpeedCap * Input.GetAxisRaw ("Pitch"), xRotThrust * Time.deltaTime);
		}
//Arrest rotation around the X-axis 
		else if (rotCounterThrustON == true || Input.GetButton ("AllStop")) {
			_xRotation = Mathf.Lerp (_xRotation, 0, xRotThrust * Time.deltaTime);
		}
//Y-axis rotation (yaw right/left)
		if (Mathf.Abs (Input.GetAxis ("Yaw")) > controllerDeadzone) {
			_yRotation = Mathf.Lerp (_yRotation, yRotSpeedCap * Input.GetAxisRaw ("Yaw"), yRotThrust * Time.deltaTime);
		}
//Arrest rotation around the Y-axis 
		else if (rotCounterThrustON == true || Input.GetButton ("AllStop")) {
			_yRotation = Mathf.Lerp (_yRotation, 0, yRotThrust * Time.deltaTime);
		}
//Z-axis rotation (roll right/left)
		if (Mathf.Abs (Input.GetAxis ("Roll")) > controllerDeadzone) {
			_zRotation = Mathf.Lerp (_zRotation, zRotSpeedCap * Input.GetAxisRaw ("Roll"), zRotThrust * Time.deltaTime);
		}
//Arrest rotation around the Z-axis 
		else if (rotCounterThrustON == true || Input.GetButton ("AllStop")) {
			_zRotation = Mathf.Lerp (_zRotation, 0, zRotThrust * Time.deltaTime);
		}
		transform.Rotate (_xRotation, _yRotation, _zRotation);
		_v3Rotation = transform.rotation;

		v3Thrust.x = _xMotion;
		v3Thrust.y = _yMotion;
		v3Thrust.z = _zMotion;
		v3Thrust = _v3Rotation * v3Thrust;

		SpaceShip.Move (v3Thrust * Time.deltaTime);
		
	}

//fin
}
