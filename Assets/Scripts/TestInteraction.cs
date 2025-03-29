using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteraction : Interaction {

    public override string GetActionTypeName {
        get {
            return "Interact";
        }
    }

    public override void Interact() {
        DebugTools.Log("Test Interaction");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
