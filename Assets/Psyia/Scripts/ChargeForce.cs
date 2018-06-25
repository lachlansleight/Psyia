using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ForceSource))]
public class ChargeForce : MonoBehaviour {

	public VRDevice TargetDevice;

	public float ChargeTime;
	public float DischargeTime;
	public AnimationCurve ForceAttenuationCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

	private float ChargeTimer;
	private float DischargeTimer;
	private bool Discharging;
	private ForceSource MyForce;


	void Awake() {
		MyForce = GetComponent<ForceSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(VRTK_Devices.TriggerPressed(TargetDevice) && !Discharging) {
			ChargeTimer += Time.deltaTime / ChargeTime;

			if(ChargeTimer >= 1f) {
				Discharging = true;
				DischargeTimer = 0f;
			}
		} else {
			ChargeTimer = 0f;
		}

		if(Discharging) {
			MyForce.StrengthModifier = ForceAttenuationCurve.Evaluate(DischargeTimer);
			DischargeTimer += Time.deltaTime / DischargeTime;
			if(DischargeTimer >= 1f) {
				Discharging = false;
				ChargeTimer = 0f;
			}
		} else {
			MyForce.StrengthModifier = 0;
		}
	}
}
