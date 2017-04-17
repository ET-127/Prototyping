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
	public NetworkDiscovery networkDiscovery;
    public bool startBroadcasting;

	// Use this for initialization
	void Start () {

        
		
	}
	
	// Update is called once per frame
	void Update () {

        
		
	}

	public void BtnHost(){

		manager.StartHost();

	}

	public void BtnClient(){
		
		networkDiscovery.Initialize();
		networkDiscovery.StartAsClient ();

	}

	public void StartMatch(){

		manager.StartMatchMaker ();

	}

}
