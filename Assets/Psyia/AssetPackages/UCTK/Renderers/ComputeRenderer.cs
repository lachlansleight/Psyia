using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCTK {

	public class ComputeRenderer : MonoBehaviour {

		[System.Serializable]
		public class ComputeRendererInputBuffer {
			public string Name;

			[HideInInspector] public ComputeBuffer SetComputeBuffer;
			[HideInInspector] public GpuBuffer SetBuffer;
			public GpuBuffer MainBuffer;

		}

		public MeshTopology Topology = MeshTopology.Points;
		public ComputeRendererInputBuffer[] InputBuffers;
		public int BufferToCount;

		public Material RenderMaterial;

		public Material[] AvailableMaterials;

		void OnRenderObject() {
			if(BufferToCount >= InputBuffers.Length) BufferToCount = InputBuffers.Length - 1;
			if(BufferToCount < 0) BufferToCount = 0;

			TryAssignBuffers();
			RenderMaterial.SetPass(0);
			Graphics.DrawProcedural(Topology, InputBuffers[BufferToCount].MainBuffer.Count);
		}

		private void Update() {
			
		}

		void TryAssignBuffers() {
			for(int i = 0; i < InputBuffers.Length; i++) {
				if(InputBuffers[i].SetBuffer == null) {
					InputBuffers[i].SetBuffer = InputBuffers[i].MainBuffer;
					InputBuffers[i].SetComputeBuffer = InputBuffers[i].MainBuffer.Buffer;

					for(int j = 0; j < AvailableMaterials.Length; j++) AvailableMaterials[j].SetBuffer(InputBuffers[i].Name, InputBuffers[i].SetComputeBuffer);
				} else if(InputBuffers[i].SetBuffer !=InputBuffers[i]. MainBuffer) {
					InputBuffers[i].SetBuffer = InputBuffers[i].MainBuffer;
					InputBuffers[i].SetComputeBuffer = InputBuffers[i].MainBuffer.Buffer;

					for(int j = 0; j < AvailableMaterials.Length; j++) AvailableMaterials[j].SetBuffer(InputBuffers[i].Name, InputBuffers[i].SetComputeBuffer);
				} else if(InputBuffers[i].SetComputeBuffer == null) {
					InputBuffers[i].SetComputeBuffer = InputBuffers[i].SetBuffer.Buffer;

					for(int j = 0; j < AvailableMaterials.Length; j++) AvailableMaterials[j].SetBuffer(InputBuffers[i].Name, InputBuffers[i].SetComputeBuffer);
				} else if(InputBuffers[i].SetComputeBuffer != InputBuffers[i].SetBuffer.Buffer) {
					InputBuffers[i].SetComputeBuffer = InputBuffers[i].SetBuffer.Buffer;

					for(int j = 0; j < AvailableMaterials.Length; j++) AvailableMaterials[j].SetBuffer(InputBuffers[i].Name, InputBuffers[i].SetComputeBuffer);
				}
			}
		}
	}

}