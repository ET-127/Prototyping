using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarStats : MonoBehaviour {

	public float topSpeed;
	public float torque;
	public float brakeTorque;
	public float steerAngle;
	public float frontFrictionStiffness;
	public float rearFrictionStiffness;
	public float downForceModifier;
	public float turnTime;

	public int numOfGears;
	public List<float> gearRatios;

	public Rigidbody rb;

	public float currentSpeed;

	// Use this for initialization
	void Awake () {

		rb = GetComponent<Rigidbody> ();
	
	}
	
	// Update is called once per frame
	void Update () {

		currentSpeed = rb.GetComponent<Rigidbody> ().velocity.magnitude;
	
	}
}
