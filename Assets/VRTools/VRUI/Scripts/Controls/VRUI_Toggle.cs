using UnityEngine;
using System.Collections;


namespace VRTools
{
	namespace UI {

		public class VRUI_Toggle : VRUI_Control {
			public bool Toggled;

			private float offRotDeg = -60f;
			private float onRotDeg = 60f;

			private Quaternion offRot;
			private Quaternion onRot;

			private Quaternion rotTarget = Quaternion.identity;
			private float rotLerpRate = 0.5f;

			public override void Awake() {
				base.Awake();

				offRot = Quaternion.Euler(offRotDeg, 0f, 0f);
				onRot = Quaternion.Euler(onRotDeg, 0f, 0f);

				base.ActiveElement.localRotation = Toggled ? onRot : offRot;
				SetBoolValue(Toggled);
			}

			public override void SetBoolValue(bool newValue) {
				Toggled = newValue;
				base.SetBoolValue(newValue);
				base.ActiveElement.localRotation = Toggled ? onRot : offRot;
				base.ChangeBoolValue(Toggled);
			}

			public override void MakeActive(Vector3 inputPos, VRUI_Input inputDevice) {
				base.MakeActive(inputPos, inputDevice);

				Toggled = !Toggled;
				base.ChangeBoolValue(Toggled);
			}

			public override void Update() {
				base.Update();

				if(Toggled) rotTarget = onRot;
				else rotTarget = offRot;

				Quaternion currentRot = base.ActiveElement.localRotation;
				if(rotTarget != currentRot) {
					base.ActiveElement.localRotation = Quaternion.Lerp(base.ActiveElement.localRotation, rotTarget, rotLerpRate);
				}
			}



			public override void SetDefaultValue() {
				SetBoolValue(Toggled);
			}

		}

	}
}