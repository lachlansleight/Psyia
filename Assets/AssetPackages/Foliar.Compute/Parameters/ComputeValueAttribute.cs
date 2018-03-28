namespace Foliar.Compute {

	[System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	sealed class ComputeValue : System.Attribute {

		public ComputeValue() {
	
		}
	}

	
	[System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	sealed class ComputeKernel : System.Attribute {

		public readonly string Name;

		public ComputeKernel(string name) {
			Name = name;
		}
	}

}