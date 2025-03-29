using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class CompassTrueTracking : TrackingObject {

    PlayerController player;
	Vector3[] currentPos = new Vector3[2];
	Vector3 currentRotation;
    float initialRotation;
    //bool init;

    void Awake() {
		init = false;
        	player = this.GetComponent<PlayerController>();
		player.AddedTrackingObject(this);

	}
    
	void Start () {
        currentRotation = Vector3.zero;
    }

    IEnumerator InitialiseX() {
		while(!player.Intialised || Input.location.status != LocationServiceStatus.Running) { yield return null; } //Wait for player to be initilised
		initialRotation = Input.compass.trueHeading;
		init = true;
		DebugTools.Log ("Compass True Tracking Initialised");
    }
	
	// Update is called once per frame
	void Update () {
		if (player.Intialised && init) {
			currentRotation = new Vector3 (0, Input.compass.trueHeading - initialRotation, 0);
			//currentRotation.y *= 2f;
			//DebugTools.Track ("Compass Rotation", currentRotation.y);
			//DebugTools.Track ("Compass True Y", Input.compass.trueHeading);
		}
    }


    public override bool Initialise() {
		StartCoroutine(InitialiseX());
        return true;
    }

    public override Vector3 GetPosition() {
		return (currentPos[0] + currentPos[1] ) * 0.5f;
    }

    public override Vector3 GetRotation() {
		return currentRotation;
    }

    public void Step(float meters) {
		if (player.Intialised && init) {
			currentPos[0] += (Quaternion.Euler (currentRotation) * Vector3.forward * meters);
			DebugTools.Track ("Compass Step Pos", currentPos);
		}
    }

	Vector3 vel;
	void Move(Vector3 _acl) {
		vel += Quaternion.AngleAxis(currentRotation.y, Vector3.up).eulerAngles * _acl.z * Time.deltaTime * 2f;

		currentPos [1] += -vel;
		currentPos [1].y = 0f;
		DebugTools.Track("Gyro Move Pos", currentPos[1]);
	}

    public override void OnDrawGizmos() {
        Gizmos.color = gizmoColour;
		Gizmos.DrawSphere(GetPosition(), .5f);
    }
}
