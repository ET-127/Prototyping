using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OverriddenNetworkLobbyManager : NetworkLobbyManager {

	public NetworkDiscovery discovery;

	public override void OnStartHost()
	{

		discovery.Initialize ();
		discovery.StartAsServer ();

	}

	public override void OnStartClient(NetworkClient client)
	{
		discovery.showGUI = false;
	}

	public override void OnStopClient()
	{
		discovery.StopBroadcast();
		discovery.showGUI = true;
	}
}
