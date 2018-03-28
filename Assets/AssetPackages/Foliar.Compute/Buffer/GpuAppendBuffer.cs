using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foliar.Compute {

	public class GpuAppendBuffer : GpuBuffer {

		private ComputeBuffer _ArgsBuffer;
		private ComputeBuffer ArgsBuffer {
			get {
				if(_ArgsBuffer == null) {
					_ArgsBuffer = new ComputeBuffer(4, sizeof(int), ComputeBufferType.IndirectArguments);
				}
				return _ArgsBuffer;
			} set {
				_ArgsBuffer = value;
			}
		}

		private int FrameOfLastCurrentCountRetreive;
		private int LastCurrentCount = -1;

		public int CurrentCount {
			get {
				if(LastCurrentCount == -1) {
					int[] args = new int[]{ 0, 1, 0, 0 };
					ArgsBuffer.SetData(args);

					ComputeBuffer.CopyCount(base.Buffer, ArgsBuffer, 0);
					ArgsBuffer.GetData(args);

					LastCurrentCount = args[0];	
				}
				return LastCurrentCount;
			}
		}

		public override T[] GetData<T>() {
			return base.GetData<T>();
		}

		public override void SetCount(int NewCount) {
			base.SetCount(NewCount);
			SetCounterValue(0);
		}

		public override void SetData<T>(T[] data) {
			base.SetData(data);
		}

		public override void SetType(Type Type) {
			base.SetType(Type);
		}

		protected override void InitializeBuffer(ref ComputeBuffer buffer, int count, int stride) {
			if(buffer != null) {
				buffer.Release();
			}
			base.BufferType = ComputeBufferType.Append;
			buffer = new ComputeBuffer(count, stride, ComputeBufferType.Append);
			SetCounterValue(0);
		}

		public void SetCounterValue(uint NewValue) {
			base.Buffer.SetCounterValue(NewValue);
		}

		protected override void OnDestroy() {
			base.OnDestroy();

			if(_ArgsBuffer != null) _ArgsBuffer.Release();
		}
	}

}