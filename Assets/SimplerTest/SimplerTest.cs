using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplerTest : MonoBehaviour {

	public int MaxCount = 16;
	public ComputeBuffer MainBuffer;
	public ComputeBuffer DeadBuffer;
	public ComputeBuffer ArgsBuffer;

	public ComputeShader AddOneShader;
	public ComputeShader ConsumeShader;
	public ComputeShader AppendShader;

	// Use this for initialization
	void Start () {
		InitializeBuffers();
		PopulateBuffers();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.D)) {
			ConsumeShader.Dispatch(0, 1, 1, 1);
			LogOutput();
		}
		if(Input.GetKeyDown(KeyCode.W)) {
			AddOneShader.Dispatch(0, 1, 1, 1);
			LogOutput();
		}
		if(Input.GetKeyDown(KeyCode.S)) {
			int[] args = new int[]{ 0, 1, 0, 0 };
			ArgsBuffer.SetData(args);
			ComputeBuffer.CopyCount(DeadBuffer, ArgsBuffer, 0);
			ArgsBuffer.GetData(args);
			Debug.Log("Dead list length: " + args[0]);
		}
		
	}

	void LogOutput() {
		Vector4[] data = new Vector4[MainBuffer.count];
		MainBuffer.GetData(data);
		string output = "";
		for(int i = 0; i < data.Length; i++) {
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
		MainBuffer = new ComputeBuffer(MaxCount, sizeof(float) * 4, ComputeBufferType.Default);
		DeadBuffer = new ComputeBuffer(MaxCount, sizeof(uint), ComputeBufferType.Append);
		ArgsBuffer = new ComputeBuffer(4, sizeof(int), ComputeBufferType.IndirectArguments);
		DeadBuffer.SetCounterValue(0);

		AddOneShader.SetBuffer(0, "outputBuffer", MainBuffer);
		ConsumeShader.SetBuffer(0, "outputBuffer", MainBuffer);
		ConsumeShader.SetBuffer(0, "deadList", DeadBuffer);
		AppendShader.SetBuffer(0, "deadList", DeadBuffer);
	}

	void PopulateBuffers() {
		Vector4[] SimData = new Vector4[MaxCount];
		uint[] EmitData = new uint[MaxCount];
		for(int i = 0; i < MaxCount; i++) {
			SimData[i] = Vector4.zero;
			EmitData[i] = (uint)i;
		}
		MainBuffer.SetData(SimData);

		AppendShader.Dispatch(0, 1, 1, 1);
	}

	private void OnDestroy() {
		MainBuffer.Release();
		DeadBuffer.Release();
		ArgsBuffer.Release();
	}
}
