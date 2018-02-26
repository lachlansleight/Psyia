using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foliar.Compute {

	public class ComputeRenderer : MonoBehaviour {

		private ComputeBuffer SetComputeBuffer;
		private GpuBuffer SetBuffer;
		public GpuBuffer MainBuffer;
		public string BufferName;
		public Material RenderMaterial;

		void OnRenderObject() {
			TryAssignBuffers();
			RenderMaterial.SetPass(0);
			Graphics.DrawProcedural(MeshTopology.Points, SetBuffer.Count);
		}

		void TryAssignBuffers() {
			if(SetBuffer == null) {
				SetBuffer = MainBuffer;
				SetComputeBuffer = MainBuffer.Buffer;
				RenderMaterial.SetBuffer(BufferName, SetComputeBuffer);
			} else if(SetBuffer != MainBuffer) {
				SetBuffer = MainBuffer;
				SetComputeBuffer = MainBuffer.Buffer;
				RenderMaterial.SetBuffer(BufferName, SetComputeBuffer);
			} else if(SetComputeBuffer == null) {
				SetComputeBuffer = SetBuffer.Buffer;
				RenderMaterial.SetBuffer(BufferName, SetComputeBuffer);
			} else if(SetComputeBuffer != SetBuffer.Buffer) {
				SetComputeBuffer = SetBuffer.Buffer;
				RenderMaterial.SetBuffer(BufferName, SetComputeBuffer);
			}
		}
	}

}