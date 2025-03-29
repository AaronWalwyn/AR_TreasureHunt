using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class StepTracking : MonoBehaviour {

    PlayerController player;
    int steps = 0;
    bool count;
    float min = float.MaxValue, max = float.MinValue, avg = 1.5f, threshhold;

    void Awake() {
        player = this.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        if (player.Intialised) {

            threshhold = Mathf.Lerp((avg + min) / 2f, Input.acceleration.magnitude, Time.deltaTime * max);

            if (Input.acceleration.magnitude < min) {
                min = Input.acceleration.magnitude;
            }

            if (Input.acceleration.magnitude > max) {
                max = Input.acceleration.magnitude;
            }

            if (count) {
                if (Input.acceleration.magnitude > avg) {
                    avg = ((avg * steps) + Input.acceleration.magnitude) / (steps + 1);
                }

                if (Input.acceleration.magnitude > threshhold) {
                    count = false;
                    TakeStep();
                    steps++;
                }
            }

            if (!count && Input.acceleration.magnitude < 0.8f) {
                count = true;
            }
        }
    }

    void TakeStep() {
		if (LevelManager.tutorialComplete)
        	SendMessage("Step", 0.71f, SendMessageOptions.DontRequireReceiver);
    }
}
