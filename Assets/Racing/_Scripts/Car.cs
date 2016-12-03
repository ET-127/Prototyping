using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Car : NetworkBehaviour {

	public WheelCollider[] wheels; //List of wheel colliders owned by the car
	public List<MeshRenderer> wheelMeshes = new List<MeshRenderer>(); //List of rendered wheels
	public List<TrailRenderer> skidmarks = new List<TrailRenderer>();
	public GameObject skidMarkPrefab;

	public float maxSlipLimitS;
	public float maxSlipLimitF;

	public float[] gearRatios;
	public float[] gearTopSpeeds;
	public float outputTorque;

	public AnimationCurve forwardFrictionCurve;
	public AnimationCurve sideFrictionCurve;

	[SyncVar]
	public float forwardSlip;

	[SyncVar]
	public float sidewaysSlip;

	CarStats carStats; // Info about the car 
	Rigidbody rb; // 

	// Use this for initialization
	void Start () {


		if (!isLocalPlayer)
			return;
	
		carStats = GetComponent<CarStats> ();
		wheels = GetComponentsInChildren<WheelCollider> ();
		//maxSlipLimitF = carStats.AsymptoteF.x;
		maxSlipLimitS = carStats.ExtremumS.x;

		gearRatios = carStats.gearRatios.ToArray();

		carStats.wheelDiameter = wheels[0].radius * 2;

		float c = carStats.wheelDiameter * Mathf.PI;

		for(int i = 0; i < gearRatios.Length;i++) {

			carStats.gearTopSpeeds.Add( ((carStats.engineTorque / 60) / (gearRatios[i] * carStats.transmission)) * c * carStats.c);
			                         
		}

		gearTopSpeeds = carStats.gearTopSpeeds.ToArray();

		rb = GetComponent<Rigidbody> ();

		for (int i = 0; i < wheels.Length; i++) {

			skidmarks.Add (new TrailRenderer());

		}

		for (int i = 0; i < GetComponentsInChildren<MeshRenderer> ().Length; i++) {

			wheelMeshes.Add (GetComponentsInChildren<MeshRenderer> () [i]);

		}

		wheelMeshes.RemoveAt (0);

		SideFriction ();
		ForwardFriction ();

	}

	void ForwardFriction(){

		//Friction Stuff
		WheelFrictionCurve frontFriction = wheels[0].forwardFriction;
		WheelFrictionCurve rearFriction = wheels[0].forwardFriction;

		frontFriction.stiffness = carStats.frontFrictionStiffness;
		rearFriction.stiffness = carStats.rearFrictionStiffness;

		//Front
		//Slip = X,Value = Y.

		//Extremum
		frontFriction.extremumSlip = carStats.ExtremumF.x;
		frontFriction.extremumValue = carStats.ExtremumF.y;

		//Asymptote
		frontFriction.asymptoteSlip = carStats.AsymptoteF.x;
		frontFriction.asymptoteValue = carStats.AsymptoteF.y;

		//Rear
		//Slip = X,Value = Y.

		//Extremum
		rearFriction.extremumSlip = carStats.ExtremumF.x;
		rearFriction.extremumValue = carStats.ExtremumF.y;

		//Asymptote
		rearFriction.asymptoteSlip = carStats.AsymptoteF.x;
		rearFriction.asymptoteValue = carStats.AsymptoteF.y;

		//Spring Stuff
		JointSpring frontSuspension = wheels[0].suspensionSpring;
		JointSpring rearSuspension = wheels[0].suspensionSpring;

		frontSuspension.spring = carStats.springFront;
		rearSuspension.spring = carStats.springBack;

		for (int i = 0; i < wheels.Length - 2; i++) {

			wheels[i].forwardFriction = frontFriction;

		}

		for (int i = 2; i < wheels.Length; i++) {

			wheels[i].forwardFriction = rearFriction;

		}


	}

	void SideFriction(){

		//Friction Stuff
		WheelFrictionCurve frontFriction = wheels[0].sidewaysFriction;
		WheelFrictionCurve rearFriction = wheels[0].sidewaysFriction;

		frontFriction.stiffness = carStats.frontFrictionStiffness;
		rearFriction.stiffness = carStats.rearFrictionStiffness;

		//Front
		//Slip = X,Value = Y.

		//Extremum
		frontFriction.extremumSlip = carStats.ExtremumS.x;
		frontFriction.extremumValue = carStats.ExtremumS.y;

		//Asymptote
		frontFriction.asymptoteSlip = carStats.AsymptoteS.x;
		frontFriction.asymptoteValue = carStats.AsymptoteS.y;

		//Rear
		//Slip = X,Value = Y.

		//Extremum
		rearFriction.extremumSlip = carStats.ExtremumS.x;
		rearFriction.extremumValue = carStats.ExtremumS.y;

		//Asymptote
		rearFriction.asymptoteSlip = carStats.AsymptoteS.x;
		rearFriction.asymptoteValue = carStats.AsymptoteS.y;

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

	void FrontFrictionGraph(){

		forwardFrictionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(carStats.ExtremumF.x, carStats.ExtremumF.y),new Keyframe(carStats.AsymptoteF.x, carStats.AsymptoteF.y));

		forwardFrictionCurve.keys[1].time = carStats.ExtremumF.x;
		forwardFrictionCurve.keys[1].value = carStats.ExtremumF.y;

		forwardFrictionCurve.keys[2].time = carStats.AsymptoteF.x;
		forwardFrictionCurve.keys[2].value = carStats.AsymptoteF.y;

	}
	
	void SideFrictionGraph(){

		sideFrictionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(carStats.ExtremumS.x, carStats.ExtremumS.y),new Keyframe(carStats.AsymptoteS.x, carStats.AsymptoteS.y));
	
		sideFrictionCurve.keys[1].time = carStats.ExtremumS.x;
		sideFrictionCurve.keys[1].value = carStats.ExtremumS.y;

		sideFrictionCurve.keys[2].time = carStats.AsymptoteS.x;
		sideFrictionCurve.keys[2].value = carStats.AsymptoteS.y;

			
	}

	[Client]
	void Update () {

		if (!isLocalPlayer)
			return;

		SideFrictionGraph ();
		FrontFrictionGraph ();
		SwitchGear ();
		SpeedControl ();
		CorrectWheelPos ();
		Cmd_SkidMarks ();
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

	void GetSlip(WheelHit hit){

		if(!isLocalPlayer){

			hit.sidewaysSlip = sidewaysSlip;
			hit.forwardSlip = forwardSlip;

		}

	}

	[Command]
	void Cmd_SendSlip(WheelHit hit){

		sidewaysSlip = hit.sidewaysSlip;
		forwardSlip = hit.forwardSlip;

	}

	[Client]
	void Cmd_SkidMarks(){

		for (int i = 0; i < wheels.Length; i++) {

			WheelHit hit = new WheelHit();

			wheels [i].GetGroundHit(out hit);

			//Debug.Log (Mathf.Abs(hit.forwardSlip));

			Cmd_SendSlip (hit);

			if (skidmarks[i] == null && (Mathf.Abs (sidewaysSlip) > maxSlipLimitS || (Mathf.Abs(forwardSlip) > maxSlipLimitF))) {

				skidmarks[i] =  Instantiate(skidMarkPrefab).GetComponent<TrailRenderer>();

				RaycastHit rayHit = new RaycastHit ();

				Vector3 colliderCenterPoint = wheels [i].transform.TransformPoint (wheels [i].center);

				if (Physics.Raycast (colliderCenterPoint, -wheels [i].transform.up, out rayHit, wheels [i].suspensionDistance + wheels [i].radius)) {

					skidmarks[i].transform.position = rayHit.point + (wheels [i].transform.up * wheels [i].suspensionDistance);

				} else {

					skidmarks[i].transform.position = colliderCenterPoint - (wheels [i].transform.up * wheels [i].suspensionDistance);

				}

			} else if (skidmarks[i] != null && (Mathf.Abs (sidewaysSlip) > maxSlipLimitS || Mathf.Abs(forwardSlip) > maxSlipLimitF)) {

				RaycastHit rayHit = new RaycastHit ();

				Vector3 colliderCenterPoint = wheels [i].transform.TransformPoint (wheels [i].center);

				if (Physics.Raycast (colliderCenterPoint, -wheels [i].transform.up, out rayHit, wheels [i].suspensionDistance + wheels [i].radius)) {

					skidmarks[i].transform.position = rayHit.point + (wheels [i].transform.up * wheels [i].suspensionDistance);

				} else {

					skidmarks[i].transform.position = colliderCenterPoint - (wheels [i].transform.up * wheels [i].suspensionDistance);

				}

			} else if (skidmarks[i] != null && (Mathf.Abs (sidewaysSlip) < maxSlipLimitS || Mathf.Abs(forwardSlip) < maxSlipLimitF) ) {

				Destroy(skidmarks[i].gameObject,skidmarks[i].time);
				skidmarks [i] = null;

			}

		}

	}

	//[Command]
	public void Cmd_Drive(float h,float v,bool b){

		float turnSpeed = Mathf.Abs(1/h);

		if(v == 0){

			//b = true;

		}

		if (h > 0) {

			h = 1;

		} else if(h < 0){

			h = -1;

		}

		for (int i = 0; i < wheels.Length - 2; i++) {

			wheels[i].steerAngle = Mathf.Lerp(wheels[i].steerAngle,h * carStats.steerAngle,Time.deltaTime * turnSpeed);

		}

		for (int i = 2; i < wheels.Length; i++) {

			if (b) {

				wheels [i].brakeTorque = carStats.brakeTorque;

			} else {

				wheels [i].brakeTorque = 0;

				wheels[i].motorTorque = v * carStats.torque * 0.5f;


			}

		}

	}

}
