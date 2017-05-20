using UnityEngine;
using System.Collections;

public class ScreenshotCreator : MonoBehaviour {

	public Camera myCam;
	public string directory = "C:\\Users\\Lachlan\\Development\\foliar\\ScreenshotsEtc\\Psyia\\GeneratedScreenshots\\";
	public int superSample = 4;

	// Use this for initialization
	void Start () {
		myCam.enabled = false;

		Debug.Log("Screenshot directory: " + directory);
		if(System.IO.Directory.Exists(directory)) {
			System.IO.Directory.CreateDirectory(directory);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = VRTools.VRInput.GetDevice("ViveHMD").position;
		Vector3 rot = VRTools.VRInput.GetDevice("ViveHMD").rotation.eulerAngles;
		rot.z = 0;
		transform.rotation = Quaternion.Euler(rot);
		

		if(Input.GetKeyDown(KeyCode.S)) {
			Application.CaptureScreenshot(directory + getTimeString() + ".png", superSample);
			Debug.Log("Saved screenshot at " + directory + getTimeString() + ".png" );
		}
		if(Input.GetKeyDown(KeyCode.C)) {
			myCam.enabled = !myCam.enabled;
		}
	}

	string getTimeString() {
		string retString = System.DateTime.Now.ToShortDateString() + "_" + System.DateTime.Now.ToShortTimeString();
		retString.Replace("/", "-");
		retString.Replace(":", "-");
		//return retString;

		return Mathf.RoundToInt(Random.Range(0, 100000000)).ToString();
	}
}
