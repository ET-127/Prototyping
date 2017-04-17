using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OverriddenNetworkLobbyManager : NetworkLobbyManager {

	public NetworkDiscovery networkDiscovery;
	public override void OnStartHost()
	{
		networkDiscovery.Initialize ();
		networkDiscovery.StartAsServer ();
		

	}

	public override void OnStartClient(NetworkClient client)
	{
		//networkDiscovery.showGUI = false;
		//discovery.StopBroadcast();
	}

	public override void OnStopClient()
	{
		//networkDiscovery.StopBroadcast();
		//networkDiscovery.showGUI = true;
	}
}
