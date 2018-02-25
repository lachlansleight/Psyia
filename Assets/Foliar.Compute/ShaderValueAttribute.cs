namespace Foliar.Compute {

	[System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	sealed class ShaderValue : System.Attribute {

		public ShaderValue() {
	
		}
	}

	[System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	sealed class TextureOffset : System.Attribute {

		public readonly string Name;

		public TextureOffset(string name) {
			Name = name;
		}
	}

	[System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	sealed class TextureScale : System.Attribute {

		public readonly string Name;

		public TextureScale(string name) {
			Name = name;
		}
	}

}