using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foliar.Compute {

	[RequireComponent(typeof(GpuBuffer))]
	public class BufferSetup : MonoBehaviour {

		public string DataType;
		public int Count;
		protected GpuBuffer MyGpuBuffer;

		public virtual void Setup() {
			MyGpuBuffer = GetComponent<GpuBuffer>();
			System.Type ParsedType = System.Type.GetType(DataType);
			if(ParsedType == null) {
				Debug.LogError("Failed to parse type '" + DataType + "'!");
			}
			MyGpuBuffer.SetType(ParsedType);
			MyGpuBuffer.SetCount(Count);

			CreateData();
		}

		public virtual void Setup(int NewCount) {
			Count = NewCount;
			Setup();
		}

		protected virtual void CreateData() {

		}
	}

}