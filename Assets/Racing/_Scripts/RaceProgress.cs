using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class RaceProgress : MonoBehaviour {

	public bool hitTriggerOne;//Has the car hit the first trigger?
	public bool hitTriggerTwo;//Has the car hit the second trigger?
	public bool finished;//Has the car finished the race?

	public float time;//The time taken for the car to complete the race
	public string timerText;// The time taken for the car to complete the race in minutes and seconds

	public Text timer;//The UI timer
	public Text victoryScreen;//The UI victory screen
	public Text gearText;//The UI gearbox
	public Text speedText;//The UI speedometer 
	public Slider gearSlider;//The UI gear slider

	public GameObject victoryPanel;//The victory panel object

	public string victoryMessage;//The messaged to be displayed once the race is over

	public Car car;// The car script

	void Start(){
		//Find all the appropriate UI elements
		victoryPanel = GameObject.FindGameObjectWithTag ("Victory Panel");
		victoryScreen = GameObject.FindGameObjectWithTag ("Victory Text").GetComponent<Text> ();
		timer = GameObject.FindGameObjectWithTag ("Timer").GetComponent<Text>();
		gearText = GameObject.FindGameObjectWithTag ("Gear").GetComponent<Text> ();
		speedText = GameObject.FindGameObjectWithTag ("Speed").GetComponent<Text> ();
		gearSlider = GameObject.FindGameObjectWithTag ("Speed Slider").GetComponent<Slider> ();

		//Find the car script
		car = GetComponent<Car>();

		//Hide the victory panel object
		victoryPanel.SetActive (false);
	}

	void Update () {

		//Update the timer
		time += Time.deltaTime;

		//If both triggers have been hit
		if (hitTriggerOne && hitTriggerTwo) {

			gearText.text = "";
			speedText.text = "";
			gearSlider.value = 0;

			//Pause the UI timer
			timer.text = time.ToString ("F2");

			//Consider the player finished
			finished = true;

		} else {

			//Display the current gear on the UI gearbox
			gearText.text = car.currentGear.ToString ();

			//Display the current speed on the UI speedometer
			speedText.text = car.currentSpeed.ToString ("F1") + " mph";

			//Set the value of the gear slider depending on the current gear
			if (car.currentGear > 0) {

				gearSlider.value = (car.currentSpeed - car.gearTopSpeeds [car.currentGear - 1]) / (car.gearTopSpeeds [car.currentGear] - car.gearTopSpeeds [car.currentGear - 1]);

			} else {
				
				gearSlider.value = car.currentSpeed / car.gearTopSpeeds [car.currentGear];

			}

		}

		//Minutes
		float min = 0;

		//Seconds
		float sec = 0;

		//Convert the time from second to minutes and seconds
		min = (time / 60);
		sec = 60 * (min - Mathf.Floor (min));

		//If the car hasn't finished the race
		if (!finished) {

			//Set the UI timer to the current time taken in minutes and seconds
			timerText = Mathf.Floor(min).ToString () + ":" + sec.ToString ("F2");
			timer.text = timerText;

		//If the car has finished the race
		} else {

			//Unhide the victory panel and display a victory message!
			victoryPanel.SetActive (true);
			victoryScreen.text = "Time: " + timerText;

		}

	}
}
