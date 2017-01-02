using UnityEngine;
using System.Collections;


namespace VRTools
{
	namespace UI {

		public class VRUI_Dial : VRUI_Control {
			private Quaternion targetRot = Quaternion.identity;
			private float rotLerpRate = 0.5f;

			
			[Space(20)]public float minValue = 0f;
			public float maxValue = 1f;
			public float defaultValue = 0.5f;

			private float minRotation = 15f;
			private float maxRotation = 345f;

			private float lastHapticRotation = 1000f;

			public override void Awake() {
				base.Awake();

				base.ActiveElement.localRotation = GetRotFromValue(defaultValue);
				base.ChangeFloatValue(GetValueFromRot());
			}

			public override void SetFloatValue(float newValue) {
				base.SetFloatValue(newValue);
				base.ActiveElement.localRotation = GetRotFromValue(newValue);
				base.ChangeFloatValue(GetValueFromRot());
			}

			public override void Active(Vector3 inputPos) {
				base.Active(inputPos);

				if(base.currentState.Equals(VRUI_State.Down)) {
					Vector3 lookVector = (inputPos - base.ActiveElement.position).normalized;
					Vector3 transformed = base.ActiveElement.parent.InverseTransformDirection(lookVector);
					transformed.z = 0f;
					transformed.Normalize();
					float rotation = Mathf.Atan2(transformed.y, transformed.x) * Mathf.Rad2Deg;
					rotation += 90f;
					while(rotation < 0f) rotation += 360f;
					while(rotation > 360f) rotation -= 360f;
					rotation = Mathf.Clamp(rotation, minRotation, maxRotation);
					targetRot = Quaternion.Euler(new Vector3(0f, 0f, rotation));
				}
			}

			public Quaternion GetRotFromValue(float value) {
				float targetYrotation = Mathf.Lerp(minRotation, maxRotation, Mathf.InverseLerp(minValue, maxValue, value));
				targetYrotation = Mathf.Clamp(targetYrotation, minRotation, maxRotation);
				return Quaternion.Euler(0f, 0f, targetYrotation);
			}

			public float GetValueFromRot() {
				float yRotation = base.ActiveElement.localRotation.eulerAngles.z;
				while(yRotation < 0f) yRotation += 360f;
				while(yRotation > 360f) yRotation -= 360f;
				return Mathf.Lerp(minValue, maxValue, Mathf.InverseLerp(minRotation, maxRotation, yRotation));
			}
			
			public override void Update() {
				base.Update();

				if(base.currentState.Equals(VRUI_State.Down)) {
					base.ActiveElement.localRotation = Quaternion.Lerp(base.ActiveElement.localRotation, targetRot, rotLerpRate);
					base.ChangeFloatValue(GetValueFromRot());

					float rotationDifferential = Mathf.Abs(base.ActiveElement.localEulerAngles.z - lastHapticRotation);
					if(rotationDifferential > 15f) {
						lastHapticRotation = base.ActiveElement.localEulerAngles.z;
						base.HapticPulse(0.1f);
					}
				}
			}

			public override void SetDefaultValue() {
				SetFloatValue(defaultValue);
			}

		}

	}
}