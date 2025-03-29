using System;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class GyroscopeTracking : TrackingObject {

	PlayerController player;
	Vector3 startPos, currentRotation;
	Vector3[] currentPos = new Vector3[2];
	Vector3 vel;

    void Awake() {
		player = this.GetComponent<PlayerController>();
        player.AddedTrackingObject(this);
		init = true;
        //Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    
	void Start () {
        //startPos = currentPos = this.gameObject.transform.position;
        currentRotation = new Vector3(0, 0, 0);
    }

    void Update() {
		if (player.Intialised && init) {
            
            currentRotation.x -= Input.gyro.rotationRateUnbiased.x;
            currentRotation.y -= Input.gyro.rotationRateUnbiased.y;
            currentRotation.z = 0;
			DebugTools.Track ("Gyro Rotation", currentRotation.ToString ());
        }
		vel = Vector3.Lerp (vel, Vector3.zero, Time.deltaTime * 9.81f);
    }

    public override bool Initialise() {
        return true;
    }

    void Step(float meters) {
        
		float c = Mathf.Sqrt (Input.acceleration.normalized.x * Input.acceleration.normalized.x + Input.acceleration.normalized.z * Input.acceleration.normalized.z);
		currentPos[0] += (Quaternion.Euler(0, currentRotation.y, 0) * Vector3.forward * meters);
        DebugTools.Track("Gyro Step Pos", currentPos[0]);
    }


	void Move(Vector3 _acl) {
		vel += Quaternion.AngleAxis(currentRotation.y, Vector3.up).eulerAngles * _acl.z * Time.deltaTime * 2f;

		currentPos [1] += -vel;
		currentPos [1].y = 0f;
		DebugTools.Track("Gyro Move Pos", currentPos[1]);
	}

    public override Vector3 GetPosition() {
		return (currentPos[0] + currentPos[1]) * 0.5f;
    }

    public override Vector3 GetRotation() {
        return currentRotation;
    }

    public override void OnDrawGizmos() {
		Gizmos.color = gizmoColour;
		Gizmos.DrawSphere(currentPos[0], .5f);
	}
}
