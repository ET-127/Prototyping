﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Place : NetworkBehaviour {

	[SyncVar]
	public bool hitTriggerOne;
	[SyncVar]
	public bool hitTriggerTwo;
	[SyncVar]
	public bool finished;
	[SyncVar]
	public int place = 1;

	public float time;
	public string text;

	public Text timer;
	public Text victoryScreen;
	public Text gearText;
	public Text speedText;
	public Slider gearSlider;

	public GameObject victoryPanel;

	public string victoryMessage;

	public CarStats carStats;

	void Start(){

		if (!isLocalPlayer)
			return;

		victoryPanel = GameObject.FindGameObjectWithTag ("Victory Panel");

		victoryScreen = GameObject.FindGameObjectWithTag ("Victory Text").GetComponent<Text> ();
		timer = GameObject.FindGameObjectWithTag ("Timer").GetComponent<Text>();
		gearText = GameObject.FindGameObjectWithTag ("Gear").GetComponent<Text> ();
		speedText = GameObject.FindGameObjectWithTag ("Speed").GetComponent<Text> ();
		gearSlider = GameObject.FindGameObjectWithTag ("Speed Slider").GetComponent<Slider> ();

		carStats = GetComponent<CarStats>();

		victoryPanel.SetActive (false);
	}

	[Command]
	void Cmd_SendFinished(){

		finished = true;

	}

	void Update () {

		if (!isLocalPlayer)
			return;

		if (Input.GetKey (KeyCode.M)) {

			hitTriggerOne = true;
			hitTriggerTwo = true;

		}

		if (hitTriggerOne && hitTriggerTwo) {

			gearText.text = "";
			speedText.text = "";
			gearSlider.value = 0;

			timer.text = time.ToString ("F2");
			Cmd_SendFinished ();

		} else {

			gearText.text = carStats.currentGear.ToString ();
			speedText.text = carStats.currentSpeed.ToString ("F1") + " mph";

			if (carStats.currentGear > 0) {

				gearSlider.value = (carStats.currentSpeed - carStats.gearTopSpeeds [carStats.currentGear - 1]) / (carStats.gearTopSpeeds [carStats.currentGear] - carStats.gearTopSpeeds [carStats.currentGear - 1]);

			} else {
				
				gearSlider.value = carStats.currentSpeed / carStats.gearTopSpeeds [carStats.currentGear];

			}

		}

		float min = 0;
		float sec = 0;

		min = (time / 60);
		sec = 60 * (min - Mathf.Floor(min));

		if (!finished) {

			text = Mathf.Floor(min).ToString () + ":" + sec.ToString ("F2");

			timer.text = text;
			 
		} else {

			victoryPanel.SetActive (true);
			victoryScreen.text = "YOlO";

			victoryMessage = place.ToString();

			if (place == 1) {

				victoryMessage += "st!";

			} else if (place == 2) {

				victoryMessage += "nd!";

			} else if (place == 3) {

				victoryMessage += "rd!";

			} else if(place == 4){

				victoryMessage += "th!";

			}

			victoryScreen.text = victoryMessage;


		}

	}
}
