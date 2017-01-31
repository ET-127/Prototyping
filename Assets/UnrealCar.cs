using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnrealCar : MonoBehaviour {

	public Transform[] frontUpperPair;
	public Transform[] frontLowerPair;

	public Transform[] rearUpperPair;
	public Transform[] rearLowerPair;

	public float hoverForce;

	Rigidbody rb;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		Hover ();
	}

	void Hover(){

		Debug.Log (-(Physics.gravity/4f));

		for (int i = 0; i < 2; i++) {

			rb.AddForceAtPosition(Vector3.up * hoverForce,frontLowerPair[i].position);
			//rb.AddForceAtPosition(-Vector3.up * hoverForce,frontUpperPair[i].position);

			rb.AddForceAtPosition(Vector3.up * hoverForce,rearLowerPair[i].position);
			//rb.AddForceAtPosition(-Vector3.up * hoverForce,rearUpperPair[i].position);

		}

	}
}
