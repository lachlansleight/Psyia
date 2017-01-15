using UnityEngine;
using System.Collections;


namespace VRTools
{
	namespace UI {

		public class VRUI_Slider : VRUI_Control {
			private float zPosTarget = 0f;
			private float zPosLerpRate = 0.5f;
			
			[Space(20)]
			public float minValue = 0f;
			public float maxValue = 1f;
			public float defaultValue = 0.5f;

			private float lastHapticPosition = 1000f;

			public override void Active(Vector3 inputPos) {
				base.Active(inputPos);

				if(base.currentState.Equals(VRUI_State.Down)) {
					Vector3 projectedPosition = base.ActiveElement.parent.InverseTransformPoint(inputPos);
					zPosTarget = Mathf.Clamp(projectedPosition.z, -1f, 1f);
				}
			}

			public override void SetFloatValue(float newValue) {
				base.SetFloatValue(newValue);
				base.ActiveElement.localPosition = new Vector3(0f, 0f, GetPositionFromValue(newValue));
				base.ChangeFloatValue(GetValueFromPosition());
			}

			public override void Awake() {
				base.Awake();
				base.ActiveElement.localPosition = new Vector3(0f, 0f, GetPositionFromValue(defaultValue));
				base.ChangeFloatValue(GetValueFromPosition());
			}

			float GetValueFromPosition() {
				return Mathf.Lerp(minValue, maxValue, Mathf.InverseLerp(-1f, 1f, base.ActiveElement.localPosition.z));
			}

			float GetPositionFromValue(float value) {
				return Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(minValue, maxValue, value));
			}
			
			public override void Update() {
				base.Update();

				if(base.currentState.Equals(VRUI_State.Down)) {
					Vector3 curPos = base.ActiveElement.localPosition;
					base.ActiveElement.localPosition = new Vector3(curPos.x, curPos.y, Mathf.Lerp(curPos.z, zPosTarget, zPosLerpRate));
					if(curPos.z != base.ActiveElement.localPosition.z) base.ChangeFloatValue(GetValueFromPosition());

					float posDifferential = Mathf.Abs(base.ActiveElement.localPosition.z - lastHapticPosition);
					if(posDifferential > 0.05f) {
						base.HapticPulse(0.1f);
						lastHapticPosition = base.ActiveElement.localPosition.z;
					}
				}
			}

			public override void SetDefaultValue() {
				SetFloatValue(defaultValue);
			}

		}

	}
}