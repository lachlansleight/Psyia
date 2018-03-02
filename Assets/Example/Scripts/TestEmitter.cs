using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class TestEmitter : MonoBehaviour {

	public GpuAppendBuffer AppendBuffer;
	public ComputeDispatcher Emitter;
	public ExampleShaderValues Values;
	public ComputeParameterSetter Setter;

	public int EmitCount = 1024;
	public float Distance = 2;

	Vector3 SpawnPos = Vector3.zero;
	Vector3 LastSpawnPos = Vector3.zero;
	
	private void Awake() {
	}

	private void Update() {
		if(VRTK_Devices.TouchpadPressed("left")) {
			if(AppendBuffer.CurrentCount > 0) {
				SpawnPos = VRTK_Devices.Position("left");
				if(VRTK_Devices.TouchpadPressDown("left")) LastSpawnPos = SpawnPos;
				SetSpawnPos();
				Emitter.Dispatch((int)Mathf.Min(EmitCount, AppendBuffer.CurrentCount), 1, 1);
			}
		} else if(VRTK_Devices.TouchpadPressed("right")) {
			if(AppendBuffer.CurrentCount > 0) {
				SpawnPos = VRTK_Devices.Position("right");
				if(VRTK_Devices.TouchpadPressDown("right")) LastSpawnPos = SpawnPos;
				SetSpawnPos();
				Emitter.Dispatch((int)Mathf.Min(EmitCount, AppendBuffer.CurrentCount), 1, 1);
			}
		}

		LastSpawnPos = SpawnPos;
	}

	void SetSpawnPos() {
		Values.LastSpawnPosition = LastSpawnPos;
		Values.SpawnPosition = SpawnPos;
		Setter.ApplyNow();
	}
}
