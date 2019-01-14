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
using UCTK;

namespace Psyia {
	public class PsyiaCoreParticleEmitter : MonoBehaviour {

		public GpuAppendBuffer AppendBuffer;
		public ComputeDispatcher Emitter;

		private int _currentCount;
		

		public void Emit(Vector3 Position, int Amount) { Emit(Position, Vector3.zero, Amount); }
		public void Emit(Vector3 Position, Vector3 Velocity, int Amount) {
			Emitter.Shader.SetVector("SpawnPosition", Position);
			Emitter.Shader.SetVector("SpawnVelocity", Velocity);
			Emitter.Shader.SetVector("LastSpawnPosition", Position);
			Emitter.Shader.SetVector("LastSpawnVelocity", Velocity);

			float AppendBufferCount = AppendBuffer.CurrentCount;
			int NumberToEmit = (int)Mathf.Min(Amount, AppendBufferCount);
			if(NumberToEmit > 0) {
				Emitter.Dispatch(NumberToEmit, 1, 1);
			}
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

			int NumberToEmit = Mathf.Min(Amount, _currentCount);
			if(NumberToEmit > 0) {
				Emitter.Dispatch(NumberToEmit, 1, 1);
			}
			
			//Unfortunately, this line does what we want but it's so expensive we just can't call it every frame
			//Takes literally 30ms to complete
			//AppendBuffer.SetCurrentCountDirty();
		}

		public void LateUpdate()
		{
			_currentCount = AppendBuffer.CurrentCount;
		}
	}
}