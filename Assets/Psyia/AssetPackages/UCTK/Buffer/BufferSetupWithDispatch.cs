using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCTK {

	/// <summary>
	/// Custom version of BufferSetup where a ComputeShader is used to create the data (useful for AppendStructuredBuffers)
	/// </summary>
	public class BufferSetupWithDispatch : BufferSetup {

		/// <summary>
		/// ComputeDispatcher to be called during the CreateData stage
		/// </summary>
		public ComputeDispatcher Dispatcher;
		/// <summary>
		/// Number of thread groups to use when dispatching the setup compute shader
		/// </summary>
		public Vector3Int ThreadGroups;

		protected override void CreateData() {
			Dispatcher.Dispatch(ThreadGroups.x, ThreadGroups.y, ThreadGroups.z);
		}
	}
}