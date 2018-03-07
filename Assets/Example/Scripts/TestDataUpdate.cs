using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class TestDataUpdate : MonoBehaviour {

	public ExampleShaderValues TargetValues;

	public Transform Left;
	public Transform Right;
	public Transform HMD;

	public float LeftStrength = 1;
	public float RightStrength = 1;

	public float Damping = 0.01f;
	public float ParticleCharge = -0.1f;
	public float ParticleMass = 1;
	public float SofteningFactor = 0.01f;

	public float Lifespan = 5f;


	// Use this for initialization
	void Start () {
		Damping = TargetValues.Damping;
		ParticleCharge = TargetValues.ParticleCharge;
		ParticleMass = TargetValues.ParticleMass;
		SofteningFactor = TargetValues.SofteningFactor;
	}
	
	// Update is called once per frame
	void Update () {
		TargetValues.LeftController = new Vector4(Left.position.x, Left.position.y, Left.position.z, LeftStrength * VRTK_Devices.TriggerAxis(VRDevice.Left));
		TargetValues.RightController = new Vector4(Right.position.x, Right.position.y, Right.position.z, RightStrength * VRTK_Devices.TriggerAxis(VRDevice.Right));
		TargetValues.LeftVelocity = VRTK_Devices.Velocity(VRDevice.Left);
		TargetValues.RightVelocity = VRTK_Devices.Velocity(VRDevice.Right);
		TargetValues.Headset = new Vector4(HMD.position.x, HMD.position.y, HMD.position.z, 0f);

		TargetValues.Damping = Damping;
		TargetValues.ParticleCharge = ParticleCharge;
		TargetValues.ParticleMass = ParticleMass;
		TargetValues.SofteningFactor = SofteningFactor;

		TargetValues.Lifespan = Lifespan;
		TargetValues.Time = Time.time;
		TargetValues.DeltaTime = Time.deltaTime;		
	}
}
