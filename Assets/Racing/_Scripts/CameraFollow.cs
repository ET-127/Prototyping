using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform player;
	public Vector3 offset;
	public float smoothTime;
	Vector3 refVel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = Vector3.SmoothDamp (transform.position,player.position + offset,ref refVel,smoothTime);
		
	}
}
