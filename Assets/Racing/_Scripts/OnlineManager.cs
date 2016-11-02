using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class OnlineManager : MonoBehaviour {

	void Awake(){

		if(!NetworkManager.singleton){

			for(int i =0;i < GetComponents<NetworkTransformChild>().Length;i++){

				Destroy (GetComponents<NetworkTransformChild> ()[i]);

			}

			Destroy(GetComponent<NetworkTransform>());
			Destroy (GetComponent<PlayerNetworkManager>());
			Destroy (GetComponent<Net_PlayerInput>());
			Destroy(GetComponent<NetworkIdentity>());
			gameObject.AddComponent<PlayerInput> ();
			gameObject.SetActive (true);

		}

	}
}
 