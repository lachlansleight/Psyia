using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foliar.Compute {

	public class ComputeDispatcher : MonoBehaviour {

		private ComputeBuffer SetComputeBuffer;
		private GpuBuffer SetBuffer;
		public GpuBuffer MainBuffer;

		public ComputeShader Shader;
		public string BufferName;
		private int _KernelIndex = -1;
		private int KernelIndex {
			get {
				if(_KernelIndex == -1) {
					_KernelIndex = Shader.FindKernel(KernelName);
				}
				return _KernelIndex;
			}
		}
		/*
		private string _KernelName;
		public string KernelName {
			get {
				return _KernelName;
			} set {
				_KernelName = value;
				_KernelIndex = -1;
			}
		}*/
		public string KernelName;

		public int ThreadGroupsX = 1;
		public int ThreadGroupsY = 1;
		public int ThreadGroupsZ = 1;

		ComputeStruct[] Data;

		private void Awake() {
			MainBuffer.SetType(typeof(ComputeStruct));
		}

		private void OnRenderObject() {
			TryAssignBuffers();
			Shader.Dispatch(KernelIndex, ThreadGroupsX, ThreadGroupsY, ThreadGroupsZ);
		}

		void TryAssignBuffers() {
			if(SetBuffer == null) {
				SetBuffer = MainBuffer;
				SetComputeBuffer = MainBuffer.Buffer;
				Shader.SetBuffer(KernelIndex, BufferName, SetComputeBuffer);
			} else if(SetBuffer != MainBuffer) {
				SetBuffer = MainBuffer;
				SetComputeBuffer = MainBuffer.Buffer;
				Shader.SetBuffer(KernelIndex, BufferName, SetComputeBuffer);
			} else if(SetComputeBuffer == null) {
				SetComputeBuffer = SetBuffer.Buffer;
				Shader.SetBuffer(KernelIndex, BufferName, SetComputeBuffer);
			} else if(SetComputeBuffer != SetBuffer.Buffer) {
				SetComputeBuffer = SetBuffer.Buffer;
				Shader.SetBuffer(KernelIndex, BufferName, SetComputeBuffer);
			}
		}
	}

}