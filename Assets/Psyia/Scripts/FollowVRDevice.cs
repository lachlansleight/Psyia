using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowVRDevice : MonoBehaviour {

	public VRDevice TargetDevice;
	
	void Update () {
		transform.position = VRTK_Devices.Position(TargetDevice);
		transform.eulerAngles = VRTK_Devices.EulerAngles(TargetDevice);
	}
}
