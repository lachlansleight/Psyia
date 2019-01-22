/*

Copyright (c) 2018 Lachlan Sleight

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

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
			SimulateShader.SetFloat("ParticleMinimumMass", Value);
		}
		public void SetLifespan(float Value) {
			SimulateShader.SetFloat("Lifespan", Value);
			ColorShader.SetFloat("Lifespan", Value);
		}
		public void SetChromaY(float Value) {
			ColorShader.SetFloat("Y", Value);
		}

		public void SetFloorCollision(bool Value)
		{
			SimulateShader.SetInt("FloorCollision", Value ? 1 : 0);
		}

		void Update()
		{
			SetFloorCollision(Physics.FloorCollision);
			SetDamping(Physics.ParticleDrag);
			SetForceMultiplier(Physics.ForceMultiplier);
			SetMass(Physics.ParticleMinimumMass);
			SetLifespan(Global.ParticleLifetime);
			SetChromaY(Rendering.ChromaY);
		}

	}
}