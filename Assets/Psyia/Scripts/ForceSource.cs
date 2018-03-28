using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSource : MonoBehaviour {

	public PsyiaForce Force;
	public float StrengthModifier = 1f;

	void Start() {
		ForceManager.Instance.AddSource(this);
	}
}
