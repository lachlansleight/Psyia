using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCTK;

namespace Psyia {

	public class RenderingManager : MonoBehaviour {

		public Material ParticleMaterial {
			get {
				return Renderer.RenderMaterial;
			} set {
				if(Renderer.RenderMaterial != value) {
					Debug.Log("Setting material to " + (value == null ? "null" : value.name));
				}
				if(Application.isPlaying) {
					Renderer.SupplyNewMaterial(value);
				} else {
					Renderer.SetMaterialInEditor(value);
				}
			}
		}

		public ComputeRenderer Renderer;

		public float ChromaY = 0.2f;

		void Awake() {
			Renderer.SupplyNewMaterial(ParticleMaterial);
		}

	}
}