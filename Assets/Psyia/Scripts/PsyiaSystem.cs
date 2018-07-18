using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Psyia {
	public class PsyiaSystem : MonoBehaviour {

		[System.Serializable]
		public class GlobalSettings {
			[Range(1, 1024)]
			public int MaxParticleCount = 10;
			public float DefaultParticleSize = 0.01f;
			public Color DefaultParticleColor = Color.white;
			public float DefaultParticleLifetime = 5f;
		}

		[System.Serializable]
		public class EmissionSettings {
			public List<PsyiaEmitter> Emitters = new List<PsyiaEmitter>();
		}

		[System.Serializable]
		public class SizeOverLifetimeSettings {
			public enum AnimationMode {
				None,
				Linear,
				Curve
			}
			public AnimationMode Mode = AnimationMode.None;
			public Vector2 LinearAnimation = new Vector2(1f, 0f);
			public AnimationCurve CurveAnimation = AnimationCurve.Constant(0f, 1f, 1f);
		}

		[System.Serializable]
		public class ColorOverLifetimeSettings {
			public enum AnimationMode {
				None,
				Linear,
				Gradient
			}
			public AnimationMode Mode = AnimationMode.None;
			public Color StartColor = Color.white;
			public Color EndColor = Color.white;
			public Gradient ColorGradient = new Gradient();
		}

		[System.Serializable]
		public class PhysicsSettings {
			[Range(0.01f, 10f)]
			public float ParticleMass = 1f;
			[Range(0f, 1f)]
			public float ParticleDrag;
			public List<PsyiaForce> Forces = new List<PsyiaForce>();
		}

		[System.Serializable]
		public class RenderingSettings {
			public Material ParticleMaterial;
			public enum PsyiaSortMode {
				None,
				BackToFront,
				OldToYoung,
				YoungToOld
			}
			public PsyiaSortMode SortMode = PsyiaSortMode.None;
			[Range(0f, 1f)]
			public float GlobalAlphaMultiplier = 1f;
		}

		public GlobalSettings Global;
		public EmissionSettings Emission;
		public SizeOverLifetimeSettings SizeOverLifetime;
		public ColorOverLifetimeSettings ColorOverLifetime;
		public PhysicsSettings Physics;
		public RenderingSettings Rendering;
	}
}