using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Psyia {

	public class PsyiaEmitter : MonoBehaviour {

		public class EmissionSettings {
			public float EmissionRadius;
			public float EmissionVelocity;
		}

		private PsyiaCoreParticleEmitter _MainEmitter;
		private PsyiaCoreParticleEmitter MainEmitter {
			get {
				if(_MainEmitter == null) {
					Transform MainParent = transform.parent;
					while(MainParent.GetComponent<PsyiaSystem>() == null && MainParent != null) {
						MainParent = MainParent.parent;
					}
					if(MainParent == null) {
						Debug.LogError("Failed to find source psyia system!");
						return null;
					}
					_MainEmitter = MainParent.GetComponentInChildren<PsyiaCoreParticleEmitter>();
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

		public int StartEmitCount = 1000;
		public float EmitOverTime = 10;
		public float EmitOverDistance = 10;

		private float DistanceSinceLastEmit = 0f;
		private float TimeSinceLastEmit = 0f;

		private Vector3 LastPosition = Vector3.zero;
		private Vector3 LastVelocity = Vector3.zero;
		private Vector3 Velocity = Vector3.zero;

		[System.Serializable]
		public class EmitterSettings {
			public float EmissionRadius = 0.02f;
			public float EmissionVelocity = 0.05f;
			public float InheritVelocity = 0.05f;
		}

		public EmitterSettings Settings;

		void Start() {
			if(Mode == PsyiaEmitterMode.Start) {
				MainEmitter.Emit(transform.position, StartEmitCount);
			}
		}

		void Update() {

			int ParticlesToEmit = GetParticleEmitCount();
			if(ParticlesToEmit > 0) {
				MainEmitter.Emit(LastPosition, transform.position, LastVelocity, Velocity, ParticlesToEmit, Settings);

				TimeSinceLastEmit = 0f;
				DistanceSinceLastEmit = 0f;
			} else {
				DistanceSinceLastEmit += (transform.position - LastPosition).magnitude;
				TimeSinceLastEmit += Time.deltaTime;
			}

			LastVelocity = Velocity;
			Velocity = (transform.position - LastPosition) / Time.deltaTime;
			LastPosition = transform.position;
		}

		int GetParticleEmitCount() {
			if(Mode == PsyiaEmitterMode.Time) {
				return Mathf.RoundToInt(TimeSinceLastEmit * EmitOverTime);
			} else if(Mode == PsyiaEmitterMode.Distance) {
				return Mathf.RoundToInt(DistanceSinceLastEmit * EmitOverDistance);
			} else {
				return 0;
			}
		}

		public void Emit(int Count) {
			MainEmitter.Emit(LastPosition, transform.position, LastVelocity, Velocity, Count, Settings);
			TimeSinceLastEmit = 0;
			DistanceSinceLastEmit = 0;
		}
	}

}