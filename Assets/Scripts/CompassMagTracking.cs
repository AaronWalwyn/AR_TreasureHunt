using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class CompassMagTracking : TrackingObject {

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

	IEnumerator InitialiseX()
	{
		DebugTools.Log("*** Test 2");
		Input.compass.enabled = true;
		yield return new WaitForSeconds(1f);
		DebugTools.Log("*** Test 3");
		DebugTools.Log("*** Compass Enabled: " + Input.compass.enabled);
		float x = Input.compass.headingAccuracy;
		while (!player.Intialised) { yield return null; } //Wait for player to be initilised
		DebugTools.Log("*** Test 4");
		DebugTools.Log("*** HEADING ACCURACY: " + x.ToString());
		yield return new WaitForSeconds(1f);

		player.AddedTrackingObject(this);
        initialRotation = Input.compass.magneticHeading;
		init = true;
		DebugTools.Log ("Compass Magnetic Tracking Initialised");
	}

	// Update is called once per frame
	void Update () {

		DebugTools.Track("Compass Heading Accuracy", Input.compass.headingAccuracy); 

		if (player.Intialised && init) {
            currentRotation = new Vector3(0, Input.compass.magneticHeading - initialRotation, 0);
			//currentRotation.y *= 2f;
			DebugTools.Track ("Compass Rotation", currentRotation.y);
            DebugTools.Track ("Compass Magnetic", Input.compass.rawVector.ToString() );
		}
	}


	public override bool Initialise() {
		DebugTools.Log("*** Test 1");
		StartCoroutine(InitialiseX());
		return true;
	}

	public override Vector3 GetPosition() {
		return (currentPos[0] + currentPos[1]) * 0.5f;
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
