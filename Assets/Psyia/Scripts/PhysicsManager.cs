using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Psyia {
	public class PhysicsManager : MonoBehaviour {

		[Range(0.01f, 10f)]
		public float ParticleMass = 1f;
		[Range(0f, 1f)]
		public float ParticleDrag;
		public float ForceMultiplier = 1f;
		
		public ForceManager Forces;
	}
}