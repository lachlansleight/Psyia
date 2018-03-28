using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class UpdateVivePositions : MonoBehaviour {

	public ViveLocations Locations;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Locations.HeadsetPosition = VRTK_Devices.Position(VRDevice.HMD);
		Locations.LeftControllerPosition = VRTK_Devices.Position(VRDevice.Left);
		Locations.RightControllerPosition = VRTK_Devices.Position(VRDevice.Right);
	}
}
