using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SyncNetworkPos : NetworkBehaviour {

	[SyncVar]
	public Vector3 syncPos;
	public Vector3 lastPos;

	[SyncVar]
	public Quaternion syncRot;
	public Quaternion lastRot;

	public Transform transform;
	public Quaternion rotation;

	public float lerpRatePos;
	public float lerpRateRot;

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

			transform.position = Vector3.Lerp (transform.position,syncPos,Time.fixedDeltaTime * lerpRatePos);

		}

	}

	void LerpRotation (){

		if (!isLocalPlayer) {

			transform.rotation = Quaternion.Slerp (transform.rotation,syncRot,Time.fixedDeltaTime * lerpRateRot);

		}

	}

	[Command]
	void Cmd_SendPosToServer(Vector3 pos){

		syncPos = pos;

	}

	[Command]
	void Cmd_SendRotToServer(Quaternion rot){

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

		if(isLocalPlayer && Vector3.Angle(transform.rotation.eulerAngles,lastRot.eulerAngles) > rotThreshold){

			Cmd_SendRotToServer (transform.rotation);

		}

	}
}
