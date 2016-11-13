using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HoldPos : NetworkBehaviour {

	[SyncVar]
	public bool hold = true;
	
	// Update is called once per frame
	void Update () {

		if (hold) {

			GetComponent<Rigidbody> ().velocity = Vector3.zero;

		}

	
	}
}
