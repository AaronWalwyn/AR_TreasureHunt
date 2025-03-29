using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class MomentumTracking : MonoBehaviour {

	PlayerController player;

	void Awake() {
		player = this.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.Intialised && LevelManager.tutorialComplete) {
			SendMessage("Move", Input.gyro.userAcceleration, SendMessageOptions.DontRequireReceiver);
		}
	}
}
