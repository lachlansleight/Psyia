using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class TestSorter : MonoBehaviour {

	public enum SortMode {
		OddEven,
		Bitonic
	}

	[Header("Settings")]
	public SortMode Mode = SortMode.OddEven;
	public bool Increasing = true;
	public int SortIterationsPerFrame = 100;

	[Header("References")]
	public GpuBuffer DistanceBuffer;
	public ComputeDispatcher SortShader;
	public ExampleSortValues Values;
	public ComputeParameterSetter Setter;


	private bool CreateBitonic = true;
	private int MergeStep = 0;
	private int ButterflyStep = 0;
	private int SubStep = 0;

	private int Stage;
	private int SubStage;

	// Use this for initialization
	void Start () {
		
	}

	public void DoSortSteps() {
		for(int i = 0; i < SortIterationsPerFrame; i++) {
			SetValues();
			SortShader.Dispatch(DistanceBuffer.Count / 1024, 1, 1);

			if(Mode == SortMode.OddEven) {
				IncrementOddEven();
			} else if(Mode == SortMode.Bitonic) {
				IncrementBitonic();
			}
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

	void IncrementBitonic() {
		if(!CreateBitonic) {
			//if we've finished, reset
			if(MergeStep == 0) {
				CreateBitonic = true;
				ButterflyStep = 0;
				SubStep = 0;
			//otherwise, keep merging
			} else {
				MergeStep--;
			}
		} else {
			//if we've finished this butterfly step...
			if(SubStep == ButterflyStep) {
				//and this was the last butterfly step, start merging the bitonics
				if(ButterflyStep == (int)LogBaseTwo(DistanceBuffer.Count) - 1) {
					CreateBitonic = false;
					ButterflyStep = 0;
					SubStep = 0;
					MergeStep = (int)LogBaseTwo(DistanceBuffer.Count) - 1;
				//reset substep and incrment butterfly step
				} else {
					SubStep = 0;
					ButterflyStep++;
				}
			} else {
				SubStep++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetValues() {
		Values.CreateBitonic = CreateBitonic ? 1 : 0;
		Values.MergeStep = MergeStep;
		Values.ButterflyStep = ButterflyStep;
		Values.SubStep = SubStep;

		Values.Stage = Stage;
		Values.SubStage = SubStage;

		Values.Increasing = Increasing ? 1 : 0;

		Setter.ApplyNow();
	}

	float LogBaseTwo(float Number) {
		return Mathf.Log(Number) / Mathf.Log(2f);
	}
}
