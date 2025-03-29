using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour { 
	
    	public static LevelManager instance;
    	public static UIManager uiManager;

    	static bool init = false;
    	public static bool tutorialComplete = false;

    	string time;

    	public PlayerController player;
    	public ARController arc;

	public UnityEngine.UI.Mask[] keyMasks = new UnityEngine.UI.Mask[4];
	public UnityEngine.UI.Text timer;
	float gameTimer;

	int keysFound = 0;


	void Awake() {
		instance = this;
		uiManager = this.GetComponent<UIManager> ();
		keysFound = 0;
	}

    	void Start() {
        	Initilise();
		UpdateWinState ();
		gameTimer = 0;
    	}

	void Update () {
		gameTimer += Time.deltaTime;
		int secs = Mathf.RoundToInt(gameTimer);
		int mins = secs / 60;
		secs -= (mins * 60);
		time = timer.text = mins.ToString("D2") + ":" + secs.ToString("D2");	
	}
		
	public void Initilise () {
		arc.StartAR();
		//player.CalibrateAndStart();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		init = true;
	}

	public int KeyFound() {
		keysFound++;

		UpdateWinState ();

		return keysFound;
	}

	void UpdateWinState() {

		for (int i = 1; i <= keyMasks.Length; i++) {
			keyMasks [i - 1].showMaskGraphic = (i <= keysFound);
		}

		DebugTools.Track ("Keys Found", keysFound);

		if (keysFound >= 5) {
			GameComplete();
		}
	}

	void GameComplete() {
		uiManager.ShowGameOver(time);
	}

	void OnMarkerFound(ARMarker marker) {
		DebugTools.Log (marker.Tag);
	}
}
