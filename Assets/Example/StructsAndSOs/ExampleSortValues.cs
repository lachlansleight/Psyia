using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

[CreateAssetMenu(fileName = "ExampleSortValue.asset", menuName = "ScriptableObjects/ExampleSortValue", order = 1)]
public class ExampleSortValues : ShaderValues {
	[ComputeValue]
	public int ButterflyStep;

	[ComputeValue]
	public int SubStep;

	[ComputeValue]
	public int MergeStep;

	[ComputeValue]
	public int CreateBitonic;

	[ComputeValue]
	public int Stage;

	[ComputeValue]
	public int SubStage;

	[ComputeValue]
	public int Increasing;
}
