using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCTK {

	/// <summary>
	/// Component for managing AppendBuffer shader objects
	/// </summary>
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

		/// <summary>
		/// Current active size of the AppendBuffer
		/// WARNING: call sparingly - this will hang if the AppendBuffer is being access by a compute shader
		/// </summary>
		//TODO: Prevent this disastrous situation! Can I check whether the AppendBuffer is in use and return last value if so?
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

		/// <summary>
		/// Gets compute data as an array
		/// </summary>
		public override T[] GetData<T>() {
			return base.GetData<T>();
		}

		/// <summary>
		/// Sets buffer count (does not initialise data)
		/// </summary>
		public override void SetCount(int NewCount) {
			base.SetCount(NewCount);
			SetCounterValue(0);
		}

		/// <summary>
		/// Set compute buffer data
		/// </summary>
		public override void SetData<T>(T[] data) {
			base.SetData(data);
		}

		/// <summary>
		/// Sets buffer data type
		/// </summary>
		public override void SetType(Type Type) {
			base.SetType(Type);
		}

		/// <summary>
		/// Initializes a compute buffer, releasing and disposing it first if necessary.
		/// </summary>
		/// <param name="buffer">The buffer to be initialized - can be null</param>
		/// <param name="count">The buffer count</param>
		/// <param name="stride">The buffer stride in bytes</param>
		protected override void InitializeBuffer(ref ComputeBuffer buffer, int count, int stride) {
			if(buffer != null) {
				buffer.Release();
			}
			base.BufferType = ComputeBufferType.Append;
			buffer = new ComputeBuffer(count, stride, ComputeBufferType.Append);
			SetCounterValue(0);
		}

		/// <summary>
		/// Sets buffer counter value (note - it's on you if you set this beyond the max size of the buffer!)
		/// </summary>
		public void SetCounterValue(uint NewValue) {
			base.Buffer.SetCounterValue(NewValue);
		}

		protected override void OnDestroy() {
			base.OnDestroy();

			if(_ArgsBuffer != null) _ArgsBuffer.Release();
		}
	}

}