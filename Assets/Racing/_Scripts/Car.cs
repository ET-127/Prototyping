using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Car : NetworkBehaviour {

	public WheelCollider[] wheels; //List of wheel colliders owned by the car
	public List<MeshRenderer> wheelMeshes = new List<MeshRenderer>(); //List of rendered wheels
	public float[] gearRatios;
	public float[] gearTopSpeeds;
	public float outputTorque;

	public AnimationCurve frictionCurve;

	CarStats carStats; // Info about the car 
	Rigidbody rb; // 

	// Use this for initialization
	void Awake () {
	
		carStats = GetComponent<CarStats> ();
		wheels = GetComponentsInChildren<WheelCollider> ();

		gearRatios = carStats.gearRatios.ToArray();

		carStats.wheelDiameter = wheels[0].radius * 2;

		float c = carStats.wheelDiameter * Mathf.PI;

		for(int i = 0; i < gearRatios.Length;i++) {

			carStats.gearTopSpeeds.Add(((carStats.engineTorque / 60) / (gearRatios[i] * carStats.transmission)) * c);
			                         
		}

		gearTopSpeeds = carStats.gearTopSpeeds.ToArray();

		rb = GetComponent<Rigidbody> ();

		for (int i = 0; i < GetComponentsInChildren<MeshRenderer> ().Length; i++) {

			wheelMeshes.Add (GetComponentsInChildren<MeshRenderer> () [i]);

		}

		wheelMeshes.RemoveAt (0);

		//Friction Stuff
		WheelFrictionCurve frontFriction = wheels[0].sidewaysFriction;
		WheelFrictionCurve rearFriction = wheels[0].sidewaysFriction;

		frontFriction.stiffness = carStats.frontFrictionStiffness;
		rearFriction.stiffness = carStats.rearFrictionStiffness;

		//Front
		//Slip = X,Value = Y.

		//Extremum
		frontFriction.extremumSlip = carStats.Extremum.x;
		frontFriction.extremumValue = carStats.Extremum.y;

		//Asymptote
		frontFriction.asymptoteSlip = carStats.Asymptote.x;
		frontFriction.asymptoteValue = carStats.Asymptote.y;

		//Rear
		//Slip = X,Value = Y.

		//Extremum
		rearFriction.extremumSlip = carStats.Extremum.x;
		rearFriction.extremumValue = carStats.Extremum.y;

		//Asymptote
		rearFriction.asymptoteSlip = carStats.Asymptote.x;
		rearFriction.asymptoteValue = carStats.Asymptote.y;

		//Spring Stuff
		JointSpring frontSuspension = wheels[0].suspensionSpring;
		JointSpring rearSuspension = wheels[0].suspensionSpring;

		frontSuspension.spring = carStats.springFront;
		rearSuspension.spring = carStats.springBack;

		for (int i = 0; i < wheels.Length - 2; i++) {

			wheels[i].sidewaysFriction = frontFriction;
			wheels[i].suspensionSpring = frontSuspension;

		}

		for (int i = 2; i < wheels.Length; i++) {

			wheels[i].sidewaysFriction = rearFriction;
			wheels[i].suspensionSpring = rearSuspension;

		}

	}

	void SwitchGear(){

		if (carStats.currentSpeed >= gearTopSpeeds [carStats.currentGear]){

			carStats.currentGear++;

		} else if(carStats.currentGear - 1 > -1 && carStats.currentSpeed <= gearTopSpeeds [carStats.currentGear - 1]){

			carStats.currentGear--;

		}

		carStats.currentGear = Mathf.Clamp(carStats.currentGear,0 , gearTopSpeeds.Length - 1);

	}
	
	void Graph(){

		frictionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(carStats.Extremum.x, carStats.Extremum.y),new Keyframe(carStats.Asymptote.x, carStats.Asymptote.y));
	
		frictionCurve.keys[1].time = carStats.Extremum.x;
		frictionCurve.keys[1].value = carStats.Extremum.y;

		frictionCurve.keys[2].time = carStats.Asymptote.x;
		frictionCurve.keys[2].value = carStats.Asymptote.y;

			
	}

	// Update is called once per frame
	void Update () {

		Graph ();
		SwitchGear ();
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
			wheelMeshes [i].transform.rotation = Quaternion.Euler(wheelRot.eulerAngles);

		}

	}

	public void Cmd_Drive(float h,float v,bool b){

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
