using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class TestEmitter : MonoBehaviour {

	public GpuAppendBuffer AppendBuffer;
	public ComputeDispatcher Emitter;

	public int EmitCount = 1024;
	public float Distance = 2;
	
	private void Awake() {
	}

	private void Update() {
		if(Input.GetMouseButton(0)) {
			if(AppendBuffer.CurrentCount - EmitCount > 0) {
				Emitter.Dispatch(EmitCount, 1, 1);
			} else if(AppendBuffer.CurrentCount > 0) {
				Emitter.Dispatch(AppendBuffer.CurrentCount, 1, 1);
			}
		}
	}
}
