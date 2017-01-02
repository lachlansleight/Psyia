using UnityEngine;
using System.Collections;

public class FinalUIParent : MonoBehaviour {

	public bool active = false;
	public string currentDevice = "ViveLeft";
	public VRTools.UI.VRUI_Input input;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(active) {
			transform.position = VRTools.VRInput.GetDevice(currentDevice).position;
			transform.rotation = VRTools.VRInput.GetDevice(currentDevice).rotation;

			if(VRTools.VRInput.GetDevice(currentDevice).GetButtonDown("Menu")) {
				active = false;
				transform.GetChild(0).gameObject.SetActive(false);
				return;
			}
		}

		if(VRTools.VRInput.GetDevice("ViveLeft").GetButtonDown("Menu")) {
				active = true;
				transform.GetChild(0).gameObject.SetActive(true);
				currentDevice = "ViveLeft";
				input.deviceName = currentDevice == "ViveLeft" ? "ViveRight" : "ViveLeft";
			} else if(VRTools.VRInput.GetDevice("ViveRight").GetButtonDown("Menu")) {
				active = true;
				transform.GetChild(0).gameObject.SetActive(true);
				currentDevice = "ViveRight";
				input.deviceName = currentDevice == "ViveLeft" ? "ViveRight" : "ViveLeft";
			}
	}
}
