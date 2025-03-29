using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteraction : Interaction {

	[SerializeField]
	int mapPieceIndex;

	public override string GetActionTypeName {
		get {
			return  "COLLECT";
		}
	}

	public override void Interact() {
		LevelManager.uiManager.ShowMapComponent (mapPieceIndex);
		Destroy (this.GetComponent<BoxCollider> ());
		Destroy (this);
	}
}
