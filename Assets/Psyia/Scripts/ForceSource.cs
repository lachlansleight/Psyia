using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSource : MonoBehaviour {

	public PsyiaForce Force;
	public float StrengthModifier = 1f;

	public bool UseTransformPosition = true;
	public bool UseTransformRotation = true;
	public bool UseStrengthModifier = true;

	void Start() {
		ForceManager.Instance.AddSource(this);
	}

	public ForceData GetForceData() {
		return Force.GetForceData(
			UseTransformPosition ? transform.position : Force.Position,
			UseTransformRotation ? transform.eulerAngles : Force.Rotation,
			UseStrengthModifier ? Force.Strength * StrengthModifier : Force.Strength
		);
	}
}
