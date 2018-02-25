using UnityEngine;

namespace Foliar.Compute {

	public class GPUBuffer : MonoBehaviour {

		/// <summary>
		/// Stride of buffer data struct type
		/// </summary>
		public int Stride {
			get {
				return System.Runtime.InteropServices.Marshal.SizeOf(DataType);
			}
		}
		/// <summary>
		/// Type of compute buffer
		/// </summary>
		public ComputeBufferType BufferType = ComputeBufferType.Default;
		/// <summary>
		/// Type of buffer data type
		/// </summary>
		public System.Type DataType;

		private ComputeBuffer _Buffer;
		/// <summary>
		/// The actual compute buffer
		/// </summary>
		public ComputeBuffer Buffer {
			get {
				if(_Buffer == null) {
					_Buffer = new ComputeBuffer(1, Stride);
				}
				return _Buffer;
			} private set {
				_Buffer = value;
			}
		}

		public int Count {
			get {
				return Buffer.count;
			}
		}

		/// <summary>
		/// Sets buffer count (does not initialise data)
		/// </summary>
		public void SetCount(int NewCount) {
			if(DataType == null) {
				Debug.LogError("Cannot set buffer count before a type has been assigned - set buffer type first");
				return;
			}
			Buffer = new ComputeBuffer(NewCount, Stride);
		}

		/// <summary>
		/// Sets buffer data type
		/// </summary>
		public void SetType(System.Type Type) {
			DataType = Type;
		}

		/// <summary>
		/// Set compute buffer data
		/// </summary>
		public void SetData<T>(T[] data) {
			if(DataType == null) {
				SetType(typeof(T));
			}
			if(data.Length != Buffer.count) {
				Debug.LogError("Cannot set buffer data - supplied array has length " + data.Length + ", buffer has count " + Buffer.count + " - set buffer count first");
				return;
			}
			Buffer.SetData(data);
		}

		private void OnDestroy() {
			Buffer.Release();
			Buffer.Dispose();
		}

	}
}
