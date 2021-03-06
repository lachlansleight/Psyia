﻿/*

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
using UCTK;

namespace Psyia {
	public class PsyiaCoreParticleEmitter : MonoBehaviour {

		public GpuBuffer ParticleBuffer;
		public ComputeDispatcher Emitter;

		public void Emit(Vector3 Position, int Amount) { Emit(Position, Vector3.zero, Amount); }
		public void Emit(Vector3 Position, Vector3 Velocity, int Amount) {
			Emitter.Shader.SetVector("SpawnPosition", Position);
			Emitter.Shader.SetVector("SpawnVelocity", Velocity);
			Emitter.Shader.SetVector("LastSpawnPosition", Position);
			Emitter.Shader.SetVector("LastSpawnVelocity", Velocity);

			DispatchEmit(Amount);
		}
		public void Emit(Matrix4x4 StartTransform, Matrix4x4 LastTransform, Vector3 StartVelocity, Vector3 EndVelocity, int Amount, PsyiaEmitter.EmitterSettings Settings) {
			Emitter.Shader.SetMatrix("SpawnTransform", StartTransform);
			Emitter.Shader.SetVector("SpawnVelocity", EndVelocity);
			Emitter.Shader.SetMatrix("LastSpawnTransform", LastTransform);
			Emitter.Shader.SetVector("LastSpawnVelocity", StartVelocity);

			Emitter.Shader.SetFloat("SpawnRadius", Settings.EmissionRadius);
			Emitter.Shader.SetFloat("SpawnRadiusShell", Settings.EmissionRadiusShell);
			Emitter.Shader.SetFloat("SpawnArcSize", Settings.EmissionArcSize);

			Emitter.Shader.SetFloat("SpawnInheritVelocity", Settings.InheritVelocity);
			Emitter.Shader.SetFloat("MinSpawnVelocity", Settings.MinSpawnVelocity);
			Emitter.Shader.SetFloat("MaxSpawnVelocity", Settings.MaxSpawnVelocity);

			Emitter.Shader.SetFloat("MinSpawnMass", Settings.MinSpawnMass);
			Emitter.Shader.SetFloat("MaxSpawnMass", Settings.MaxSpawnMass);
			
			Emitter.Shader.SetFloat("RandomiseDirection", Settings.RandomiseDirection);

			DispatchEmit(Amount);
		}

		private void DispatchEmit(float amount)
		{
			var emitSpan = amount / ParticleBuffer.Count;
			var emitMin = Random.Range(0f, 1f - emitSpan);
			//Debug.Log($"Emission properties [maxcount, amount, span, min]: [{ParticleBuffer.Count}, {amount}, {emitSpan}, {emitMin}]");
			Emitter.Shader.SetFloat("EmitSpan", emitSpan);
			Emitter.Shader.SetFloat("EmitMin", emitMin);
			Emitter.Dispatch();
			Emitter.Shader.SetFloat("EmitSpan", 0f);
			Emitter.Shader.SetFloat("EmitMin", 2f);
		}
	}
}