using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoll : MonoBehaviour {

	public Transform ball;
	public float force;
	public Rigidbody rb;

	// Use this for initialization
	void Start () {

		rb = GetComponentInChildren<Rigidbody> ();

	}
	
	// Update is called once per frame
	void Update () {
		
		rb.AddTorque (transform.right * force * Input.GetAxisRaw("Vertical"));
		rb.transform.Rotate (Vector3.up * Input.GetAxisRaw("Horizontal"),Space.World);
		
	}
}
