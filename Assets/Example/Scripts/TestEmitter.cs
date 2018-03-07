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
		if(VRTK_Devices.TouchpadPressed(VRDevice.Left)) {
			if(AppendBuffer.CurrentCount > 0) {
				SpawnPos = VRTK_Devices.Position(VRDevice.Left);
				if(VRTK_Devices.TouchpadPressDown(VRDevice.Left)) LastSpawnPos = SpawnPos;
				Values.SpawnVelocity = VRTK_Devices.Velocity(VRDevice.Left);
				SetSpawnPos();
				Emitter.Dispatch((int)Mathf.Min(EmitCount, AppendBuffer.CurrentCount), 1, 1);
			}
		} else if(VRTK_Devices.TouchpadPressed(VRDevice.Right)) {
			if(AppendBuffer.CurrentCount > 0) {
				SpawnPos = VRTK_Devices.Position(VRDevice.Right);
				if(VRTK_Devices.TouchpadPressDown(VRDevice.Right)) LastSpawnPos = SpawnPos;
				Values.SpawnVelocity = VRTK_Devices.Velocity(VRDevice.Right);
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
