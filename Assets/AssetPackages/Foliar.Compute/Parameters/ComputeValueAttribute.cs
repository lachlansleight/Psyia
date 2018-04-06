namespace Foliar.Compute {

	[System.AttributeUsage(System.AttributeTargets.All, Inherited = true, AllowMultiple = true)]
	class ComputeValue : System.Attribute {

		public ComputeValue() {
	
		}
	}


	[ComputeValue]
	class ComputeTexture : ComputeValue {

		public readonly string ShaderName;
		public readonly string KernelName;

		public ComputeTexture(string shaderName, string kernelName) {
			ShaderName = shaderName;
			KernelName = kernelName;
		}
	}

}