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

		private List<Material> _CachedMaterials;
		private List<Material> CachedMaterials {
			get {
				if(_CachedMaterials == null) {
					_CachedMaterials = new List<Material>();
				}
				return _CachedMaterials;
			} set {
				_CachedMaterials = value;
			}
		}

		void Awake() {
			SupplyNewMaterial(RenderMaterial);
		}

		void OnRenderObject() {
			if(RenderMaterial == null) return;

			if(BufferToCount >= InputBuffers.Length) BufferToCount = InputBuffers.Length - 1;
			if(BufferToCount < 0) BufferToCount = 0;

			SupplyBuffersToMaterial(RenderMaterial);

			RenderMaterial.SetPass(0);
			Graphics.DrawProcedural(Topology, InputBuffers[BufferToCount].MainBuffer.Count);
		}

		private void Update() {
			
		}

		void SupplyBuffersToMaterials(Material[] mats) {
			for(int i = 0; i < InputBuffers.Length; i++) {
				for(int j = 0; j < mats.Length; j++) {
					mats[j].SetBuffer(InputBuffers[i].Name, InputBuffers[i].MainBuffer.Buffer);
				}
			}
		}

		void SupplyBuffersToMaterial(Material mat) {
			for(int i = 0; i < InputBuffers.Length; i++) {
				mat.SetBuffer(InputBuffers[i].Name, InputBuffers[i].MainBuffer.Buffer);
			}
			/*
			for(int i = 0; i < InputBuffers.Length; i++) {
				if(InputBuffers[i].SetBuffer == null) {
					InputBuffers[i].SetBuffer = InputBuffers[i].MainBuffer;
					InputBuffers[i].SetComputeBuffer = InputBuffers[i].MainBuffer.Buffer;

					mat.SetBuffer(InputBuffers[i].Name, InputBuffers[i].SetComputeBuffer);
				} else if(InputBuffers[i].SetBuffer !=InputBuffers[i]. MainBuffer) {
					InputBuffers[i].SetBuffer = InputBuffers[i].MainBuffer;
					InputBuffers[i].SetComputeBuffer = InputBuffers[i].MainBuffer.Buffer;

					mat.SetBuffer(InputBuffers[i].Name, InputBuffers[i].SetComputeBuffer);
				} else if(InputBuffers[i].SetComputeBuffer == null) {
					InputBuffers[i].SetComputeBuffer = InputBuffers[i].SetBuffer.Buffer;

					mat.SetBuffer(InputBuffers[i].Name, InputBuffers[i].SetComputeBuffer);
				} else if(InputBuffers[i].SetComputeBuffer != InputBuffers[i].SetBuffer.Buffer) {
					InputBuffers[i].SetComputeBuffer = InputBuffers[i].SetBuffer.Buffer;

					mat.SetBuffer(InputBuffers[i].Name, InputBuffers[i].SetComputeBuffer);
				}
			}
			*/
		}

		public void SupplyNewMaterial(Material mat) {
			if(mat == null) return;
			if(!CachedMaterials.Contains(mat)) {
				CachedMaterials.Add(mat);
				SupplyBuffersToMaterial(mat);
				RenderMaterial = mat;
			} else {
				RenderMaterial = CachedMaterials[CachedMaterials.IndexOf(mat)];
			}
		}

		public void SetMaterialInEditor(Material mat) {
			RenderMaterial = mat;
		}
	}

}