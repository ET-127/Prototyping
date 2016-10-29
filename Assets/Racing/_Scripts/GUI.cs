using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class GUI : MonoBehaviour {

	public string AIlevel;
	public GameObject netManager;

	public void AIStart_Button(){

		SceneManager.LoadScene(AIlevel);

	}

	public void MultiStart_Button(){

		netManager.SetActive (true);
		gameObject.SetActive (false);

	}

}
