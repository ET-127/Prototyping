using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	public float h;
	public float v;
	public bool b;

	public float s;
	public Car car;

	// Use this for initialization
	void Start () {

		car = GetComponent<Car> ();
	
	}
	
	// Update is called once per frame
	void Update () {

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
}
