using UnityEngine;
using System.Collections;


namespace VRTools
{
	namespace UI {

		public enum VRUI_Vector_SimulationSpace {
			Local,
			World
		}

		public class VRUI_Vector : VRUI_Control {
			private Quaternion rotationTarget = Quaternion.identity;
			private float rotationLerpRate = 0.5f;

			public Vector3 startRotation = new Vector3(0f, 0f, 1f);

			public VRUI_Vector_SimulationSpace SimulationSpace = VRUI_Vector_SimulationSpace.Local;

			Vector3 lastHapticVector = Vector3.one;

			public override void Awake() {
				base.Awake();

				if(SimulationSpace.Equals(VRUI_Vector_SimulationSpace.Local)) {
					base.ActiveElement.localRotation = Quaternion.Euler(startRotation);
				} else {
					base.ActiveElement.rotation = Quaternion.Euler(startRotation);
				}
				base.ChangeVector3Value(GetVectorFromRotation());
			}

			public override void Active(Vector3 inputPos) {
				base.Active(inputPos);

				rotationTarget = Quaternion.LookRotation(inputPos - base.ActiveElement.position);
			}

			public override void SetVector3Value(Vector3 newValue) {
				base.SetVector3Value(newValue);
				if(SimulationSpace.Equals(VRUI_Vector_SimulationSpace.Local)) {
					base.ActiveElement.localRotation = Quaternion.Euler(newValue);
				} else {
					base.ActiveElement.rotation = Quaternion.Euler(newValue);
				}
				base.ChangeVector3Value(GetVectorFromRotation());
			}

			private Vector3 GetVectorFromRotation() {
				if(SimulationSpace.Equals(VRUI_Vector_SimulationSpace.Local)) {
					return base.ActiveElement.localRotation * Vector3.forward;
				} else {
					return base.ActiveElement.rotation * Vector3.forward;
				}
			}
			
			public override void Update() {
				base.Update();

				if(base.currentState.Equals(VRUI_State.Down)) {
					base.ActiveElement.rotation = Quaternion.Lerp(base.ActiveElement.rotation, rotationTarget, rotationLerpRate);
					base.ChangeVector3Value(GetVectorFromRotation());

					float angleDifferential = Vector3.Angle(base.ActiveElement.forward, lastHapticVector);
					if(angleDifferential > 15f) {
						lastHapticVector = base.ActiveElement.forward;
						base.HapticPulse(0.1f);
					}
				}
			}

			public override void SetDefaultValue() {
				SetVector3Value(startRotation);
			}
		}

	}
}