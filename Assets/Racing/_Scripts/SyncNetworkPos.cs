using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SyncNetworkPos : NetworkBehaviour {

	[SyncVar]
	public Vector3 syncPos;
	public Vector3 lastPos;

	[SyncVar]
	public Vector3 syncRot;
	public Vector3 lastRot;

	public Transform transform;
	public Quaternion rotation;

	public float speedPos;
	public float speedRot;

	public float posThreshold;
	public float rotThreshold;

	void Start(){

		transform = GetComponent<Transform> ();
		syncPos = NetworkManager.singleton.GetStartPosition().position;

	}

	void Update () {

		LerpPosition ();
		LerpRotation ();
		TransmitPosition ();
		TransmitRotation ();
	
	}

	void LerpPosition (){

		if (!isLocalPlayer) {

			transform.Translate((syncPos - transform.position) * speedPos);

		}

	}

	void LerpRotation (){

		if (!isLocalPlayer) {

			transform.Rotate((syncRot - transform.eulerAngles) * speedRot);

		}

	}

	[Command]
	void Cmd_SendPosToServer(Vector3 pos){

		syncPos = pos;

	}

	[Command]
	void Cmd_SendRotToServer(Vector3 rot){

		syncRot = rot;

	}

	[Client]
	void TransmitPosition () {

		if(isLocalPlayer && Vector3.Distance(transform.position,lastPos) > posThreshold){

			Cmd_SendPosToServer (transform.position);

		}

	}

	[Client]
	void TransmitRotation () {

		if(isLocalPlayer && Vector3.Angle(transform.rotation.eulerAngles,lastRot) > rotThreshold){

			Cmd_SendRotToServer (transform.rotation.eulerAngles);

		}

	}
}
