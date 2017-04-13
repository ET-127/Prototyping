using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Trigger : MonoBehaviour {

	public bool isTriggerOne;
	public bool isTriggerTwo;

	void OnTriggerEnter(Collider c){

		if (c.transform.root.GetComponent<Place> () == null)
			return;

		if(c.transform.root.GetComponent<Place>().hitTriggerOne && !c.transform.root.GetComponent<Place>().hitTriggerTwo && isTriggerTwo){

			c.transform.root.GetComponent<Place> ().hitTriggerTwo = true;

		} else if(!c.transform.root.GetComponent<Place>().hitTriggerOne && !c.transform.root.GetComponent<Place>().hitTriggerOne && isTriggerOne){

			c.transform.root.GetComponent<Place> ().hitTriggerOne = true;

		}

	}

}
