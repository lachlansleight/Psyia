using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foliar.Compute;

public class ParticleEmitter : MonoBehaviour {

	public struct SpawnInfo {
		public Vector3 Position;
		public Vector3 LastPosition;
		public Vector3 Velocity;
		public Vector3 LastVelocity;
	}

	public GpuAppendBuffer AppendBuffer;
	public ComputeDispatcher Emitter;

	[Range(0, 1024)] public int EmitCount = 1024;

	SpawnInfo LeftInfo;
	SpawnInfo RightInfo;

	public void Emit() {
		float AppendBufferCount = AppendBuffer.CurrentCount;
		int NumberEmitted = 0;

		if(VRTK_Devices.TouchpadPressed(VRDevice.Left)) {
			if(AppendBufferCount > 0) {
				LeftInfo.LastPosition = LeftInfo.Position;
				LeftInfo.LastVelocity = LeftInfo.Velocity;

				LeftInfo.Position = VRTK_Devices.Position(VRDevice.Left);
				LeftInfo.Velocity = VRTK_Devices.Velocity(VRDevice.Left);

				Emitter.Shader.SetVector("SpawnPosition", LeftInfo.Position);
				Emitter.Shader.SetVector("SpawnVelocity", LeftInfo.Velocity);

				if(VRTK_Devices.TouchpadPressDown(VRDevice.Left)) {
					Emitter.Shader.SetVector("LastSpawnPosition", LeftInfo.Position);
					Emitter.Shader.SetVector("LastSpawnVelocity", LeftInfo.Velocity);
				} else {
					Emitter.Shader.SetVector("LastSpawnPosition", LeftInfo.LastPosition);
					Emitter.Shader.SetVector("LastSpawnVelocity", LeftInfo.LastVelocity);
				}

				int NumberToEmit = (int)Mathf.Min(EmitCount, AppendBufferCount);
				Emitter.Dispatch(NumberToEmit, 1, 1);
				NumberEmitted += NumberToEmit;
			}
		}

		AppendBufferCount -= NumberEmitted;
		
		if(VRTK_Devices.TouchpadPressed(VRDevice.Right)) {
			if(AppendBufferCount > 0) {
				RightInfo.LastPosition = RightInfo.Position;
				RightInfo.LastVelocity = RightInfo.Velocity;

				RightInfo.Position = VRTK_Devices.Position(VRDevice.Right);
				RightInfo.Velocity = VRTK_Devices.Velocity(VRDevice.Right);

				Emitter.Shader.SetVector("SpawnPosition", RightInfo.Position);
				Emitter.Shader.SetVector("SpawnVelocity", RightInfo.Velocity);

				if(VRTK_Devices.TouchpadPressDown(VRDevice.Right)) {
					Emitter.Shader.SetVector("LastSpawnPosition", RightInfo.Position);
					Emitter.Shader.SetVector("LastSpawnVelocity", RightInfo.Velocity);
				} else {
					Emitter.Shader.SetVector("LastSpawnPosition", RightInfo.LastPosition);
					Emitter.Shader.SetVector("LastSpawnVelocity", RightInfo.LastVelocity);
				}
				
				int NumberToEmit = (int)Mathf.Min(EmitCount, AppendBufferCount);
				Emitter.Dispatch(NumberToEmit, 1, 1);
				NumberEmitted += NumberToEmit;
			}
		}
	}
}
