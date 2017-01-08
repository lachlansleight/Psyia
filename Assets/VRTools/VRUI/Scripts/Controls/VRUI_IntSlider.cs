using UnityEngine;
using System.Collections;


namespace VRTools
{
	namespace UI {

		public class VRUI_IntSlider : VRUI_Control {
			private float zPosTarget = 0;
			private float zPosLerpRate = 0.5f;
			private int targetValue;
			private int currentValue;
			
			[Space(20)]
			public int minValue = 0;
			public int maxValue = 4;
			public int defaultValue = 0;

			private float[] valuePositions;

			public override void Awake() {
				base.Awake();

				if(valuePositions == null) {
					SetupvaluePositions();
				} else if(valuePositions.Length == 0) {
					SetupvaluePositions();
				}

				base.ActiveElement.localPosition = new Vector3(0f, 0f, valuePositions[defaultValue]);
				targetValue = defaultValue;
				currentValue = defaultValue;

				base.ChangeIntValue(GetValueFromIndex(currentValue));
			}

			private void SetupvaluePositions() {
				valuePositions = new float[maxValue - minValue + 1];
				for(int i = 0; i < valuePositions.Length; i++) {
					valuePositions[i] = Mathf.Lerp(-1f, 1f, ((float)i / (float)(valuePositions.Length - 1)));
					//VRDebug.DrawLine(base.ActiveElement.position, base.ActiveElement.position + (Quaternion.Euler(0f, valuePositions[i], 0f) * Vector3.forward * 0.05f), Color.green, 10f);
					//random idea to self: VRDebug.DrawLine should really be using a geometry shader rendering lines - could probably just make each line a vertex in a RWBuffer
					//i.e. position + tangent as the two locations, color for color - do we need to depth test? (if so, need a second mesh using a different material that renders as overlay - or maybe a second pass?)
				}
			}

			public override void SetIntValue(int newValue) {
				base.SetIntValue(newValue);

				if(valuePositions == null) {
					SetupvaluePositions();
				} else if(valuePositions.Length == 0) {
					SetupvaluePositions();
				}

				base.ActiveElement.localPosition = new Vector3(0f, 0f, valuePositions[newValue]);
				targetValue = newValue;
				currentValue = newValue;

				base.ChangeIntValue(GetValueFromIndex(currentValue));
			}

			int GetValueFromIndex(int index) {
				return Mathf.RoundToInt(Mathf.Lerp(minValue, maxValue, Mathf.InverseLerp(0f, valuePositions.Length -1, index)));
			}

			public override void Active(Vector3 inputPos) {
				base.Active(inputPos);

				if(base.currentState.Equals(VRUI_State.Down)) {
					Vector3 projectedPosition = base.ActiveElement.parent.InverseTransformPoint(inputPos);
					float position = Mathf.Clamp(projectedPosition.z, -1f, 1f);

					//this gives us an aimed-for rotation from 0 to 360
					targetValue = GetClosestValueFromPosition(position, currentValue);
					zPosTarget = position;
				}
			}

			int GetClosestValueFromPosition(float position, int exclude) {
				float smallestDistance = Mathf.Infinity;
				int chosenValue = 0;
				for(int i = 0; i < valuePositions.Length; i++) {
					float currentDistance = Mathf.Abs(position - valuePositions[i]);
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
					float curPos = valuePositions[currentValue];
					float tarPos = valuePositions[targetValue];

					float finalPos = Mathf.Lerp(curPos, tarPos, lerp(Mathf.InverseLerp(curPos, tarPos, zPosTarget)));
					Vector3 targetPos = new Vector3(0f, 0f, finalPos);

					base.ActiveElement.localPosition = Vector3.Lerp(base.ActiveElement.localPosition, targetPos, zPosLerpRate);
					if(Mathf.InverseLerp(curPos, tarPos, zPosTarget) > 0.5f) {
						currentValue = targetValue;
						if(currentValue != base.intValue) {
							base.ChangeIntValue(GetValueFromIndex(currentValue));
							base.HapticPulse(0.5f);
						}
					}
				} else {
					float finalPos = valuePositions[currentValue];
					base.ActiveElement.localPosition = new Vector3(0f, 0f, Mathf.Lerp(base.ActiveElement.localPosition.z, finalPos, zPosLerpRate));
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