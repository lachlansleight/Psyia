using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foliar.Compute {

	public class TestDataSupply : MonoBehaviour {

		public GpuBuffer TargetBuffer;
		public GpuAppendBuffer AppendTargetBuffer;
		public ComputeDispatcher AppendDispatcher;
		public GpuBuffer DistanceBuffer;
		public int Count = 10000;

		private void Awake() {
			InitializeData();
			SetupData();
		}

		private void Update() {
		}

		void SetupData() {
			ComputeStruct[] Data = new ComputeStruct[Count];
			DistanceStruct[] DistanceData = new DistanceStruct[Count];
			for(int i = 0; i < Data.Length; i++) {
				Data[i].pos = Random.insideUnitSphere * 0.1f + new Vector3(0, 1, 0);
				Data[i].velocity = Vector3.zero;
				Data[i].color = Color.Lerp(Color.red, Color.blue, Random.Range(0f, 1f));
				Data[i].color.a = 1f;
				Data[i].isAlive = 0;

				DistanceData[i] = new DistanceStruct();
				DistanceData[i].Index = i;
				DistanceData[i].Distance = 0f;
			}

			TargetBuffer.SetData(Data);
			DistanceBuffer.SetData(DistanceData);

			AppendDispatcher.Dispatch(Count / 1024, 1, 1);
		}

		void InitializeData() {
			TargetBuffer.SetType(typeof(ComputeStruct));
			TargetBuffer.SetCount(Count);

			AppendTargetBuffer.SetType(typeof(uint));
			AppendTargetBuffer.SetCount(Count);

			DistanceBuffer.SetType(typeof(DistanceStruct));
			DistanceBuffer.SetCount(Count);
		}
	}

}