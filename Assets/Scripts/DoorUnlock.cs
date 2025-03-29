using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlock : Interaction {

    public DoorInteraction[] doors;

    public override string GetActionTypeName {
        get {
            return "PRESS";
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public override void Interact() {
        foreach (DoorInteraction di in doors) {
            di.UnlockDoor();
        }
    }
}
