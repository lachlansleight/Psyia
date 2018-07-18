using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCTK;

[CreateAssetMenu(fileName = "EmissionSettings.asset", menuName = "ScriptableObjects/EmissionSettings", order = 1)]
public class EmissionSettings : ShaderValues {
	[ComputeValue] public float SpawnVelocityScatter;
    [ComputeValue] public float SpawnInheritVelocity;
    [ComputeValue] public float SpawnRadius;
}
