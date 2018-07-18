using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCTK;

[CreateAssetMenu(fileName = "ParticleSettings.asset", menuName = "ScriptableObjects/ParticleSettings", order = 1)]
public class ParticleSettings : ShaderValues {
	[ComputeValue] public float Damping;
    [ComputeValue] public float NoiseDamping;
    [ComputeValue] public float TurbulenceViscosity;
    [ComputeValue] public float ForceMultiplier;
    [ComputeValue] public float ParticleMass;
    [ComputeValue] public float NoiseAmount;
    [ComputeValue] public float Lifespan;
    [ComputeValue] public float Y;
}
