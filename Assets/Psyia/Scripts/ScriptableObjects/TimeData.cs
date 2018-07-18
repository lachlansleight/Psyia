using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCTK;

[CreateAssetMenu(fileName = "TimeData.asset", menuName = "ScriptableObjects/TimeData", order = 1)]
public class TimeData : ShaderValues {
	[ComputeValue] public float Time;
    [ComputeValue] public float DeltaTime;
}
