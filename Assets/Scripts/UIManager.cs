using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	[SerializeField]
	CanvasGroup mapGroup;
	[SerializeField]
	GameObject mapUIIcon;

	[SerializeField]
	CanvasGroup gameOverGroup;

	[SerializeField]
	RectTransform mapBounds, playerIcon;

	void Awake() {
	}

	void Start() {
		HideGroup (mapGroup);
		HideGroup((gameOverGroup));
	}

	void Update() {
		playerIcon.localRotation = Quaternion.Euler( new Vector3(0, 0, -LevelManager.instance.player.transform.localRotation.eulerAngles.y+90f));
		playerIcon.localPosition = new Vector3 (LevelManager.instance.player.transform.position.x * (mapBounds.sizeDelta.x / 192f), LevelManager.instance.player.transform.position.z * (mapBounds.sizeDelta.y / 134f));
	}

	public void ShowMap() {
		DebugTools.Log ("Test string please ignore", false, true);
		ShowGroup (mapGroup);
	}

	public void ShowGameOver(string time) {
		ShowGroup(gameOverGroup);
		gameOverGroup.transform.FindChild("Score").GetComponent<UnityEngine.UI.Text>().text = "Final Time: " + time;
	}

	public void HideMap() {
		HideGroup (mapGroup);
	}

	public void SwitchMap() {
		SwitchGroup (mapGroup);
	}

	public void ShowMapComponent(int index) {
		if (1 <= index && index <= 5) {
			mapGroup.transform.FindChild ("map_" + index).GetComponent<UnityEngine.UI.Image> ().enabled = true;
			mapUIIcon.transform.FindChild ("map_" + index).GetComponent<UnityEngine.UI.Image> ().enabled = true;
		}
	}

	public static void HideGroup(CanvasGroup cg) {
		cg.alpha = 0f;
		cg.interactable = false;
		cg.blocksRaycasts = false;
	}

	public static void ShowGroup(CanvasGroup cg) {
		cg.alpha = 1f;
		cg.interactable = true;
		cg.blocksRaycasts = true;
	}

	public static void SwitchGroup(CanvasGroup cg) {
		if (cg.interactable) HideGroup (cg);
		else ShowGroup(cg);
	}
}
