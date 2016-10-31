using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Net_PlayerInput : NetworkBehaviour {

	public float h;
	public float v;
	public bool b;

	public float s = 1;
	public Car car;

	// Use this for initialization
	void Start () {

		car = GetComponent<Car> ();
	
	}

	[Command]
	void Cmd_Input(){

		h = Input.GetAxisRaw("Horizontal") * s;
		v = Input.GetAxisRaw("Vertical") * s;
		b = Input.GetKey (KeyCode.Space);

		/*if (Input.GetKey(KeyCode.Q)) {

			h = -0.5f;

		}

		if (Input.GetKey(KeyCode.E)) {

			h = 0.5f;

		}*/

		car.Drive (h,v,b);

	}
	
	// Update is called once per frame
	void Update () {

		if (gameObject.tag != "Player")
			return;


		Cmd_Input ();
	
	}
}
