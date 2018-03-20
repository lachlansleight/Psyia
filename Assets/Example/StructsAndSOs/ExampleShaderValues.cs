using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

[CreateAssetMenu(fileName = "ExampleShaderValue.asset", menuName = "ScriptableObjects/ExampleShaderValue", order = 1)]
public class ExampleShaderValues : ShaderValues {
	[ShaderValue]
	public Color _Color;
		
	[ComputeValue]
	public Vector4 LeftController;

	[ComputeValue]
	public Vector4 RightController;

	[ComputeValue]
	public Vector3 LeftEulers;
	
	[ComputeValue]
	public Vector3 RightEulers;

	[ComputeValue]
	public Vector3 LeftVelocity;

	[ComputeValue]
	public Vector3 RightVelocity;

	[ComputeValue]
	public Vector4 Headset;

	[ComputeValue]
	public Vector4 SpawnPosition;

	[ComputeValue]
	public Vector3 SpawnVelocity;

	[ComputeValue]
	public float SpawnVelocityScatter;

	[ComputeValue]
	public float SpawnInheritVelocity;

	[ComputeValue]
	public Vector4 LastSpawnPosition;

	[ComputeValue]
	public float Damping;
		
	[ComputeValue]
	public float ParticleCharge;
		
	[ComputeValue]
	public float ParticleMass;
		
	[ComputeValue]
	public float SofteningFactor;

	[ComputeValue]
	public float Lifespan;

	[ComputeValue]
	public float Time;

	[ComputeValue] [ShaderValue]
	public float DeltaTime;

	[ComputeValue] [ShaderValue]
	public Vector3 FieldStartPos;

	[ComputeValue] [ShaderValue]
	public Vector3 FieldEndPos;

	[ComputeValue] [ShaderValue]
	public Vector3 FieldCount;

	[ComputeValue]
	public float FieldDamping;

	[ShaderValue]
	public float FieldDisplayTime;

	[ComputeValue]
	public float ThresholdFieldChange;

	[ComputeValue]
	public float ControllerForce;

	[ComputeValue]
	public float ControllerRadius;

	[ComputeValue]
	public float VortexStrength;

	[ComputeValue]
	public float AttractionStrength;

	[ComputeValue]
	public float ControllerInstant;

	[ComputeValue]
	public float ControllerAttenuating;
}
