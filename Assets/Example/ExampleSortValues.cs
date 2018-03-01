using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

[CreateAssetMenu(fileName = "ExampleSortValue.asset", menuName = "ScriptableObjects/ExampleSortValue", order = 1)]
public class ExampleSortValues : ShaderValues {
	[ShaderValue]
	public int ButterflyStep;

	[ShaderValue]
	public int SubStep;

	[ShaderValue]
	public int MergeStep;

	[ShaderValue]
	public int CreateBitonic;
}
