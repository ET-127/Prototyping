using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Trigger : MonoBehaviour {

	public bool isTriggerOne;
	public bool isTriggerTwo;

	void OnTriggerEnter(Collider c){

		//If the object thats hit the collider does not keep track of their progress
		if (c.transform.root.GetComponent<RaceProgress> () == null)
			return;
		//If the car has hit the first trigger
		if(c.transform.root.GetComponent<RaceProgress>().hitTriggerOne && !c.transform.root.GetComponent<RaceProgress>().hitTriggerTwo && isTriggerTwo){

			c.transform.root.GetComponent<RaceProgress> ().hitTriggerTwo = true;

		} else if(!c.transform.root.GetComponent<RaceProgress>().hitTriggerOne && !c.transform.root.GetComponent<RaceProgress>().hitTriggerTwo && isTriggerOne){

			c.transform.root.GetComponent<RaceProgress> ().hitTriggerOne = true;

		}

	}

}
