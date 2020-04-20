using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plrSpaceShipControllerV3 : MonoBehaviour {

	//Game settings
	public float xThrust = 2f, yThrust = 2f, zThrust = 4f;

	//Player settings
	public float controllerDeadzone = 0.1f;
	public bool motCounterThrustON= true;

	//Variables
	Vector3 v3Thrust, v3Motion, _v3Motion;					//Thrust vector (local rotation), Motion (world), Motion (local rotation)
	Quaternion _v3Rotation; 								//Local rotation
	int _xDir, _yDir, _zDir;




	void Start () {
		controllerDeadzone = Mathf.Clamp (controllerDeadzone, 0, 0.99f);
	}
	
	void Update () {
		
		if (Mathf.Abs (Input.GetAxis ("MotionX")) > controllerDeadzone) {
			v3Thrust.x = Input.GetAxisRaw ("MotionX") * xThrust;
			_xDir = (int) Input.GetAxisRaw ("MotionX");
		}
		else {
			v3Thrust.x = 0;
		}

		v3Thrust = _v3Rotation * v3Thrust;
		_v3Motion = _v3Rotation * v3Motion;
	}

	void LateUpdate () {

			v3Motion.x += v3Thrust.x * Time.deltaTime;
		if (Input.GetButton ("AllStop") || motCounterThrustON == true) {
			_v3Motion.x -= (_xDir * xThrust) * Time.deltaTime;
			v3Motion.x = _v3Motion.x;
		}




		//thingy.Move (v3Motion * Time.deltaTime);
	}


}
