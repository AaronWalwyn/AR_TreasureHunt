using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TrackingObject : MonoBehaviour {

    [Range(0.0f, 2.0f)]
	public float positionWeight;
	public float rotationWeight;

	public Color gizmoColour;

    abstract public bool Initialise();

	public bool init;
	public bool Initialised {
		get {
			return init;
		}
	}

    abstract public Vector3 GetPosition();

    abstract public Vector3 GetRotation();

    abstract public void OnDrawGizmos();

	public void SetPositionWeightSlider(UnityEngine.UI.Slider _slider) {
		positionWeight = Mathf.Clamp (_slider.value, 0f, 2.0f); 
	}

	public void SetRotationWeightSlider(UnityEngine.UI.Slider _slider) {
		rotationWeight = Mathf.Clamp (_slider.value, 0f, 2.0f); 
	}
}
