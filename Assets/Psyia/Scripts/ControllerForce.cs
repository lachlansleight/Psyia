using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ForceSource))]
public class ControllerForce : MonoBehaviour {

	public VRDevice TargetDevice;
	public float MinForce;
	public float MaxForce;
	private ForceSource MyForceSource;

	void Awake() {
		MyForceSource = GetComponent<ForceSource>();
	}
	
	void Update () {
		transform.position = VRTK_Devices.Position(TargetDevice);
		transform.eulerAngles = VRTK_Devices.EulerAngles(TargetDevice);
		MyForceSource.StrengthModifier = Mathf.Lerp(MinForce, MaxForce, VRTK_Devices.TriggerAxis(TargetDevice));
	}
}
