using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Car : MonoBehaviour {

	public WheelCollider[] wheels; //List of wheel colliders owned by the car
	public List<MeshRenderer> wheelMeshes = new List<MeshRenderer>(); //List of rendered wheels
	public float[] gearRatios;
	public int currentGear;
	public float outputTorque;
	CarStats carStats; // Info about the car 
	Rigidbody rb; // 

	// Use this for initialization
	void Awake () {
	
		carStats = GetComponent<CarStats> ();
		wheels = GetComponentsInChildren<WheelCollider> ();
		gearRatios = carStats.gearRatios.ToArray();
		rb = GetComponent<Rigidbody> ();

		for (int i = 0; i < GetComponentsInChildren<MeshRenderer> ().Length; i++) {

			wheelMeshes.Add (GetComponentsInChildren<MeshRenderer> () [i]);

		}


		wheelMeshes.RemoveAt (0);

		WheelFrictionCurve front = wheels[0].sidewaysFriction;
		WheelFrictionCurve rear = wheels[0].sidewaysFriction;

		front.stiffness = carStats.frontFrictionStiffness;
		rear.stiffness = carStats.rearFrictionStiffness;

		for (int i = 0; i < wheels.Length - 2; i++) {

			wheels[i].sidewaysFriction = front;

		}

		for (int i = 2; i < wheels.Length; i++) {

			wheels[i].sidewaysFriction = rear;

		}

	}
	
	// Update is called once per frame
	void Update () {

		SpeedControl ();
		CorrectWheelPos ();
		DownForce ();
	
	}

	void DownForce(){

		rb.AddForce(Vector3.down * (rb.velocity.magnitude / carStats.topSpeed) * carStats.downForceModifier);

	}

	void SpeedControl(){

		if (carStats.rb.velocity.magnitude >= carStats.topSpeed) {

			carStats.rb.velocity = carStats.rb.velocity.normalized * carStats.topSpeed;

		}

	}

	void CorrectWheelPos(){

		Vector3 wheelPos;
		Quaternion wheelRot;

		for (int i = 0; i < wheels.Length; i++) {

			wheels [i].GetWorldPose (out wheelPos, out wheelRot);

			wheelMeshes [i].transform.position  = wheelPos;
			wheelMeshes [i].transform.rotation = Quaternion.Euler(wheelRot.eulerAngles - new Vector3(0,0,90));

		}

	}

	public void Drive(float h,float v,bool b){

		for (int i = 0; i < wheels.Length - 2; i++) {

			wheels[i].steerAngle = Mathf.Lerp(wheels[i].steerAngle,h * carStats.steerAngle,Time.deltaTime * (1/carStats.turnTime));

		}

		for (int i = 2; i < wheels.Length; i++) {

			if (b) {

				wheels [i].brakeTorque = carStats.brakeTorque;

			} else {

				wheels [i].brakeTorque = 0;
				wheels[i].motorTorque = v * carStats.torque;

			}

		}

	}

}
