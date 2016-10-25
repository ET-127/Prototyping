using UnityEngine;
using System.Collections;

public class FollowCar : MonoBehaviour {

	public Transform car;
	public Transform optimum;
	public Vector3 posOffset;

	public Vector3 currentVelPos;
	public float smoothTimePos = 0.3f;

	// Use this for initialization
	void Start () {

		car = GameObject.FindGameObjectWithTag ("Player").transform;
		optimum = car.FindChild ("Optimum").transform;
		optimum.localPosition = posOffset;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		transform.LookAt (car.transform.position /* + car.GetComponent<Rigibody>().velocity */);

		Vector3 wantedPos = new Vector3 (optimum.transform.position.x,posOffset.y,optimum.transform.position.z);

		transform.position = Vector3.SmoothDamp (transform.position, wantedPos, ref currentVelPos, smoothTimePos);

	}
}
