using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class TestEmitter : MonoBehaviour {

	public GpuAppendBuffer AppendBuffer;
	public ComputeDispatcher Emitter;
	public int Count = 1048576;

	private void Awake() {
			InitializeData();
			SetupData();
		}

		private void Update() {
			if(Input.GetKey(KeyCode.Space)) {
				if(AppendBuffer.CurrentCount > 0) {
					Emitter.Dispatch();
				}
			}
		}

		void SetupData() {
			int[] Data = new int[Count];
			for(int i = 0; i < Data.Length; i++) {
				Data[i] = i;
			}

			AppendBuffer.SetData(Data);

			Debug.LogFormat("Append buffer current count is now " + AppendBuffer.CurrentCount);
		}

		void InitializeData() {
			AppendBuffer.SetType(typeof(int));
			AppendBuffer.SetCount(Count);
		}
}
