using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ForceSource))]
public class TriggerForceMultiplier : MonoBehaviour {

	public VRDevice TargetDevice;
	public float MinForce;
	public float MaxForce;
	private ForceSource MyForceSource;

	void Awake() {
		MyForceSource = GetComponent<ForceSource>();
	}
	
	void Update () {
		if(MyForceSource != null)
			MyForceSource.StrengthModifier = Mathf.Lerp(MinForce, MaxForce, VRTK_Devices.TriggerAxis(TargetDevice));
	}
}
