using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public ARCamera arc;

	public void StartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("level_001");
    }

	public void StartTest() {
		UnityEngine.SceneManagement.SceneManager.LoadScene("level_002");
	}

    public void CameraSettings() {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("launchPreferencesActivity");
        }
#endif
    }
}
