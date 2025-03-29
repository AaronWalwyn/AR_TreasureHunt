using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteraction : Interaction {

	int remainingInteraction = 1;

	public override string GetActionTypeName {
		get {
			return (remainingInteraction > 0) ? "Collect Key" : "";
		}
	}

	public override void Interact() {
		if (remainingInteraction > 0) {
			remainingInteraction--;

			LevelManager.instance.KeyFound ();

			DebugTools.Log("Key Collected");
			Destroy (this.gameObject);
		}
	}
}
