using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ControllerForce))]
public class ControllerForceDistanceMod : MonoBehaviour {

	public Transform Target;
	public AnimationCurve ModifierCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	private ControllerForce CF;

	void Awake() {
		CF = GetComponent<ControllerForce>();
	}
	
	// Update is called once per frame
	void Update () {
		float CurrentDistance = Vector3.Magnitude(transform.position - Target.position);
		CF.MaxForce = ModifierCurve.Evaluate(CurrentDistance);
	}
}
