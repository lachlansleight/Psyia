using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foliar.Compute {

	public class TestDataSupply : MonoBehaviour {

		public GPUBuffer TargetBuffer;
		public int Count = 10000;

		private void Awake() {
			ComputeStruct[] Data = new ComputeStruct[Count];
			for(int i = 0; i < Data.Length; i++) {
				Data[i].Position = Random.insideUnitSphere + new Vector3(0, 1, 0);
				Data[i].Velocity = Vector3.zero;
				Data[i].Color = Color.white;
			}

			TargetBuffer.SetType(typeof(ComputeStruct));
			TargetBuffer.SetCount(Count);
			TargetBuffer.SetData(Data);
		}
	}

}