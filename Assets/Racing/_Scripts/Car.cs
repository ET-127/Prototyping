using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Car : MonoBehaviour {

	public WheelCollider[] wheels; //List of wheel colliders owned by the car
	public List<MeshRenderer> wheelMeshes = new List<MeshRenderer>(); //List of rendered wheels
	public float topSpeed; // The fastest the car can go
	public float torque; // The torque the wheels are currently exerting
	public float engineTorque; // The base torque of the engine
	public float brakeTorque; // The ammount of torque used to brake
	public float steerAngle; // The angle at which the wheels turn when steering
	public float steerTime;// How long it takes to turn the wheel to its steer angle

	public float frontFrictionStiffness;// The stiffness of the steering on the front wheels of the car
	public float rearFrictionStiffness;// The stiffness of the steering on the rear wheels of the car

	public float springFront;// The spring force on the front wheels of the car
	public float springBack;// The spring force on the rear wheels of the car
	public float dampingFront;// The amount of spring damping on the front wheels of the car
	public float dampingBack;// The amount of spring damping on the rear wheels of the car

	public Vector2 ExtremumF = new Vector2(0.4f,1); // The co-ordinate of the Extrumum for forward friction
	public Vector2 AsymptoteF = new Vector2(0.8f,0.5f);// The co-ordinate of the Asymptote for the forward friction
	public Vector2 ExtremumS = new Vector2(0.2f,1);// The co-ordinate of the Extrumum for the sideways friction
	public Vector2 AsymptoteS = new Vector2(0.5f,0.75f);// The co-ordinate of the Asymptote for the sideways friction

	public float downForceModifier;// How many times larger should downforcebe thean drag?

	public int numOfGears;// The number of gears the car has
	public float[] gearRatios;//An array of the gear ratios of the car
	public float[] gearTopSpeeds;// Each gear ratios respective top speed
	public float transmission;//The gear transmission
	public int currentGear = 1;//The car's current gear

	public float wheelDiameter;//The diameter of the wheel

	public float currentSpeed;//The car's current speed

	public float milestokilo = 2.23694f;

	public AnimationCurve forwardFrictionCurve;// A curve describing the behaviour of the forward tire friction
	public AnimationCurve sideFrictionCurve;// A curve describing the behaviour of the sideways tire friction

    public AudioSource carRev;//The car rev sound effect
    public AudioSource carIdle;//The car idling sound effect
    public float engineDecay;// The rev sound's current decay
    public float engineDecayTime;//The time taken for the rev sound to decay

	public Vector3 downForce;//The downforce
    public bool isIdle;//Is the car idling

	float steerRefVel;//The reference velocity for steering
	Rigidbody rb;//The Rigidbody component

	public bool hold = true;// Should the car be held?

	// Use this for initialization
	void Start () {

		wheels = GetComponentsInChildren<WheelCollider> ();
		wheelDiameter = wheels[0].radius * 2;
		rb = GetComponent<Rigidbody> ();

		float c = wheelDiameter * Mathf.PI;

		for(int i = 0; i < gearRatios.Length;i++) {

			gearTopSpeeds[i] = ((engineTorque / 60) / (gearRatios[i] * transmission)) * c * milestokilo;

		}

		topSpeed = gearTopSpeeds[gearTopSpeeds.Length - 1];
			
		for (int i = 0; i < GetComponentsInChildren<MeshRenderer> ().Length; i++) {

			wheelMeshes.Add(GetComponentsInChildren<MeshRenderer> () [i]);

		}

		wheelMeshes.RemoveAt (0);

		SideFriction ();
		ForwardFriction ();

	}

	void Hold () {

		//Should the car be held in place?
		if (hold) {

			GetComponent<Rigidbody> ().velocity = Vector3.zero;

		}

	}

	//Flip the car if it falls onto its back
	void CarFlip () {

		if(transform.up.y < -0.6f){

			transform.eulerAngles = new Vector3 (transform.eulerAngles.x,transform.eulerAngles.y,0);
			GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;

		}

	}

    void CarAudio()
    {

        if (currentGear > 0)
        {

            carRev.pitch = 0.5f + (currentSpeed) / (gearTopSpeeds[currentGear]);

        }
        else
        {

            carRev.pitch = 0.5f + (currentSpeed / gearTopSpeeds[currentGear]);

        }

        carRev.volume = Mathf.SmoothDamp(carRev.volume, Input.GetAxisRaw("Vertical"),ref engineDecay,engineDecayTime);

		if (carRev.volume < carIdle.volume && !isIdle)
        {
            carIdle.mute = false;
            carRev.mute = true;
			isIdle = true;

            //carIdle.pitch = carRev.pitch;

        } else if (carRev.volume > carIdle.volume){
            carIdle.mute = true;
            carRev.mute = false;
			isIdle = false;

            carIdle.pitch = 0.75f;

        }
    }

	void FrontFrictionGraph(){

		//Create a graph with three points one at the origin one at the Extremum and one at the Asymptote
		forwardFrictionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(ExtremumF.x, ExtremumF.y),new Keyframe(AsymptoteF.x, AsymptoteF.y));

	}

	void SideFrictionGraph(){

		//Create a graph with three points one at the origin one at the Extremum and one at the Asymptote
		sideFrictionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(ExtremumS.x, ExtremumS.y),new Keyframe(AsymptoteS.x, AsymptoteS.y));

	}

	void ForwardFriction(){

		//Store the current friction data for the front and rear wheels in two variables
		WheelFrictionCurve friction = wheels[0].forwardFriction;

		//Change the extremum's co-ordinate
		friction.extremumSlip = ExtremumF.x;
		friction.extremumValue = ExtremumF.y;

		//Change the asymptote's co-ordinate
		friction.asymptoteSlip = AsymptoteF.x;
		friction.asymptoteValue = AsymptoteF.y;

		for (int i = 0; i < wheels.Length; i++) {

			wheels[i].forwardFriction = friction;

		}
			
	}

	void SideFriction(){

		//Store the current friction data for the front and rear wheels in two variables
		WheelFrictionCurve friction = wheels[0].sidewaysFriction;

		//Change the extremum's co-ordinate
		friction.extremumSlip = ExtremumS.x;
		friction.extremumValue = ExtremumS.y;

		//Change the asymptote's co-ordinate
		friction.asymptoteSlip = AsymptoteS.x;
		friction.asymptoteValue = AsymptoteS.y;

		for (int i = 0; i < wheels.Length; i++) {

			wheels[i].sidewaysFriction = friction;

		}

	}

	void SwitchGear(){
		
		//Is the car going faster than its current maximum?
		if (currentSpeed >= gearTopSpeeds [currentGear]){

			//Then move up a gear
			currentGear++;

		//Is the speed of the car lower than the maximum speed of the gear ratio below?
		} else if(currentGear - 1 > -1 && currentSpeed <= gearTopSpeeds [currentGear - 1]){

			//Then move down a gear
			currentGear--;

		}

		//Restrict the current Gear to be no smaller than 0 nor bigger then the number of gears
		currentGear = Mathf.Clamp(currentGear,0 , numOfGears - 1);

	}
		
	void Update () {

		currentSpeed = rb.velocity.magnitude * milestokilo;

		SideFrictionGraph ();
		FrontFrictionGraph ();
		SwitchGear ();
		SpeedControl ();
		//CorrectWheelPos ();
		DownForce ();
        CarAudio();
		Hold ();

    }

	void DownForce(){

		downForce = Vector3.down * (0.5f * downForceModifier * rb.drag * (currentSpeed * currentSpeed));

        rb.AddForce(downForce);

	}

	void SpeedControl(){

		//Is the car going faster than top speed?
		if (rb.velocity.magnitude >= topSpeed) {

			//Set the current velocity to the speed cap
			rb.velocity = rb.velocity.normalized * topSpeed;

		}

	}

	void CorrectWheelPos(){

		//The position of the wheel collider
		Vector3 wheelPos;

		//The rotation of the wheel collider
		Quaternion wheelRot;

		//For each wheel collider
		for (int i = 0; i < wheels.Length; i++) {

			//Get the wheel collider's position and rotation
			wheels [i].GetWorldPose (out wheelPos, out wheelRot);

			//Set the wheel mesh's position to that of the respective wheel collider 
			wheelMeshes [i].transform.position  = wheelPos;

			//Set the wheel mesh's rotation to that of the respective wheel collider 
			wheelMeshes [i].transform.rotation = Quaternion.Euler(wheelRot.eulerAngles);

		}

	}

	public void Drive(float h,float v,bool b){

		//Calculate the amount of torque provided by the current gear
		torque = engineTorque * transmission * gearRatios [currentGear];

		//For each front wheel
		for (int i = 0; i < wheels.Length - 2; i++) {
			
			float angle = 0;

			// Multiply the horizontal axis by the steer angle to get the desired angle then clamp the desired angle between steerAngle and -steerAngle
			angle = Mathf.Clamp (h * steerAngle,-steerAngle,steerAngle);

			//Gradualy steer the tire to the desired angle
			wheels[i].steerAngle = Mathf.SmoothDampAngle(wheels[i].steerAngle,-angle,ref steerRefVel,steerTime);

		}

		//For each rear wheel
		for (int i = 2; i < wheels.Length; i++) {

			//If the player wants to brake
			if (b) {

				//Set the wheels' brake torque
				wheels [i].brakeTorque = brakeTorque;

			} else {

				//Set the wheels' brake torque to 0
				wheels [i].brakeTorque = 0;

				//Set the wheels' torque
				wheels[i].motorTorque = v * torque * 0.5f;


			}

		}

	}

}
