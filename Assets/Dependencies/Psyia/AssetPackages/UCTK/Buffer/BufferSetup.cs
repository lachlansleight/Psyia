using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCTK {

	/// <summary>
	/// Component attached to a GpuBuffer GameObject that specifies the initial type, size and contents of the buffer
	/// </summary>
	[RequireComponent(typeof(GpuBuffer))]
	public class BufferSetup : MonoBehaviour {

		/// <summary>
		/// The string name of the Data Type to be used with this Compute Buffer (case-sensitive)
		/// </summary>
		public string DataType;

		/// <summary>
		/// The number of elements to initialize the buffer with
		/// </summary>
		public int Count;

		/// <summary>
		/// The GpuBuffer this BufferSetup object is initializing
		/// </summary>
		protected GpuBuffer MyGpuBuffer;

		/// <summary>
		/// Initializes the GpuBuffer with the correct DataType and Count, then calls CreateData
		/// </summary>
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

		/// <summary>
		/// Initialies the GpuBuffer with the correct DataType, supplies a custom buffer length, then calls CreateData
		/// </summary>
		/// <param name="NewCount"></param>
		public virtual void Setup(int NewCount) {
			Count = NewCount;
			Setup();
		}

		/// <summary>
		/// Virtual fucntion that actually creates the data to populate the GpuBuffer with
		/// </summary>
		protected virtual void CreateData() {

		}
	}

}