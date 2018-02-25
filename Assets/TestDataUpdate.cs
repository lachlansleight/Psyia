using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class TestDataUpdate : MonoBehaviour {

	public ShaderValues TargetValues;

	public Transform Left;
	public Transform Right;

	public float LeftStrength = 1;
	public float RightStrength = 1;

	public float Damping = 0.01f;
	public float Charge = -0.1f;
	public float Mass = 1;
	public float Softening = 0.01f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TargetValues.LeftController = new Vector4(Left.position.x, Left.position.y, Left.position.z, LeftStrength);
		TargetValues.RightController = new Vector4(Right.position.x, Right.position.y, Right.position.z, RightStrength);

		TargetValues.Damping = Damping;
		TargetValues.ParticleCharge = Charge;
		TargetValues.ParticleMass = Mass;
		TargetValues.SofteningFactor = Softening;
	}
}
