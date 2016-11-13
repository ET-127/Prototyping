using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarStats : MonoBehaviour {

	public float topSpeed;
	public float torque;
	public float engineTorque;
	public float brakeTorque;
	public float steerAngle;

	public float frontFrictionStiffness;
	public float rearFrictionStiffness;

	public float springFront;
	public float springBack;
	public float dampingFront;
	public float dampingBack;
	public Vector2 Extremum = new Vector2(0.2f,1);
	public Vector2 Asymptote = new Vector2(0.5f,0.75f);

	public float downForceModifier;
	public float turnTime;

	public int numOfGears;
	public List<float> gearRatios;
	public List<float> gearTopSpeeds;
	public float wheelDiameter;

	public float transmission;
	public int currentGear = 1;

	public Rigidbody rb;

	public float currentSpeed;
	public float acceleration;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody> ();
		topSpeed = gearTopSpeeds [gearTopSpeeds.Count - 1];
	
	}

	// Update is called once per frame
	void FixedUpdate () {

		float lspeed = 0;

		torque = engineTorque * transmission * gearRatios [currentGear];

		currentSpeed = rb.GetComponent<Rigidbody> ().velocity.magnitude;
		acceleration = (currentSpeed - lspeed) / Time.fixedDeltaTime;
		lspeed = currentSpeed;
	
	}
}
