using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkManagerAddOns : NetworkBehaviour {

	// Use this for initialization
	void Start () {

		Debug.Log (NetworkManager.singleton.networkAddress);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
