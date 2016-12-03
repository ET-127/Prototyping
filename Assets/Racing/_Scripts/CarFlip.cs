using UnityEngine;
using System.Collections;

public class CarFlip : MonoBehaviour {

	public 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log(transform.up.y);

		if(transform.up.y < -0.6f){

			transform.eulerAngles = new Vector3 (transform.eulerAngles.x,transform.eulerAngles.y,0);
			GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;

		}
	
	}
}
