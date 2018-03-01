using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUBitonic : MonoBehaviour {

	public int Count = 1024;
	public Mesh DrawMesh;
	public Material DrawMat;

	public bool AutoIncrement = false;
	public int IncrementStep = 1;

	public float TotalDrawSize = 10f;
	public float TotalDrawHeight = 2f;

	public bool CreateBitonic = true;
	public int ButterflyStep = 0;
	public int SubStep = 0;
	public int MergeStep = 0;

	SortStruct[] Data;

	public struct SortStruct {
		public Vector4 Color;
	}

	// Use this for initialization
	void Start () {
		BuildData();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.A)) AutoIncrement = !AutoIncrement;

		if(Input.GetKeyDown(KeyCode.UpArrow)) IncrementStep++;
		if(Input.GetKeyDown(KeyCode.DownArrow)) IncrementStep = (int)Mathf.Max(1, IncrementStep - 1);

		if(AutoIncrement || Input.GetKeyDown(KeyCode.Space)) {
			for(int i = 0; i < IncrementStep; i++) {
				BitonicStep();

				if(!CreateBitonic) {
					//if we've finished, reset
					if(MergeStep == 0) {
						return;
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
						if(ButterflyStep == (int)LogBaseTwo(Count) - 1) {
							CreateBitonic = false;
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

		DrawData();
	}

	float LogBaseTwo(float Number) {
		return Mathf.Log(Number) / Mathf.Log(2f);
	}

	void BuildData() {
		Data = new SortStruct[Count];
		for(int i = 0; i < Data.Length; i++) {
			Data[i] = new SortStruct();
			Data[i].Color = new Vector4(Random.Range(0f, 1f), 1f, 1f, 1f);
		}
	}

	void DrawData() {
		for(int i = 0; i < Data.Length; i++) {
			Matrix4x4 TransformMat = Matrix4x4.TRS(new Vector3(TotalDrawSize * (float)i / (float)Count, 0f, 0f), Quaternion.identity, new Vector3(TotalDrawSize / (float)Count, Data[i].Color.x * TotalDrawHeight, TotalDrawSize / (float)Count));
			Graphics.DrawMesh(DrawMesh, TransformMat, DrawMat, 0);
		}
	}

	void BitonicStep() {
		for(int CurrentIndex = 0; CurrentIndex < Data.Length; CurrentIndex++) {
			uint CurrentSize = 0;
			uint ButterflySize = 0;

			uint TargetIndex = 0;
			bool Skip = false;
			bool Ascending = false;
			try {
				if (CreateBitonic) {
					CurrentSize = (uint)Mathf.Pow(2, ButterflyStep - SubStep);
					ButterflySize = (uint)Mathf.Pow(2, ButterflyStep);

					TargetIndex = (uint)(CurrentIndex + CurrentSize);
					Skip = CurrentIndex % (CurrentSize * 2) > (CurrentSize - 1);
					Ascending = (uint)Mathf.Floor(CurrentIndex / (ButterflySize * 2)) % 2 > 0;

					if (!Skip) {
						SortStruct a = Data[CurrentIndex];
						SortStruct b = Data[TargetIndex];

						if (a.Color.x < b.Color.x != Ascending) {
							Data[CurrentIndex] = b;
							Data[TargetIndex] = a;
						}
					}
				}
				else {
					CurrentSize = (uint)Mathf.Pow(2, MergeStep);

					TargetIndex = (uint)(CurrentIndex + CurrentSize);
					Skip = (uint)Mathf.Floor(CurrentIndex / CurrentSize) % 2 > 0;
		
					if (!Skip) {
						SortStruct a = Data[CurrentIndex];
						SortStruct b = Data[TargetIndex];

						if (a.Color.x > b.Color.x) {
							Data[CurrentIndex] = b;
							Data[TargetIndex] = a;
						}
					}
				}
			} catch(System.Exception e) {
				Debug.LogError("Error at index " + CurrentIndex + " - " + e.Message);
				Debug.LogFormat("CB: {0}\tBFS: {1}\tSS: {2}\tMS: {3}", CreateBitonic, ButterflyStep, SubStep, MergeStep);
				if(CreateBitonic) {
					Debug.LogFormat("CurrentSize: {0}\tButterflySize: {1}\tTargetIndex: {2}\tSkip: {3}\tAscending: {4}", CurrentSize, ButterflySize, TargetIndex, Skip, Ascending);
				} else {
					Debug.LogFormat("CurrentSize: {0}\tTargetIndex: {1}\tSkip: {2}", CurrentSize, TargetIndex, Skip);
				}
				return;
			}
		}
	}
}
