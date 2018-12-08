/*

Copyright (c) 2018 Lachlan Sleight

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

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
