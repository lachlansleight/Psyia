using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCTK;

public class PsyiaCoreParticleEmitter : MonoBehaviour {

	public GpuAppendBuffer AppendBuffer;
	public ComputeDispatcher Emitter;

	public void Emit(Vector3 Position, int Amount) { Emit(Position, Vector3.zero, Amount); }
	public void Emit(Vector3 Position, Vector3 Velocity, int Amount) {
		Emitter.Shader.SetVector("SpawnPosition", Position);
		Emitter.Shader.SetVector("SpawnVelocity", Velocity);
		Emitter.Shader.SetVector("LastSpawnPosition", Position);
		Emitter.Shader.SetVector("LastSpawnVelocity", Velocity);

		float AppendBufferCount = AppendBuffer.CurrentCount;
		int NumberToEmit = (int)Mathf.Min(Amount, AppendBufferCount);
		Emitter.Dispatch(NumberToEmit, 1, 1);
	}
	public void Emit(Vector3 StartPosition, Vector3 EndPosition, Vector3 StartVelocity, Vector3 EndVelocity, float Amount) {
		Emitter.Shader.SetVector("SpawnPosition", EndPosition);
		Emitter.Shader.SetVector("SpawnVelocity", EndVelocity);
		Emitter.Shader.SetVector("LastSpawnPosition", StartPosition);
		Emitter.Shader.SetVector("LastSpawnVelocity", StartVelocity);

		float AppendBufferCount = AppendBuffer.CurrentCount;
		int NumberToEmit = (int)Mathf.Min(Amount, AppendBufferCount);
		Emitter.Dispatch(NumberToEmit, 1, 1);
	}
}
