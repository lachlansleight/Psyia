using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCTK {

	public class ComputeDispatcher : MonoBehaviour, IDispatchable {

		[System.Serializable]
		public class ComputeDispatcherTargetBuffer {
			public string Name;

			[HideInInspector] public ComputeBuffer SetComputeBuffer;
			[HideInInspector] public GpuBuffer SetBuffer;
			public GpuBuffer MainBuffer;

		}

		[System.Serializable]
		public class ComputeDispatcherTargetTexture {
			public string Name;
			public Texture2D Texture;
		}

		public bool AutoDispatch;
		public int AutoDispatchInterval = 1;
		private int AutoDispatchCountdown = 0;

		public ComputeShader Shader;

		public ComputeDispatcherTargetBuffer[] TargetBuffers;
		public ComputeDispatcherTargetTexture[] TargetTextures;

		private int _KernelIndex = -1;
		private int KernelIndex {
			get {
				if(_KernelIndex == -1) {
					if(Shader != null) _KernelIndex = Shader.FindKernel(KernelName);
				}
				return _KernelIndex;
			}
		}
		public string KernelName;

		public int ThreadGroupsX = 1;
		public int ThreadGroupsY = 1;
		public int ThreadGroupsZ = 1;

		

		private void Awake() {
			//MainBuffer.SetType(typeof(ComputeStruct));
			AutoDispatchCountdown = AutoDispatchInterval;
		}

		private void Update() {
			
		}

		private void OnRenderObject() {
			if(AutoDispatch) {
				AutoDispatchCountdown -= 1;
				if(AutoDispatchCountdown <= 0) {
					Dispatch();
					AutoDispatchCountdown = AutoDispatchInterval;
				}
			}
		}

		public virtual void Dispatch() {
			TryAssignBuffers();
			TryAssignTextures();
			Shader.Dispatch(KernelIndex, ThreadGroupsX, ThreadGroupsY, ThreadGroupsZ);
		}

		public virtual void Dispatch(int OverrideX, int OverrideY, int OverrideZ) {
			TryAssignBuffers();
			TryAssignTextures();
			Shader.Dispatch(KernelIndex, OverrideX, OverrideY, OverrideZ);
		}

		void TryAssignBuffers() {
			for(int i = 0; i < TargetBuffers.Length; i++) {
				if(TargetBuffers[i].SetBuffer == null) {
					TargetBuffers[i].SetBuffer = TargetBuffers[i].MainBuffer;
					TargetBuffers[i].SetComputeBuffer = TargetBuffers[i].MainBuffer.Buffer;

					Shader.SetBuffer(KernelIndex, TargetBuffers[i].Name, TargetBuffers[i].SetComputeBuffer);
				} else if(TargetBuffers[i].SetBuffer !=TargetBuffers[i]. MainBuffer) {
					TargetBuffers[i].SetBuffer = TargetBuffers[i].MainBuffer;
					TargetBuffers[i].SetComputeBuffer = TargetBuffers[i].MainBuffer.Buffer;

					Shader.SetBuffer(KernelIndex, TargetBuffers[i].Name, TargetBuffers[i].SetComputeBuffer);
				} else if(TargetBuffers[i].SetComputeBuffer == null) {
					TargetBuffers[i].SetComputeBuffer = TargetBuffers[i].SetBuffer.Buffer;

					Shader.SetBuffer(KernelIndex, TargetBuffers[i].Name, TargetBuffers[i].SetComputeBuffer);
				} else if(TargetBuffers[i].SetComputeBuffer != TargetBuffers[i].SetBuffer.Buffer) {
					TargetBuffers[i].SetComputeBuffer = TargetBuffers[i].SetBuffer.Buffer;

					Shader.SetBuffer(KernelIndex, TargetBuffers[i].Name, TargetBuffers[i].SetComputeBuffer);
				}
			}
		}

		void TryAssignTextures() {
			for(int i = 0; i < TargetTextures.Length; i++) {
				if(TargetTextures[i].Texture != null) Shader.SetTexture(KernelIndex, TargetTextures[i].Name, TargetTextures[i].Texture);
			}
		}

		public virtual bool GetAutoDispatch() {
			return AutoDispatch;
		}
		public virtual void SetAutoDispatch(bool NewValue) {
			AutoDispatch = NewValue;
		}
	}

}