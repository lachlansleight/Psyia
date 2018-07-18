using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Psyia {

	public class PsyiaEmitter : MonoBehaviour {

		private PsyiaCoreParticleEmitter MainEmitter;

		public enum PsyiaEmitterMode {
			Time,
			Start,
			Distance
		}

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

		[Range(0f, 1f)]
		public float InheritVelocity = 0f;

		private float DistanceSinceLastEmit = 0f;
		private float TimeSinceLastEmit = 0f;

		private Vector3 LastPosition = Vector3.zero;
		private Vector3 LastVelocity = Vector3.zero;
		private Vector3 Velocity = Vector3.zero;

		void Awake() {
			MainEmitter = GameObject.FindObjectOfType<PsyiaCoreParticleEmitter>();
		}

		void Start() {
			if(Mode == PsyiaEmitterMode.Start) {
				MainEmitter.Emit(transform.position, StartEmitCount);
			}
		}

		void Update() {

			int ParticlesToEmit = GetParticleEmitCount();
			if(ParticlesToEmit > 0) {
				MainEmitter.Emit(LastPosition, transform.position, LastVelocity * InheritVelocity, Velocity * InheritVelocity, ParticlesToEmit);

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

	}

}