using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour {

	public float controllerDeadzone = 0f;
	public bool motCounterThrustON = true;
	public bool rotCounterThrustON = true;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		controllerDeadzone = Mathf.Clamp (controllerDeadzone , 0, 0.99f);
	}
}
