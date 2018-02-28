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
	public Vector4 SpawnPosition;

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

	[ComputeValue]
	public float DeltaTime;
}
