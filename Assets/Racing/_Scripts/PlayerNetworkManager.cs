using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNetworkManager : NetworkBehaviour {
	
	public GameObject[] spawnPrefabs;

	// Use this for initialization
	void Start () {

		if (!this.isLocalPlayer) {
			
			gameObject.tag = "Online";
			return;

		}

		for (int i = 0; i < spawnPrefabs.Length; i++) {

			Instantiate (spawnPrefabs [i]);

		}

	}

}
