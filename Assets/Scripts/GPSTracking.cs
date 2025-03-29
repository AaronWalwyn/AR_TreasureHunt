using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class GPSTracking : TrackingObject {

	const float EARTH_RADIUS = 6371000f;

	//bool init;
	PlayerController player;
    Vector3 currentPos;
	public Vector3 min, max;
	Vector3 start, playArea;


	void Awake() {
		player = this.GetComponent<PlayerController>();
        player.AddedTrackingObject(this);
	}

	// Update is called once per frame
	float lastUpdate = 0.0f, dif;
	void Update () {
		if (player.Intialised && Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running) {
			LocationInfo li = Input.location.lastData;
			if (start == Vector3.zero) {
				lastUpdate = Time.time;
				start = new Vector3 (li.latitude, li.altitude, li.longitude);
			} else {
				currentPos += Measure(li.longitude - start.z, li.latitude - start.x);
				DebugTools.Track ("GPS Position", currentPos.x + " | " + currentPos.z);
				lastUpdate = Time.timeScale;
			}
		}
	}

    public override bool Initialise() {
        return true;
    }

    public override Vector3 GetPosition() {
        return currentPos;
    }

	public float GetPositionWeight{
		get {
			return positionWeight / Time.time - lastUpdate;
		}
	}

    public override Vector3 GetRotation() {
        return Vector3.zero;
    }

    public override void OnDrawGizmos() {
		Gizmos.color = gizmoColour;
		Gizmos.DrawWireCube (Vector3.zero + max - ((max - min) * 0.5f), max - min);
	}

	Vector3 Measure(float deltaLong, float deltaLat) {
		Vector3 m = new Vector3 ();

		m.x = deltaLat * Mathf.Deg2Rad * EARTH_RADIUS;
		m.z = deltaLong * Mathf.Deg2Rad * EARTH_RADIUS;

		return m;
	}

}
