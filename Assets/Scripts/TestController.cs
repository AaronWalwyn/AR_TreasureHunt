using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : PlayerController {

	public UnityEngine.UI.Text outputText;
	public RectTransform rotationOutput;
	public UnityEngine.UI.Image progress;

	Vector3 sPosition, sRotation;

	// Use this for initialization
	void Start () {
		LevelManager.tutorialComplete = true;
	}

	public void Calibrate() {
		sPosition = sRotation = Vector3.zero;

		foreach (TrackingObject to in trackingObjects) {
			to.Initialise();

			to.positionWeight = 0f;
			to.rotationWeight = 0f;
		}

		if (!this.Intialised) {
			CalibrateAndStart ();
			outputText.text = "Starting Calibration...";
		}
	}

	public void Sample() {
		if(!runningSample) StartCoroutine (StartSample ());
	}

	bool runningSample = false;
	IEnumerator StartSample() {
		runningSample = true;
		int samples = Mathf.RoundToInt ((1f / Time.deltaTime) * 10f);
		Vector3 finalRotation = Vector3.zero;
		Vector3 finalPosition = Vector3.zero;
        float c = 0, s = 0;
		Vector3 samplePosition, sampleRotation;

		for (int i = 0; i < samples; i++) {

			SampleTrackingObjects (out sampleRotation, out samplePosition);

            c += Mathf.Cos(sampleRotation.y * Mathf.Deg2Rad);
            s += Mathf.Sin(sampleRotation.y * Mathf.Deg2Rad);

			/*finalPosition += samplePosition;
			finalRotation += sampleRotation;*/

			float p = (1f / (float)samples) * (i + 1f);

			progress.color = Color.white * p;

			yield return null;
		}

        float y = Mathf.Atan((s / samples) / (c / samples));
        sRotation = new Vector3(0, y * Mathf.Rad2Deg, 0);

        while (sRotation.y > 360f) sRotation.y -= 360f;
        while (sRotation.y < 0f) sRotation.y += 360f;

		sPosition = finalPosition /= samples;

		runningSample = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.Intialised) {
			string s = "";
			Vector3 rotation = Vector3.zero;
			Vector3 position = Vector3.zero;

			SampleTrackingObjects (out rotation, out position);

			rotationOutput.localRotation = Quaternion.Euler(new Vector3(0, 0, -rotation.y));

			s += Time.time.ToString () + "\n";

			foreach (TrackingObject to in trackingObjects) {
				s += to.GetType ().ToString () + " Rotation: " + to.GetRotation () * to.rotationWeight + "\n";
				s += to.GetType ().ToString () + " Position: " + to.GetPosition () * to.positionWeight + "\n";
			}

			s += "\n";
				
			s += "Average Rotation: " + rotation.ToString () + "\n";
			s += "Average Position: " + position.ToString () + "\n";

			s += "\n";

			s += "Sample Rotation: " + sRotation.ToString () + "\n";
			s += "Sample Position: " + sPosition.ToString () + "\n";

			outputText.text = s;
		}
	}
}
