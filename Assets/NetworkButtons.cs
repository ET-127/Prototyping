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

        if (startBroadcasting == true)
        {
            if (manager.numPlayers > 1)
            {

                manager.StartHost();

            }

        }
		
	}

	public void BtnHost(){

        /*if(!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null){

            manager.StartHost();


		}*/

        networkDiscovery.StartAsServer();

       

	}

	public void BtnClient(){

        /*if (!manager.IsClientConnected () && !NetworkServer.active && manager.matchMaker == null) {

			manager.StartClient ();

		}*/

        networkDiscovery.StartAsClient();

	}

	public void StartMatch(){

		manager.StartMatchMaker ();

	}

}
