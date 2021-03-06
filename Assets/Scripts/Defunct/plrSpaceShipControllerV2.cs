using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plrSpaceShipControllerV2 : MonoBehaviour {

	//Variables
	CharacterController SpaceShip;
	float _xDir, _yDir, _zDir;
	float _xRotation, _yRotation, _zRotation;										//Containers for XYZ thrust direction
//
	float xCounterThrustTimer = 0, yCounterThrustTimer = 0;							//Timers for storing and apllying counter-thrust if motCounterThrustON == true
//
	Vector3 v3Thrust, _v3Thrust;																//Vector3 for applying thrust (aligned by _v3Rotation)
	Vector3 v3SpeedCap;
	Quaternion _v3Rotation;															//Vector for XYZ rotation
	Vector3 v3Motion;																//Absolute movement vector (world-aligned)

	//Game Settings
	public float xThrust = 2f, yThrust = 2f, zThrust = 4f;							//Thrust delivered by XYZ thruster(s) for orthagonal motion
	public float xSpeedCap = 10f, ySpeedCap = 10f, zSpeedCap = 20f;					//Caps on XYZ motion/rotation velocity
	public float xRotThrust = 0.5f, yRotThrust = 0.5f, zRotThrust = 0.5f;			//Thrust delivered during pitch/yaw/roll manueveres
	public float xRotSpeedCap = 10f, yRotSpeedCap = 10f, zRotSpeedCap = 10f;		//You get the picture ¬_¬

	//Player Settings
	public float controllerDeadzone = 0.1f;											//Dampen excess joystick/analogue stick sensitivity
	public bool motCounterThrustON = true;											//If enabled, provides an automatic counter-force against XY velocity when the XY thrusters cease firing.
	public bool rotCounterThrustON = true;											//If enabled, provides an automatic counter-force against XYZ rotation when XYZ thrusters cease firing.


	void Awake () {
	}


	void Start () {
		SpaceShip = this.GetComponent<CharacterController>();
		InitializeSettings ();
	}


	public void InitializeSettings () {
		controllerDeadzone = Mathf.Clamp (controllerDeadzone , 0, 0.99f);
		//v3Thrust = new Vector3 (xThrust, yThrust, zThrust);
		v3SpeedCap = new Vector3 (xSpeedCap, ySpeedCap, zSpeedCap);
	}


	void Update () {

//NEW SPACESHIP-STYLE CONTROLS
//This apples rotation to the thrust vector before applying it to the mvement vector, allowing for realistic application of space-style thrust.



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
//ROTATION IS IMPORTANT FOR APPLYING THRUST CORRECTLY, WHICH IS WHY IT APPEARS AT THE TOP OF THE SCRIPT





//NEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW




//If Input > deadzone
		if (Mathf.Abs (Input.GetAxis ("MotionX")) > controllerDeadzone) {
//Set xThrust
			v3Thrust.x = Input.GetAxisRaw ("MotionX") * xThrust;
			_v3Thrust.x += v3Thrust.x * Time.deltaTime;				//
//Record direction of motion if motion is non-zero
			if (Mathf.Sign (_v3Thrust.x) == Input.GetAxisRaw ("MotionX") ) {
				_xDir = Input.GetAxisRaw ("MotionX");
			}
		}
		else if (Input.GetButton ("AllStop") && Mathf.Abs (_v3Thrust.x) > 0) {
			v3Thrust.x = Mathf.Sign (_v3Thrust.x) * -xThrust;
		}
		else {
			v3Thrust.x = 0;
		}



//If Input > deadzone
		if (Mathf.Abs (Input.GetAxis ("MotionY")) > controllerDeadzone) {
//Set xThrust
			v3Thrust.y = Input.GetAxisRaw ("MotionY") * yThrust;
//Record direction of motion if motion is non-zero
			if (Mathf.Sign (_v3Thrust.y) == Input.GetAxisRaw ("MotionY") ) {
				_yDir = Input.GetAxisRaw ("MotionY");
			}
		}
		else if (Input.GetButton ("AllStop") && Mathf.Sign (_v3Thrust.y) == _yDir) {
			v3Thrust.y = _yDir * -yThrust;
		}
		else {
			v3Thrust.y = 0;
		}



//If Input > deadzone
		if (Mathf.Abs (Input.GetAxis ("MotionZ")) > controllerDeadzone) {
//Set xThrust
			v3Thrust.z = Input.GetAxisRaw ("MotionZ") * zThrust;
//Record direction of motion if motion is non-zero
			if (Mathf.Sign (_v3Thrust.z) == Input.GetAxisRaw ("MotionZ") ) {
				_zDir = Input.GetAxisRaw ("MotionZ");
			}
		}
		else if (Input.GetButton ("AllStop") && Mathf.Sign (_v3Thrust.z) == _zDir) {
			v3Thrust.z = _zDir * -zThrust;
		}
		else {
			v3Thrust.z = 0;
		}
		//...
		v3Thrust = _v3Rotation * v3Thrust;



//Apply thrust to v3Motion...
		v3Motion.x += v3Thrust.x * Time.deltaTime;
		v3Motion.y += v3Thrust.y * Time.deltaTime;
		v3Motion.z += v3Thrust.z * Time.deltaTime;


//... and cap the in-game speed.
//		v3Motion.x = Mathf.Clamp (v3Motion.x, -v3SpeedCap.x, v3SpeedCap.x);
//		v3Motion.y = Mathf.Clamp (v3Motion.y, -v3SpeedCap.y, v3SpeedCap.y);
//		v3Motion.z = Mathf.Clamp (v3Motion.z, -v3SpeedCap.z, v3SpeedCap.z);


		Debug.Log (_v3Thrust);
		SpaceShip.Move (v3Motion * Time.deltaTime);
	}


////////////////////////////////////////////////////////////////////////////
//**************************************************************************

//fin
}