using UnityEngine;
using System.Collections;

public class FinalUIParent : MonoBehaviour {

	public bool active = false;
	public string currentDevice = "ViveLeft";
	public VRTools.UI.VRUI_Input input;
	public float rotationRate = 0.3f;

	bool moving = false;
	
	[Range(-1f, 1f)] public float debug = 0f;

	// Update is called once per frame
	void Update () {
		Vector3 forwardScale = -transform.forward;
		forwardScale.y = 0;
		forwardScale.Normalize();
		Vector3 displacement = VRTools.VRInput.GetDevice("ViveHMD").position - transform.position;
		displacement.y = 0;
		displacement.Normalize();

		debug = Vector3.Dot(forwardScale, displacement);

		if(Vector3.Dot(forwardScale, displacement) < 0.8f) moving = true;
		else if(Vector3.Dot(forwardScale, displacement) > 0.975f) moving = false;

		if(moving) {
			Quaternion lookRot = Quaternion.LookRotation(VRTools.VRInput.GetDevice("ViveHMD").position - transform.position);
			float yRot = lookRot.eulerAngles.y;
			Quaternion desiredRot = Quaternion.Euler(0f, yRot + 180f, 0f);
			transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, rotationRate);
		}
		return;
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
