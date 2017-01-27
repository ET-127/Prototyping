using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class GUI : MonoBehaviour {

	public int map;
	public GameObject netManager;
	public GameObject startPage;
	public GameObject networkPage;

	public void Start_Button(){

		//netManager.GetComponent<NetworkLobbyManager> ().playScene = SceneManager.GetSceneAt(map).name;
		netManager.SetActive (true);
		startPage.SetActive (false);
		networkPage.SetActive (true);

	}

}
