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
    [ComputeValue] public int LutWidth;
    [ComputeTexture("ColorParticlesTexLookup", "ColorParticles")] public Texture2D XLut;
    [ComputeTexture("ColorParticlesTexLookup", "ColorParticles")] public Texture2D YLut;
    [ComputeTexture("ColorParticlesTexLookup", "ColorParticles")] public Texture2D ZLut;
}
