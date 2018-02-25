using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foliar.Compute {

	[CreateAssetMenu(fileName = "ShaderValue", menuName = "ScriptableObjects/ShaderValue", order = 0)]
	public class ShaderValues : ScriptableObject {

		[ShaderValue]
		public Color _Color;
		
		[ComputeValue]
		public Vector4 LeftController;

		[ComputeValue]
		public Vector4 RightController;

		[ComputeValue]
		public float Damping;
		
		[ComputeValue]
		public float ParticleCharge;
		
		[ComputeValue]
		public float ParticleMass;
		
		[ComputeValue]
		public float SofteningFactor;
	}

}