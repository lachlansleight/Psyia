using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class SimpleTest : MonoBehaviour {

	public int MaxCount = 100;
	public GpuBuffer SimBuffer;
	public GpuAppendBuffer EmitBuffer;

	public ComputeDispatcher Simulator;
	public ComputeDispatcher Emitter;

	// Use this for initialization
	void Start () {
		InitializeBuffers();
		PopulateBuffers();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.D)) {
			Emitter.Dispatch();
			LogOutput();
		}
		if(Input.GetKeyDown(KeyCode.W)) {
			Simulator.Dispatch();
			LogOutput();
		}
		
	}

	void LogOutput() {
		Vector4[] data = SimBuffer.GetData<Vector4>();
		string output = "";
		for(int i = 0; i < data.Length; i++) {
			Debug.Log(data[i]);
			if(data[i].y > 0) {
				output += (data[i].x);
			} else {
				output += ("-");
			}
			output += (" ");
		}
		Debug.Log(output);
	}

	void InitializeBuffers() {
		SimBuffer.SetType(typeof(Vector4));
		EmitBuffer.SetType(typeof(int));

		SimBuffer.SetCount(MaxCount);
		EmitBuffer.SetCount(MaxCount);
	}

	void PopulateBuffers() {
		Vector4[] SimData = new Vector4[MaxCount];
		int[] EmitData = new int[MaxCount];
		for(int i = 0; i < MaxCount; i++) {
			SimData[i] = Vector4.zero;
			EmitData[i] = i;
		}
		SimBuffer.SetData(SimData);
		EmitBuffer.SetData(EmitData);
	}


}
