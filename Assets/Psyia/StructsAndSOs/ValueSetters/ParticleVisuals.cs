﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

[CreateAssetMenu(fileName = "ParticleVisuals.asset", menuName = "ScriptableObjects/ParticleVisuals", order = 1)]
public class ParticleVisuals : ShaderValues {

    public Material ParticleMaterial;
    [ShaderValue] public Texture2D _Image;
    [ShaderValue] public Color _Color;
	[ShaderValue] public float _PointSize;
    [ShaderValue] public float _LineLength;
}
