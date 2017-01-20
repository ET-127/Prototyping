using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Net_PlayerInputMouse : NetworkBehaviour {

	public float h;
	public float v;
	public bool b;

	public float s = 1;
	public Car car;

	// Use this for initialization
	void Start () {

		car = GetComponent<Car> ();

		Cursor.lockState = CursorLockMode.Confined;

	}

	// Update is called once per frame
	void Update () {

		if (!isLocalPlayer)
			return;

		h = (-(Input.mousePosition.x / Screen.width) + 0.5f) * 2 * s;
		Debug.Log (h);
		v = Input.GetAxisRaw("Fire2");
		b = Input.GetMouseButtonDown (0);

		/*if (Input.GetKey(KeyCode.Q)) {

			h *= 0.5f;

		}

		if (Input.GetKey(KeyCode.E)) {

			h *= 0.5f;

		}*/

		car.Cmd_Drive (h,v,b);

	}
}
