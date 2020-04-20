using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plrController : MonoBehaviour {

	//Variables
	CharacterController SpaceShip;
	//float _xMotion, _yMotion, _zMotion;												//Containers for XYZ velocity values
	float _xRotation, _yRotation, _zRotation;										//Containers for XYZ orientation values
	int _xDir, _yDir, _zDir;														//Containers for XYZ thrust direction

	Vector3 v3Thrust;																//Vector3 for applying thrust (aligned by _v3Rotation)
	float xCounterThrustTimer = 0, yCounterThrustTimer = 0;							//Timers for storing and apllying counter-thrust if motCounterThrustON == true
	//Vector3 v3CounterThrust = Vector3.zero, v3CounterThrustA = Vector3.zero;		//Vector3 for applying counter-thrust with AllStop or motCounterThrustON
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



//GET DIRECTION OF MOTION



//SPACESHIP THRUST CONTROLS
//SpaceShip X-Axis thrust
		if (Mathf.Abs (Input.GetAxis ("MotionX")) > controllerDeadzone) {
			_xDir = (int)Input.GetAxisRaw ("MotionX");
			v3Thrust.x = _xDir * xThrust;
			xCounterThrustTimer += Time.deltaTime;
		}
		else {
			if (Input.GetButton ("AllStop") || (motCounterThrustON == true && xCounterThrustTimer > 0) ) {
				v3Thrust.x = _xDir * -xThrust;
				xCounterThrustTimer -= Time.deltaTime;
			}
			else {
				v3Thrust.x = 0;
			}
		}

//SpaceShip Y-Axis thrust
		if (Mathf.Abs (Input.GetAxis ("MotionY")) > controllerDeadzone) {
			_yDir = (int)Input.GetAxisRaw ("MotionY");
			v3Thrust.y = _yDir * yThrust;
			yCounterThrustTimer += 1 * Time.deltaTime;
		}
		else {
			if (Input.GetButton ("AllStop") || (motCounterThrustON == true && yCounterThrustTimer > 0) ) {
				v3Thrust.y = _yDir * -yThrust;
				yCounterThrustTimer -= 1* Time.deltaTime;
			}
			else {
				v3Thrust.y = 0;
			}
		}

//SpaceShip Z-Axis thrust
		if (Mathf.Abs (Input.GetAxis ("MotionZ")) > controllerDeadzone) {
			_zDir = (int)Input.GetAxisRaw ("MotionZ");
			v3Thrust.z = _zDir * zThrust;
		}
		else {
			if (Input.GetButton ("AllStop")) {
				v3Thrust.z = _zDir * -zThrust;
			}
			else {
				v3Thrust.z = 0;
			}
		}



////////////////////////////////////////////////////////////////////////////
//**************************************************************************

//Rotate the thrust vector to get the proper direction for the thrust vector
		v3Thrust = _v3Rotation * v3Thrust;

//Apply thrust to v3Motion...
		v3Motion.x += v3Thrust.x * Time.deltaTime;
		v3Motion.y += v3Thrust.y * Time.deltaTime;
		v3Motion.z += v3Thrust.z * Time.deltaTime;

//... and cap the in-game speed.
		v3Motion.x = Mathf.Clamp (v3Motion.x, -xSpeedCap, xSpeedCap);
		v3Motion.y = Mathf.Clamp (v3Motion.y, -ySpeedCap, ySpeedCap);
		v3Motion.z = Mathf.Clamp (v3Motion.z, -zSpeedCap, zSpeedCap);


		SpaceShip.Move (v3Motion * Time.deltaTime);		
	}

////////////////////////////////////////////////////////////////////////////
//**************************************************************************

//fin
}





/*
//SpaceShip Y-Axis thrust
		if (Mathf.Abs (Input.GetAxis ("MotionY")) > controllerDeadzone) {
			v3Thrust.y = Input.GetAxisRaw ("MotionY") * yThrust;
		}
		else {
			v3Thrust.y = 0;
		}

//SpaceShip Z-Axis thrust
		if (Mathf.Abs (Input.GetAxis ("MotionZ")) > controllerDeadzone) {
			v3Thrust.z = Input.GetAxisRaw ("MotionZ") * zThrust;
		}
		else {
			v3Thrust.z = 0;
		}



/*
//ALLSTOP
//If AllStop == true
//if input = 0
//Counter-thrust == thrust
//Rotate by v3rotation
//Apply to v3Motion

//If ALLSTOP
		if (Input.GetButton ("AllStop")) {
			if (Input.GetAxis ("MotionX") <= controllerDeadzone) {
				v3CounterThrust.x = xThrust;
			}
			else {
				v3CounterThrust.x = 0;
			}
// Y
			if (Input.GetAxis ("MotionY") <= controllerDeadzone) {
				v3CounterThrust.y = yThrust;
			}
			else {
				v3CounterThrust.y = 0;
			}
// Z
			if (Input.GetAxis ("MotionZ") <= controllerDeadzone) {
				v3CounterThrust.z = zThrust;
			}
			else {
				v3CounterThrust.z = 0;
			}
			v3CounterThrust = _v3Rotation * v3CounterThrust;
			v3Motion.x = Mathf.Lerp (v3Motion.x, 0, v3CounterThrust.x);
			v3Motion.y = Mathf.Lerp (v3Motion.y, 0, v3CounterThrust.y);
			v3Motion.z = Mathf.Lerp (v3Motion.z, 0, v3CounterThrust.z);
		}
//else
		else if (motCounterThrustON == true) {
//If thrust direction == input direction
//WONT WORK
//Cos we need direction of motion vvv instead of v3Thrust.x
//Maybe some jiggory pokery involving angles.
			if (Mathf.Abs (v3Thrust.x) == Input.GetAxisRaw ("MotionX")) {
				xCounterThrustTimer += Time.deltaTime;
			}
			if (Mathf.Abs (Input.GetAxis ("MotionX")) <= controllerDeadzone && xCounterThrustTimer > 0) {
				v3CounterThrustA.x = xThrust;
				xCounterThrustTimer-= Time.deltaTime;
			}
			else {
				v3CounterThrustA.x = 0;
			}

			xCounterThrustTimer = Mathf.Clamp (xCounterThrustTimer, 0, xSpeedCap);

			if (Mathf.Abs (v3Thrust.y) == Input.GetAxisRaw ("MotionY")) {
				v3CounterThrustA.x = yThrust;
				yCounterThrustTimer-= Time.deltaTime;
			}
			else {
				v3CounterThrustA.y = 0;
			}

			yCounterThrustTimer = Mathf.Clamp (yCounterThrustTimer, 0, ySpeedCap);

			v3CounterThrustA = _v3Rotation * v3CounterThrustA;

			v3Motion.x = Mathf.Lerp (v3Motion.x, 0, v3CounterThrustA.x);
			v3Motion.y = Mathf.Lerp (v3Motion.y, 0, v3CounterThrustA.y);

		}
*/






//CONTROLLING THE CAMERA TO TRACK AHEAD OF THE SPACESHIP'S ORIENTATION. THE CAMERA WILL LERP AFTER THE PLAYER AND ALSO LERP TO TRACK THE PLAYER'S AIMING RETICULE (HOPEFULLY)
		//viewport.transform.rotation...



//fin
//}
