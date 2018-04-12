using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

[CreateAssetMenu(fileName = "ParticleSettings.asset", menuName = "ScriptableObjects/ParticleSettings", order = 1)]
public class ParticleSettings : ShaderValues {
	[ComputeValue] public float Damping;
    [ComputeValue] public float ForceMultiplier;
    [ComputeValue] public float ParticleMass;
    [ComputeValue] public float Lifespan;
}
