using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCTK;

public class OddEvenSortDispatcher : MonoBehaviour {

	[Header("Settings")]
	public bool Increasing = true;
	public int SortIterationsPerFrame = 20;

	[Header("References")]
	public GpuBuffer DistanceBuffer;
	public ComputeDispatcher SortShader;

	private int Stage;
	private int SubStage;

	public void DoSortSteps() {
		for(int i = 0; i < SortIterationsPerFrame; i++) {
			SetValues();
			SortShader.Dispatch(DistanceBuffer.Count / 1024, 1, 1);

			IncrementOddEven();
		}
	}

	void IncrementOddEven() {
		if(SubStage == Stage) {
			//unless we're finished
			if(Stage == LogBaseTwo(DistanceBuffer.Count) - 1) {
				//break;
				SubStage = 0;
				Stage = 0;
			} else {
				Stage++;
				SubStage = 0;
			}
		} else {
			SubStage++;
		}
	}

	void SetValues() {
		SortShader.Shader.SetInt("Stage", Stage);
		SortShader.Shader.SetInt("SubStage", SubStage);
		SortShader.Shader.SetInt("Increasing", Increasing ? 1 : 0);
	}

	float LogBaseTwo(float Number) {
		return Mathf.Log(Number) / Mathf.Log(2f);
	}
}
