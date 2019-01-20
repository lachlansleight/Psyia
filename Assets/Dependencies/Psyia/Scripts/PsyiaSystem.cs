/*

Copyright (c) 2018 Lachlan Sleight

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

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