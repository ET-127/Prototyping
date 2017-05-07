using UnityEngine;
using System.Collections;

public class Net_PlayerInput : MonoBehaviour {

	public float h;// The horizontal axis A and D/ Left and Right Arrow
	public float v;// The veritcal axis W and S/Up and Down Arrow
	public bool b;// Does the player want to break?

	public float s = 1;// The sensitivity
	public Car car;//The Car script

	// Use this for initialization
	void Start () {

		//Get the car script from the car object
		car = GetComponent<Car> ();
	
	}

	// Update is called once per frame
	void Update () {

		// Multiply the vertical axis by the sensitivity appropriately
		h = Input.GetAxisRaw("Horizontal") * -s;
		v = Input.GetAxisRaw("Vertical") * s;

		// Set b to whether or not the SPACE key is being pressed
		b = Input.GetKey (KeyCode.Space);

		//Send this data to the Drive function in Car.cs
		car.Drive (h,v,b);
	
	}
}
