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

	// Update is called once per frame
	void Update () {

		if (!isLocalPlayer)
			return;
		
		h = Input.GetAxisRaw("Horizontal") * -s;
		v = Input.GetAxisRaw("Vertical") * s;
		b = Input.GetKey (KeyCode.Space);
		//Send these inputs to the Drive function in Car.cs
		car.Cmd_Drive (h,v,b);
	
	}
}
