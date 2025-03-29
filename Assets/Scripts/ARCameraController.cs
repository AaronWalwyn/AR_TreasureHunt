using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DebugTools.Log("Testing 123");
	}

    void OnMarkerLost(ARMarker marker) {
    }
    void OnMarkerFound(ARMarker marker) {
    }


    /*void OnMarkerTracked(ARMarker marker) {
        DebugTools.Log("Test Three");
        DebugTools.Log(marker.NFTDataName, false, true);
    }*/
}
