using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class GameModeManager : NetworkBehaviour {

	[SyncVar]
	public float time = -3;

	public SyncListFloat Times = new SyncListFloat();
	public SyncListBool FinishedCars = new SyncListBool();

	public List<Car> cars = new List<Car>();

	[SyncVar]
	public int finishedCars;

	void Start(){

		cars.AddRange(FindObjectsOfType<Car>());

		for(int i = 0; i < cars.Count; i++) {

			FinishedCars.Add (false);
			Times.Add (0);

		}

	}

    [Server]
	void Timer(){

		for(int i = 0; i < cars.Count; i++) {

			Rpc_SendTime (i);

		}

	}

    [ClientRpc]
	public void Rpc_SendTime(int i){

		if (time >= 0 && cars[i].GetComponent<HoldPos> ().hold && !cars[i].GetComponent<Place> ().finished) {

			cars[i].GetComponent<HoldPos> ().hold = false;

		} else if(cars[i].GetComponent<Place> ().finished){

			cars[i].GetComponent<Net_PlayerInput> ().enabled = false;

		}

		if (!cars[i].GetComponent<Place> ().finished) {

			cars[i].GetComponent<Place> ().time = time;

		} else {

            if (!FinishedCars[i])
            {

                FinishedCars[i] = true;
                Times[i] = cars[i].GetComponent<Place>().time;

                cars[i].GetComponent<Place>().place = finishedCars + 1;
                finishedCars++;

            }
            else { return; }

        }

	}
    [ClientRpc]
    public void Rpc_Finish(int i){

		

	}

	// Update is called once per frame
	void Update () {

        if (!isServer) return;
		time += Time.deltaTime;
		Timer();

	}
}
