using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUOddEvenMerge : MonoBehaviour {

	public int Count = 1024;
	public Mesh DrawMesh;
	public Material DrawMat;

	public bool AutoIncrement = false;
	public int IncrementStep = 1;

	public float TotalDrawSize = 10f;
	public float TotalDrawHeight = 2f;

	public int Stage;
	public int SubStage;

	float[] Data;

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
				OddEvenStep();

				//time to move up a stage
				if(SubStage == Stage) {
					//unless we're finished
					if(Stage == LogBaseTwo(Count) - 1) {
						break;
					} else {
						Stage++;
						SubStage = 0;
					}
				} else {
					SubStage++;
				}
				
			}
		}

		DrawData();
	}

	float LogBaseTwo(float Number) {
		return Mathf.Log(Number) / Mathf.Log(2f);
	}

	void BuildData() {
		Data = new float[Count];
		for(int i = 0; i < Data.Length; i++) {
			Data[i] = Random.Range(0f, 1f);
		}
	}

	void DrawData() {
		for(int i = 0; i < Data.Length; i++) {
			Matrix4x4 TransformMat = Matrix4x4.TRS(new Vector3(TotalDrawSize * (float)i / (float)Count, 0f, 0f), Quaternion.identity, new Vector3(TotalDrawSize / (float)Count, Data[i] * TotalDrawHeight, TotalDrawSize / (float)Count));
			Graphics.DrawMesh(DrawMesh, TransformMat, DrawMat, 0);
		}
	}

	void OddEvenStep() {
		for(int CurrentIndex = 0; CurrentIndex < Data.Length; CurrentIndex++) {
			uint CurrentSize = 0;
			uint TargetIndex = 0;
			uint ExtraPow = 0;
			bool Skip = false;

			try {
				CurrentSize = (uint)Mathf.Pow(2f, Stage - SubStage);
				TargetIndex = (uint)CurrentIndex + CurrentSize;
				if(SubStage == 0)
					Skip = (CurrentIndex / CurrentSize) % 2 > 0;
				else if(SubStage == 1)
					Skip = (CurrentIndex / CurrentSize) % 4 != 1;
				else {
					ExtraPow = (uint)Mathf.Pow(2, SubStage + 1);
					Skip = ((CurrentIndex / CurrentSize) % 2 < 1) || ((CurrentIndex / CurrentSize) % ExtraPow == (ExtraPow - 1));
				}

				if(!Skip) {
					float a = Data[CurrentIndex];
					float b = Data[TargetIndex];
					if(a > b) {
						Data[CurrentIndex] = b;
						Data[TargetIndex] = a;
					}
				}

			} catch(System.Exception e) {
				Debug.LogError("Error at index " + CurrentIndex + " - " + e.Message);
				Debug.LogFormat("Stage: {0}\tSubStage: {1}", Stage, SubStage);
				if(SubStage < 2)
					Debug.LogFormat("CurrentSize: {0}\tTargetIndex: {1}\tSkip: {2}", CurrentSize, TargetIndex, Skip);
				else
					Debug.LogFormat("CurrentSize: {0}\tTargetIndex: {1}\tSkip: {2}\tExtraPow: {3}", CurrentSize, TargetIndex, Skip, ExtraPow);
				return;
			}
		}
	}
}
