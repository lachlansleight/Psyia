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

	public class PsyiaEmitter : MonoBehaviour {

		private PsyiaCoreParticleEmitter _MainEmitter;
		private PsyiaCoreParticleEmitter MainEmitter {
			get {
				if(_MainEmitter == null) {
					/*
					Transform MainParent = transform.parent;
					while(MainParent.GetComponent<PsyiaSystem>() == null && MainParent != null) {
						MainParent = MainParent.parent;
					}
					if(MainParent == null) {
						Debug.LogError("Failed to find source psyia system!");
						return null;
					}
					*/

					_MainEmitter = GameObject.FindObjectOfType<PsyiaCoreParticleEmitter>();
				}
				return _MainEmitter;
			}
		}

		public enum PsyiaEmitterMode {
			Time,
			Start,
			Distance,
			Manual
		}

		[SerializeField]
		private PsyiaEmitterMode _Mode;
		public PsyiaEmitterMode Mode {
			get {
				return _Mode;
			} set {
				if(value == PsyiaEmitterMode.Time) {
					TimeSinceLastEmit = 0;
				} else if(value == PsyiaEmitterMode.Distance) {
					DistanceSinceLastEmit = 0;
				}

				_Mode = value;
			}
		}

		[SerializeField]
		private int _StartEmitCount;
		public int StartEmitCount { 
			get { return _StartEmitCount; } 
			set { _StartEmitCount = value; } 
		}
		[SerializeField]
		private float _EmitOverTime;
		public float EmitOverTime { 
			get { return _EmitOverTime; }
			set { _EmitOverTime = value; }
		}
		[SerializeField]
		private float _EmitOverDistance;
		public float EmitOverDistance { 
			get { return _EmitOverDistance; }
			set { _EmitOverDistance = value; } 
		}

		[SerializeField]
		private float _EmissionMultiplier;
		public float EmissionMultiplier { 
			get { return _EmissionMultiplier; }
			set { _EmissionMultiplier = value; }
		}

		private float DistanceSinceLastEmit = 0f;
		private float TimeSinceLastEmit = 0f;

		private Matrix4x4 LastTransform;
		private Vector3 LastVelocity = Vector3.zero;
		private Vector3 Velocity = Vector3.zero;
		private Vector3 PositionLastFrame = Vector3.zero;

		Matrix4x4 GetCurrentMatrix() {
			return Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
		}

		//default values
		void Reset() {
			_Mode = PsyiaEmitterMode.Time;

			StartEmitCount = 1000;
			EmitOverTime = 10;
			EmitOverDistance = 10;
			EmissionMultiplier = 1f;

			DistanceSinceLastEmit = 0f;
			TimeSinceLastEmit = 0f;

			LastVelocity = Velocity = Vector3.zero;
			LastTransform = GetCurrentMatrix();
		}

		[System.Serializable]
		public class EmitterSettings {
			public float EmissionRadius;
			public float EmissionRadiusShell;
			public float EmissionArcSize;
			public float MinSpawnVelocity;
			public float MaxSpawnVelocity;
			public float MinSpawnMass;
			public float MaxSpawnMass;
			public float InheritVelocity;
			public float RandomiseDirection;

			public EmitterSettings() {
				EmissionRadius = 0.02f;
				EmissionRadiusShell = 1f;
				EmissionArcSize = 1f;
				MinSpawnVelocity = 0.15f;
				MaxSpawnVelocity = 0.2f;
				MinSpawnMass = 0.05f;
				MaxSpawnMass = 0.15f;
				InheritVelocity = 0.05f;
				RandomiseDirection = 0f;
			}
		}

		public EmitterSettings Settings;

		void Start() {
			LastTransform = GetCurrentMatrix();
			LastVelocity = Velocity = Vector3.zero;
			PositionLastFrame = transform.position;
			if(Mode == PsyiaEmitterMode.Start) {
				Emit(StartEmitCount);
			}
		}

		void Update() {

			int ParticlesToEmit = GetParticleEmitCount();
			if(ParticlesToEmit > 0) {
				Emit(ParticlesToEmit);

				TimeSinceLastEmit = 0f;
				DistanceSinceLastEmit = 0f;
			} else {
				DistanceSinceLastEmit += (transform.position - (Vector3)LastTransform.GetColumn(3)).magnitude;
				TimeSinceLastEmit += Time.deltaTime;
			}

			LastVelocity = Velocity;
			Velocity = (transform.position - PositionLastFrame) / Time.deltaTime;
			PositionLastFrame = transform.position;
		}

		int GetParticleEmitCount() {
			if(Mode == PsyiaEmitterMode.Time) {
				return Mathf.RoundToInt(TimeSinceLastEmit * EmitOverTime * EmissionMultiplier);
			} else if(Mode == PsyiaEmitterMode.Distance) {
				return Mathf.RoundToInt(DistanceSinceLastEmit * EmitOverDistance * EmissionMultiplier);
			} else {
				return 0;
			}
		}

		public void Emit(int Count)
		{
			if (TimeSinceLastEmit > Time.deltaTime * 3f) UpdateTransform();
			
			MainEmitter.Emit(GetCurrentMatrix(), LastTransform, LastVelocity, Velocity, Count, Settings);
			TimeSinceLastEmit = 0;
			DistanceSinceLastEmit = 0;
			UpdateTransform();
		}

		public void UpdateTransform()
		{
			LastTransform = GetCurrentMatrix();
		}
	}

}