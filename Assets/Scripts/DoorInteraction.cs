using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : Interaction {

    Animator anim;
    bool locked;

    public override string GetActionTypeName {
        get {
            return (locked) ? "LOCKED" : "OPEN DOOR";
        }
    }

    void Awake() {
        anim = this.GetComponent<Animator>();
    } 

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchDoor() {
        locked = !locked;
    }

    public void LockDoor() {
        locked = true;
    }

    public void UnlockDoor() {
        locked = false;
    }

    public override void Interact() {
        if (!locked && anim != null) {
            anim.SetBool("open", true);
        }
    }
}
