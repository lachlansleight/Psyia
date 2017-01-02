using UnityEngine;
using System.Collections;


namespace VRTools
{
	namespace UI {

		public class VRUI_Rotary : VRUI_Control {
			private float targetRot = 0;
			private float rotLerpRate = 0.5f;
			private int targetValue;
			private int currentValue;
			
			[Space(20)]
			public int minValue = 0;
			public int maxValue = 4;
			public int defaultValue = 0;

			private float[] valueRotations;

			public override void Awake() {
				base.Awake();

				if(valueRotations == null) {
					SetupValueRotations();
				} else if(valueRotations.Length == 0) {
					SetupValueRotations();
				}

				base.ActiveElement.parent.localRotation = Quaternion.Euler(-90f, 180f, 180f / (float)valueRotations.Length);
				base.ActiveElement.localRotation = Quaternion.Euler(0f, 0f, valueRotations[defaultValue]);
				targetValue = defaultValue;
				currentValue = defaultValue;

				base.ChangeIntValue(currentValue);
			}

			private void SetupValueRotations() {
				valueRotations = new float[maxValue - minValue + 1];
				for(int i = 0; i < valueRotations.Length; i++) {
					valueRotations[i] = ((float)i / (float)valueRotations.Length) * 360f;
					//VRDebug.DrawLine(base.ActiveElement.position, base.ActiveElement.position + (Quaternion.Euler(0f, valueRotations[i], 0f) * Vector3.forward * 0.05f), Color.green, 10f);
					//random idea to self: VRDebug.DrawLine should really be using a geometry shader rendering lines - could probably just make each line a vertex in a RWBuffer
					//i.e. position + tangent as the two locations, color for color - do we need to depth test? (if so, need a second mesh using a different material that renders as overlay - or maybe a second pass?)
				}
			}

			public override void SetIntValue(int newValue) {
				base.SetIntValue(newValue);

				if(valueRotations == null) {
					SetupValueRotations();
				} else if(valueRotations.Length == 0) {
					SetupValueRotations();
				}

				base.ActiveElement.localRotation = Quaternion.Euler(0f, 0f, valueRotations[newValue]);
				targetValue = newValue;
				currentValue = newValue;

				base.ChangeIntValue(currentValue);
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

					//this gives us an aimed-for rotation from 0 to 360
					targetValue = GetClosestValueFromRotation(rotation, currentValue);
					targetRot = rotation;
				}
			}

			int GetClosestValueFromRotation(float rotation, int exclude) {
				float smallestDistance = Mathf.Infinity;
				int chosenValue = 0;
				for(int i = 0; i < valueRotations.Length; i++) {
					float currentDistance = Mathf.Min(Mathf.Abs(rotation - valueRotations[i]), Mathf.Abs(Mathf.Abs(rotation - valueRotations[i]) - 360f));
					if(currentDistance < smallestDistance && i != exclude) {
						chosenValue = i;
						smallestDistance = currentDistance;
					}
				}

				return chosenValue;
			}
			
			public override void Update() {
				base.Update();

				if(base.currentState.Equals(VRUI_State.Down)) {
					float curRot = valueRotations[currentValue];
					float tarRot = valueRotations[targetValue];

					if(currentValue == maxValue && targetValue == minValue) {
						tarRot = valueRotations[targetValue] + 360f;
					}
					if(currentValue == minValue && targetValue == maxValue) {
						curRot = valueRotations[currentValue] + 360f;
					}

					float finalRot = Mathf.Lerp(curRot, tarRot, lerp(Mathf.InverseLerp(curRot, tarRot, targetRot)));
					Quaternion targetQuat = Quaternion.Euler(0f, 0f, finalRot);

					base.ActiveElement.localRotation = Quaternion.Lerp(base.ActiveElement.localRotation, targetQuat, rotLerpRate);
					if(Mathf.InverseLerp(curRot, tarRot, targetRot) > 0.5f) {
						currentValue = targetValue;
						if(currentValue != base.intValue) {
							base.ChangeIntValue(currentValue);
							base.HapticPulse(0.5f);
						}
					}
				} else {
					float finalRot = valueRotations[currentValue];
					base.ActiveElement.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(base.ActiveElement.localEulerAngles.z, finalRot, rotLerpRate));
				}
			}

			public override void SetDefaultValue() {
				SetIntValue(defaultValue);
			}

			float lerp(float t) {
				if(t < 0.5f) return 4f * t * t * t * t;
				t--;
				return 4f * t * t * t + 1f;
			}
			
		}

	}
}