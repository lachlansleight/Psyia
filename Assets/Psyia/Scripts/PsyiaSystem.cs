using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Psyia {
	public class PsyiaSystem : MonoBehaviour {

		/*
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
		*/

		private GlobalManager _Global;
		public GlobalManager Global {
			get {
				if(_Global == null) {
					_Global = GetComponentInChildren<GlobalManager>();
				}
				return _Global;
			}
		}
		private EmissionManager _Emission;
		public EmissionManager Emission {
			get {
				if(_Emission == null) {
					_Emission = GetComponentInChildren<EmissionManager>();
				}
				return _Emission;
			}
		}

		private PhysicsManager _Physics;
		public PhysicsManager Physics {
			get {
				if(_Physics == null) {
					_Physics = GetComponentInChildren<PhysicsManager>();
				}
				return _Physics;
			}
		}
		private RenderingManager _Renderer;
		public RenderingManager Renderer {
			get {
				if(_Renderer == null) {
					_Renderer = GetComponentInChildren<RenderingManager>();
				}
				return _Renderer;
			}
		}
	}
}