using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutCalibInteraction : Interaction {

	static int remaininTuts = 4;

	public override string GetActionTypeName {
		get {
			return  "CALIBRATE";
		}
	}

	public override void Interact() {
		LevelManager.tutorialComplete = (--remaininTuts == 0);

		Destroy (this.gameObject);
	}
}
