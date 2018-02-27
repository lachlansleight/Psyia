using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foliar.Compute {

	public class TestDataSupply : MonoBehaviour {

		public GpuBuffer TargetBuffer;
		public int Count = 10000;

		private void Awake() {
			InitializeData();
			SetupData();
		}

		private void Update() {
		}

		void SetupData() {
			ComputeStruct[] Data = new ComputeStruct[Count];
			for(int i = 0; i < Data.Length; i++) {
				Data[i].Position = Random.insideUnitSphere + new Vector3(0, 1, 0);
				Data[i].Velocity = Vector3.zero;
				Data[i].Color = Color.Lerp(Color.red, Color.blue, Random.Range(0f, 1f));
				Data[i].IsAlive = 0;
			}

			TargetBuffer.SetData(Data);
		}

		void InitializeData() {
			TargetBuffer.SetType(typeof(ComputeStruct));
			TargetBuffer.SetCount(Count);
		}
	}

}