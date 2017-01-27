using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkButtons : NetworkBehaviour {

	public Button btnHost;
	public Button btnClient;
	public InputField inpClient;

	public NetworkManager manager;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		
	}

	public void BtnHost(){

		if(!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null){

			manager.StartHost ();


		}

	}

	public void BtnClient(){

		if (!manager.IsClientConnected () && !NetworkServer.active && manager.matchMaker == null) {

			manager.StartClient ();

		}

	}

	public void StartMatch(){

		manager.StartMatchMaker ();

	}

}
