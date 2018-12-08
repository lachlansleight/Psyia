using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Psyia {

	public class GlobalManager : MonoBehaviour {

		[Range(1, 1024)]
		public int MaxParticleCount = 10;
		public float DefaultParticleSize = 0.01f;
		//public Color DefaultParticleColor = Color.white;
		public float ParticleLifetime = 5f;

		public ParticleCountManager CountManager;

		void Awake() {
			CountManager.ParticleCountFactor = MaxParticleCount;
			CountManager.ApplyParticleCount();
		}
	}

}