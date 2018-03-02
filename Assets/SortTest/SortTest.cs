using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortTest : MonoBehaviour {

	public struct SortStruct {
		public Vector4 color;
	}

	public enum SortMode {
		Bitonic,
		OddEven
	}

	[Header("Settings")]
	public SortMode Mode;
	public int Count = 8192;
	public bool AutoIncrement = false;
	public int IncrementStep = 1;
	public bool Increasing;

	SortStruct[] InputData; //sorting by red amount

	[Header("References")]
	public ComputeShader Sorter;
	public Material OutputMaterial;
	ComputeBuffer SortBuffer;

	[Header("Bitonic Stuff")]
	public int ButterflyStep = 0;
	public int SubStep = 0;
	public int MergeStep = 0;
	public int CreateBitonic = 0;

	[Header("OddEven Stuff")]
	public int Stage = 0;
	public int SubStage = 0;


	// Use this for initialization
	void Start () {
		CreateBitonic = 1;
		ButterflyStep = 0;
		SubStep = 0;
		MergeStep = 0;

		SetupEverything();
	}

	void SetupEverything() {
		InputData = new SortStruct[Count];
		for(int i = 0; i < InputData.Length; i++) {
			InputData[i] = new SortStruct();
			float value = Random.Range(0f, 1f);
			InputData[i].color = new Color(value, 1f - value, 1f - value, 1f);
		}

		SortBuffer = new ComputeBuffer(InputData.Length, sizeof(float) * 4);
		SortBuffer.SetData(InputData);

		Sorter.SetBuffer(Sorter.FindKernel("Bitonic"), "outputBuffer", SortBuffer);
		Sorter.SetBuffer(Sorter.FindKernel("OddEven"), "outputBuffer", SortBuffer);
		OutputMaterial.SetBuffer("inputBuffer", SortBuffer);
	}

	void SetComputeState() {
		OutputMaterial.SetFloat("_Count", (float)Count);

		Sorter.SetInt("ButterflyStep", ButterflyStep);
		Sorter.SetInt("SubStep", SubStep);
		Sorter.SetInt("MergeStep", MergeStep);
		Sorter.SetInt("CreateBitonic", CreateBitonic);

		Sorter.SetInt("Stage", Stage);
		Sorter.SetInt("SubStage", SubStage);

		Sorter.SetInt("Increasing", Increasing ? 1 : 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A)) AutoIncrement = !AutoIncrement;

		if(Input.GetKeyDown(KeyCode.UpArrow)) IncrementStep++;
		if(Input.GetKeyDown(KeyCode.DownArrow)) IncrementStep = (int)Mathf.Max(1, IncrementStep - 1);

		if(AutoIncrement || Input.GetKeyDown(KeyCode.Space)) {
			
			if(Mode == SortMode.OddEven) {

				for(int i = 0; i < IncrementStep; i++) {
					SetComputeState();
					Sorter.Dispatch(Sorter.FindKernel("OddEven"), Count / 1024, 1, 1);

					//time to move up a stage
					if(SubStage == Stage) {
						//unless we're finished
						if(Stage == LogBaseTwo(Count) - 1) {
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
			} else if(Mode == SortMode.Bitonic) {

				for(int i = 0; i < IncrementStep; i++) {
					SetComputeState();
					Sorter.Dispatch(Sorter.FindKernel("Bitonic"), Count / 1024, 1, 1);

					if(CreateBitonic == 0) {
						//if we've finished, reset
						if(MergeStep == 0) {
							//break;
							CreateBitonic = 1;
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
							if(ButterflyStep == (int)LogBaseTwo(Count) - 1) {
								CreateBitonic = 0;
								ButterflyStep = 0;
								SubStep = 0;
								MergeStep = (int)LogBaseTwo(Count) - 1;
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
			}
		}
	}

	float LogBaseTwo(float Number) {
		return Mathf.Log(Number) / Mathf.Log(2f);
	}

	private void OnRenderObject() {
		OutputMaterial.SetPass(0);
		Graphics.DrawProcedural(MeshTopology.Lines, SortBuffer.count);
	}

	private void OnDestroy() {
		SortBuffer.Release();
	}
}
