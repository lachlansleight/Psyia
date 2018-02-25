using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foliar.Compute {

	public class ComputeRenderer : MonoBehaviour {

		public GPUBuffer Buffer;
		public string BufferName;
		public Material RenderMaterial;

		void Awake() {
			RenderMaterial.SetBuffer(BufferName, Buffer.Buffer);
		}

		void OnRenderObject() {
			RenderMaterial.SetPass(0);
			Graphics.DrawProcedural(MeshTopology.Points, Buffer.Count);
		}
	}

}