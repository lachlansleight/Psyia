using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortTest : MonoBehaviour {

	public struct SortStruct {
		public Vector4 color;
	}


	public int Count = 8192;

	SortStruct[] InputData; //sorting by red amount

	public ComputeShader Sorter;
	ComputeBuffer SortBuffer;

	public Material OutputMaterial;

	public int ButterflyStep = 0;
	public int SubStep = 0;
	public int MergeStep = 0;
	public int CreateBitonic = 0;

	public bool AutoIncrement = false;
	public int IncrementStep = 1;

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

		Sorter.SetBuffer(0, "outputBuffer", SortBuffer);
		OutputMaterial.SetBuffer("inputBuffer", SortBuffer);
	}

	void SetComputeState() {
		OutputMaterial.SetFloat("_Count", (float)Count);

		Sorter.SetInt("ButterflyStep", ButterflyStep);
		Sorter.SetInt("SubStep", SubStep);
		Sorter.SetInt("MergeStep", MergeStep);
		Sorter.SetInt("CreateBitonic", CreateBitonic);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A)) AutoIncrement = !AutoIncrement;

		if(Input.GetKeyDown(KeyCode.UpArrow)) IncrementStep++;
		if(Input.GetKeyDown(KeyCode.DownArrow)) IncrementStep = (int)Mathf.Max(1, IncrementStep - 1);

		if(AutoIncrement || Input.GetKeyDown(KeyCode.Space)) {
			for(int i = 0; i < IncrementStep; i++) {
				if(CreateBitonic == 0) {
					//if we've finished, reset
					if(MergeStep == 0) {
						return;
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

				SetComputeState();
				Sorter.Dispatch(0, Count / 1024, 1, 1);
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
