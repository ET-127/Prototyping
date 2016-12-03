using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class GUI : MonoBehaviour {

	public int map;
	public GameObject netManager;

	public void Start_Button(){

		//netManager.GetComponent<NetworkLobbyManager> ().playScene = SceneManager.GetSceneAt(map).name;
		netManager.SetActive (true);
		gameObject.SetActive (false);

	}

}
