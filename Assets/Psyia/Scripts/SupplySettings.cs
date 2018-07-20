using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Psyia {
	public class SupplySettings : MonoBehaviour {

		public ComputeShader SimulateShader;
		public ComputeShader ColorShader;

		public GlobalManager Global;
		public PhysicsManager Physics;
		public RenderingManager Rendering;

		public void SetDamping(float Value) {
			SimulateShader.SetFloat("Damping", Value);
		}
		public void SetForceMultiplier(float Value) {
			SimulateShader.SetFloat("ForceMultiplier", Value);
		}
		public void SetMass(float Value) {
			SimulateShader.SetFloat("ParticleMass", Value);
		}
		public void SetLifespan(float Value) {
			SimulateShader.SetFloat("Lifespan", Value);
			ColorShader.SetFloat("Lifespan", Value);
		}
		public void SetChromaY(float Value) {
			ColorShader.SetFloat("Y", Value);
		}

		void Update() {
			SetDamping(Physics.ParticleDrag);
			SetForceMultiplier(Physics.ForceMultiplier);
			SetMass(Physics.ParticleMass);
			SetLifespan(Global.ParticleLifetime);
			SetChromaY(Rendering.ChromaY);
		}

	}
}