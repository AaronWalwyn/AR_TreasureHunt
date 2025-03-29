using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public LayerMask mask;
    public UnityEngine.UI.Image cursor;
    public UnityEngine.UI.Text cursorAction;
    public Transform sightPos;
    Interaction action;

    [Range(0.0f, 10.0f)]
    public float rotationSpeed, movementSpeed;

    public Camera cam;
    Vector3 rotation, position;
	bool init;

    public List<TrackingObject> trackingObjects = new List<TrackingObject>();
    public void AddedTrackingObject(TrackingObject _to) {
        if (!trackingObjects.Contains(_to))
            trackingObjects.Add(_to);
    }

	public Vector3 direction;
	public Vector3 startPos;
	public Vector3 GetRotation {
		get {
			return transform.forward;
		}
	}

	public bool Intialised {
		get {
			return init && Input.location.status == LocationServiceStatus.Running; 
		}
	}

	// Use this for initialization
    void Awake() {
		init = false;
    }

	public void Start () {
        CalibrateAndStart();
    }

	public IEnumerator StartGPS () {

		DebugTools.Log ("Starting Location Services");
		if (!Input.location.isEnabledByUser) {
			DebugTools.Log ("Location Services Not Enabled By User");
			yield break;
		}

		if (Input.location.status == LocationServiceStatus.Stopped)
			Input.location.Start (1f, 1f);

        while (Input.location.status == LocationServiceStatus.Initializing) {
            yield return null;
        }

        if (Input.location.status == LocationServiceStatus.Failed) {
			DebugTools.Log ("Location Services Initialisation Failed");
			//Application.Quit ();
		} else if (Input.location.status == LocationServiceStatus.Running) {
			DebugTools.Log ("Location Services Initialised");

			Input.compass.enabled = true;
			Input.compensateSensors = true;
			Input.gyro.enabled = true;

			rotation = new Vector3(0, 0, 0);
			startPos = transform.position;

			foreach (TrackingObject to in trackingObjects) to.Initialise();
			if (cam != null) {
				cam.transform.localPosition = new Vector3 (0, 1f, 0.33f);
				cam.clearFlags = CameraClearFlags.Depth;
			}
			init = true;
			DebugTools.Log ("Calibrate & Start Complete");
        }
        yield return null;
    }

    public void CalibrateAndStart() {
        if (!init) {
            StartCoroutine(StartGPS());

        }
    }

    // Update is called once per frame
    void Update() {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward); ;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 3f, mask.value)) {
            //cursor.color = new Color(1f, 0, 0, 0.2f);
            action = hit.transform.GetComponent<Interaction>();
            if (action != null) {
                cursorAction.text = "[ " + action.GetActionTypeName + " ]";
            }
        } else {
            if (action != null || cursorAction.text != "") {
                //cursor.color = new Color(1f, 1f, 1f, 0.2f);
                cursorAction.text = "";
                action = null;
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && action != null) {
            action.Interact();
        }

        if (init) {
            
            SampleTrackingObjects(out rotation, out position);

		    DebugTools.Track ("Player Rotation", rotation.ToString ());

			rotation.x = Quaternion.FromToRotation (-Vector3.up, Input.gyro.gravity).eulerAngles.x;
			//rotation.z = Quaternion.FromToRotation (-Vector3.right, Input.gyro.gravity).eulerAngles.z;

			cam.transform.localRotation = Quaternion.Euler (new Vector3 (rotation.x, 0, 0));
            cam.transform.localPosition = new Vector3(0, 0.5f, 0.33f);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation.y, 0), Time.deltaTime * rotationSpeed);
            transform.position =  Vector3.Lerp(transform.position, position, Time.deltaTime * movementSpeed);
        }


	}

    public void SampleTrackingObjects(out Vector3 rotation, out Vector3 position) {
		rotation = Vector3.zero;
		position = Vector3.zero;
        float rWeight = 0, pWeight = 0;
        float c = 0, s = 0;

        foreach(TrackingObject to in trackingObjects) {
			if (!to.Initialised) continue;
				
			position += to.GetPosition() * to.positionWeight;
            pWeight += to.positionWeight;

            rotation += to.GetRotation() * to.rotationWeight;
            rWeight += to.rotationWeight;
           
            /*if (to.rotationWeight > 0) {
		        c += Mathf.Cos(to.GetRotation().y * Mathf.Deg2Rad);
		        s += Mathf.Sin(to.GetRotation().y * Mathf.Deg2Rad);
                rWeight += 1f;
            }*/
        }

        //float y = Mathf.Atan((s / rWeight) / (c / rWeight));
        //rotation = new Vector3(0, y * Mathf.Rad2Deg, 0);

		if (rWeight != 0) rotation /= rWeight;// : Vector3.zero;
		if (pWeight != 0) position /= pWeight;// : Vector3.zero;

        while (rotation.y > 360f) rotation.y -= 360f;
        while (rotation.y < 0f) rotation.y += 360f;
    }
}
