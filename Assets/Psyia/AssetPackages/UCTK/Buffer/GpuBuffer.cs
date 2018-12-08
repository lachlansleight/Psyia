using UnityEngine;

namespace UCTK {

	/// <summary>
	/// Component for managing StructuredBuffer shader objects
	/// </summary>
	public class GpuBuffer : MonoBehaviour {

		/// <summary>
		/// Reference to the Buffer Setup component responsible for initializing this buffer
		/// Will attempt to find one on this GameObject if null
		/// </summary>
		public BufferSetup BufferSetup;

		/// <summary>
		/// Stride of buffer data struct type
		/// </summary>
		public int Stride {
			get {
				return System.Runtime.InteropServices.Marshal.SizeOf(DataType);
			}
		}
		/// <summary>
		/// Type of buffer data type
		/// </summary>
		public System.Type DataType;

		[HideInInspector] public ComputeBufferType BufferType;

		private ComputeBuffer _Buffer;
		/// <summary>
		/// The actual compute buffer
		/// </summary>
		public ComputeBuffer Buffer {
			get {
				if(_Buffer == null) return null;
				if(DataType == null) return null;

				return _Buffer;
			} private set {
				_Buffer = value;
			}
		}

		/// <summary>
		/// The size of the compute buffer
		/// </summary>
		/// <value></value>
		public int Count {
			get {
				return Buffer.count;
			}
		}

		public void Awake() {
			if(BufferSetup == null) {
				BufferSetup = GetComponent<BufferSetup>();
				if(BufferSetup == null) {
					Debug.LogError("Error: GpuBuffer requires BufferSetup script");
					return;
				}
			}

			BufferSetup.Setup();
		}

		/// <summary>
		/// Initializes a compute buffer, releasing and disposing it first if necessary.
		/// </summary>
		/// <param name="buffer">The buffer to be initialized - can be null</param>
		/// <param name="count">The buffer count</param>
		/// <param name="stride">The buffer stride in bytes</param>
		protected virtual void InitializeBuffer(ref ComputeBuffer buffer, int count, int stride) {
			if(buffer != null) {
				buffer.Release();
			}
			BufferType = ComputeBufferType.Default;
			buffer = new ComputeBuffer(count, stride, ComputeBufferType.Default);
		}

		/// <summary>
		/// Sets buffer count (does not initialise data)
		/// </summary>
		public virtual void SetCount(int NewCount) {
			if(DataType == null) {
				Debug.LogError("Cannot set buffer count before a type has been assigned - set buffer type first");
				return;
			}

			if(Buffer == null) InitializeBuffer(ref _Buffer, NewCount, Stride);
			else if(Count != NewCount) InitializeBuffer(ref _Buffer, NewCount, Stride);
		}

		/// <summary>
		/// Sets buffer data type
		/// </summary>
		public virtual void SetType(System.Type Type) {
			DataType = Type;
		}

		/// <summary>
		/// Set compute buffer data
		/// </summary>
		public virtual void SetData<T>(T[] data) {
			if(DataType == null) {
				SetType(typeof(T));
			}
			if(DataType != typeof(T)) {
				SetType(typeof(T));
			}
			if(data.Length != Buffer.count) {
				Debug.LogError("Cannot set buffer data - supplied array has length " + data.Length + ", buffer has count " + Buffer.count + " - set buffer count first");
				return;
			}
			Buffer.SetData(data);
		}

		/// <summary>
		/// Gets compute data as an array
		/// </summary>
		public virtual T[] GetData<T>() {
			if(DataType != typeof(T)) {
				Debug.LogError("Invalid type provided - expected " + DataType.Name + ", received " + typeof(T).Name);
				return null;
			}
			T[] OutputData = new T[Count];
			Buffer.GetData(OutputData);
			return OutputData;
		}

		protected virtual void OnDestroy() {
			if(_Buffer != null) _Buffer.Release();
		}

	}
}
