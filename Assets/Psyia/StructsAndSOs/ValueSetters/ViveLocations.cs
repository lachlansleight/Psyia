using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

[CreateAssetMenu(fileName = "ViveLocations.asset", menuName = "ScriptableObjects/ViveLocations", order = 1)]
public class ViveLocations : ShaderValues {
	[ComputeValue] public Vector3 HeadsetPosition;
    [ComputeValue] public Vector3 LeftControllerPosition;
    [ComputeValue] public Vector3 RightControllerPosition;
}
