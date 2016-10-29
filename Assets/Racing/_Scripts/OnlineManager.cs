using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class OnlineManager : MonoBehaviour {

	void Awake(){

		if(!NetworkManager.singleton){

			Destroy(GetComponent<NetworkTransform>());
			Destroy (GetComponent<PlayerNetworkManager>());
			Destroy (GetComponent<Net_PlayerInput>());
			Destroy(GetComponent<NetworkIdentity>());
			gameObject.AddComponent<PlayerInput> ();
			return;

		}

	}
}
 