using UnityEngine;
using System.Collections;


namespace VRTools
{
	namespace UI {

		public class VRUI_XY : VRUI_Control {
			private float zPosTarget = 0f;
			private float zPosLerpRate = 0.5f;
			private float xPosTarget = 0f;
			private float xPosLerpRate = 0.5f;

			public Transform childElementX;
			public Transform childElementZ;

			[Space(20)]
			public float minValueX = 0f;
			public float maxValueX = 1f;
			public float defaultValueX = 0.5f;
			[Space(5)]
			public float minValueZ = 0f;
			public float maxValueZ = 1f;
			public float defaultValueZ = 0.5f;

			public Vector3 lastHapticPosition = new Vector3(1000f, 1000f, 1000f);

			public override void Active(Vector3 inputPos) {
				base.Active(inputPos);

				if(base.currentState.Equals(VRUI_State.Down)) {
					Vector3 projectedPosition = base.ActiveElement.parent.InverseTransformPoint(inputPos);
					xPosTarget = Mathf.Clamp(projectedPosition.x, -1f, 1f);
					zPosTarget = Mathf.Clamp(projectedPosition.z, -1f, 1f);
				}
			}

			public override void SetVector2Value(Vector2 newValue) {
				base.SetVector2Value(newValue);
				base.ActiveElement.localPosition = new Vector3(GetXPositionFromValue(newValue.x), 0f, GetZPositionFromValue(newValue.y));
				base.ChangeVector2Value(GetValueFromPosition());
			}

			public override void Awake() {
				base.Awake();
				base.ActiveElement.localPosition = new Vector3(GetXPositionFromValue(defaultValueX), 0f, GetZPositionFromValue(defaultValueZ));
				base.ChangeVector2Value(GetValueFromPosition());
			}

			Vector2 GetValueFromPosition() {
				float xValue = Mathf.Lerp(minValueX, maxValueX, Mathf.InverseLerp(-1f, 1f, base.ActiveElement.localPosition.x));
				float zValue = Mathf.Lerp(minValueZ, maxValueZ, Mathf.InverseLerp(-1f, 1f, base.ActiveElement.localPosition.z));

				return new Vector2(xValue, zValue);
			}

			float GetXPositionFromValue(float value) {
				return Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(minValueX, maxValueX, value));
			}
			float GetZPositionFromValue(float value) {
				return Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(minValueZ, maxValueZ, value));
			}
			
			public override void Update() {
				base.Update();

				if(base.currentState.Equals(VRUI_State.Down)) {
					Vector3 curPos = base.ActiveElement.localPosition;
					base.ActiveElement.localPosition = new Vector3(Mathf.Lerp(curPos.x, xPosTarget, xPosLerpRate), curPos.y, Mathf.Lerp(curPos.z, zPosTarget, zPosLerpRate));
					childElementX.localPosition = new Vector3(base.ActiveElement.localPosition.x, childElementX.localPosition.y, childElementX.localPosition.z);
					childElementZ.localPosition = new Vector3(childElementZ.localPosition.x, childElementZ.localPosition.y, base.ActiveElement.localPosition.z);

					if(curPos.z != base.ActiveElement.localPosition.z || curPos.x != base.ActiveElement.localPosition.x) {
						base.ChangeVector2Value(GetValueFromPosition());
					}

					float positionDifferential = (base.ActiveElement.localPosition - lastHapticPosition).magnitude;
					if(positionDifferential > 0.25f) {
						base.HapticPulse(0.1f);
						lastHapticPosition = base.ActiveElement.localPosition;
					}
				}
			}

			public override void SetDefaultValue() {
				SetVector2Value(new Vector2(defaultValueX, defaultValueZ));
			}

		}

	}
}