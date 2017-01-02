using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRTools {

	public class ViveController : VRInputDevice {

		Dictionary<string, float> axes;
		Dictionary<string, int> buttons;
		Dictionary<string, int> touchButtons;
		Dictionary<string, int> intValues;

		Dictionary<string, List<Vibration>> vibrators;
		Dictionary<string, List<Vibration>> haptics;


		public ViveController() {
			base.capabilities = new Dictionary<string, int>();
				base.capabilities.Add("Axis", 5);
				base.capabilities.Add("Button", 4);
				base.capabilities.Add("TouchButton", 1);
				base.capabilities.Add("HapticVibration", 1);

			axes = new Dictionary<string, float>();
				axes.Add("TouchpadX", 0f);
				axes.Add("TouchpadY", 0f);
				axes.Add("TouchpadR", 0f);
				axes.Add("TouchpadT", 0f);
				axes.Add("Trigger", 0f);
			buttons = new Dictionary<string, int>();
				buttons.Add("Menu", 0);
				buttons.Add("Touchpad", 0);
				buttons.Add("Grip", 0);
				buttons.Add("Trigger", 0);
			touchButtons = new Dictionary<string, int>();
				touchButtons.Add("Touchpad", 0);
			intValues = new Dictionary<string, int>();
				intValues.Add("Index", 0);

			vibrators = new Dictionary<string, List<Vibration>>();
				vibrators.Add("Main", new List<Vibration>());

			haptics = new Dictionary<string, List<Vibration>>();
				haptics.Add("Main", new List<Vibration>());
		}

		public override void ListFunctionality() {
			string functionalityString = "Functionality for ViveController:";
			functionalityString += "\n  " + intValues.Count + " intValues:";
			foreach(KeyValuePair<string, int> pair in intValues) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			functionalityString += "\n  " + axes.Count + " axes:";
			foreach(KeyValuePair<string, float> pair in axes) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			functionalityString += "\n  " + buttons.Count + " buttons:";
			foreach(KeyValuePair<string, int> pair in buttons) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			functionalityString += "\n  " + touchButtons.Count + " touchButtons:";
			foreach(KeyValuePair<string, int> pair in touchButtons) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			functionalityString += "\n  " + vibrators.Count + " touchButtons:";
			foreach(KeyValuePair<string, int> pair in touchButtons) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			functionalityString += "\n  " + vibrators.Count + " vibrators:";
			foreach(KeyValuePair<string, List<Vibration>> pair in vibrators) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			functionalityString += "\n  " + haptics.Count + " haptics:";
			foreach(KeyValuePair<string, List<Vibration>> pair in haptics) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			Debug.Log(functionalityString);
		}

		public override bool GetButton(string ButtonName) {
			if(buttons.ContainsKey(ButtonName)) {
				if(buttons[ButtonName] <= 0) return false;
				else return true;
			}

			Debug.LogError("Error: button '" + ButtonName + "' not found in ViveController");
			return false;
		}
		public override bool GetButtonDown(string ButtonName) {
			if(buttons.ContainsKey(ButtonName)) {
				if(buttons[ButtonName] == 2) return true;
				else return false;
			}

			Debug.LogError("Error: button '" + ButtonName + "' not found in ViveController");
			return false;
		}
		public override bool GetButtonUp(string ButtonName) {
			if(buttons.ContainsKey(ButtonName)) {
				if(buttons[ButtonName] == -1) return true;
				else return false;
			}

			Debug.LogError("Error: button '" + ButtonName + "' not found in ViveController");
			return false;
		}


		public override bool GetTouch(string ButtonName) {
			if(touchButtons.ContainsKey(ButtonName)) {
				if(touchButtons[ButtonName] <= 0) return false;
				else return true;
			}

			Debug.LogError("Error: touchButton '" + ButtonName + "' not found in ViveController");
			return false;
		}
		public override bool GetTouchDown(string ButtonName) {
			if(touchButtons.ContainsKey(ButtonName)) {
				if(touchButtons[ButtonName] == 2) return true;
				else return false;
			}

			Debug.LogError("Error: touchButton '" + ButtonName + "' not found in ViveController");
			return false;
		}
		public override bool GetTouchUp(string ButtonName) {
			if(touchButtons.ContainsKey(ButtonName)) {
				if(touchButtons[ButtonName] == -1) return true;
				else return false;
			}

			Debug.LogError("Error: touchButton '" + ButtonName + "' not found in ViveController");
			return false;
		}

		public override float GetAxis(string AxisName) {
			if(axes.ContainsKey(AxisName)) return axes[AxisName];

			Debug.LogError("Error: axis '" + AxisName + "' not found in ViveController");
			return 0f;
		}

		public override int GetInt (string variableName)
		{
			if(intValues.ContainsKey(variableName)) return intValues[variableName];

			Debug.LogError("Error - variableName '" + variableName + "' not found in ViveController intValues");
			return 0;
		}

		public override void SetInt (string variableName, int value)
		{
			if(intValues.ContainsKey(variableName)) {
				intValues[variableName] = value;
				return;
			}

			Debug.LogError("Error - variableName '" + variableName + "' not found in ViveController intValues");
		}

		public override void SetButton (string buttonName, int value)
		{
			if(buttons.ContainsKey(buttonName)) {
				buttons[buttonName] = value;
				return;
			}

			Debug.LogError("Error - button '" + buttonName + "' not found in ViveController buttons");
		}

		public override void SetTouchButton (string buttonName, int value)
		{
			if(touchButtons.ContainsKey(buttonName)) {
				touchButtons[buttonName] = value;
				return;
			}

			Debug.LogError("Error - touchButton '" + buttonName + "' not found in ViveController touch buttons");
		}

		public override void SetAxis (string axisName, float value)
		{
			if(axes.ContainsKey(axisName)) {
				axes[axisName] = value;
				return;
			}

			Debug.LogError("Error - axis '" + axisName + "' not found in ViveController axes");
		}

		public override void SetVibration(string vibrationName, float frequency, float amplitude) {
			if(!vibrators.ContainsKey(vibrationName)) {
				Debug.LogError("Error: vibration '" + vibrationName + "' not found in XboxController");
				return;
			}
			if(frequency <= 0 || amplitude <= 0) {
				Debug.LogWarning("Warning - vibration frequency and amplitude must be above zero. Use StopVibration() to stop vibrations");
				return;
			}

			Vibration newVibe = new Vibration();
			newVibe.frequency = frequency;
			newVibe.amplitude = amplitude;
			newVibe.stopTime = Mathf.Infinity;
			newVibe.nextTime = Time.time + (1f / frequency);

			vibrators[vibrationName].Add(newVibe);

			Vibrate();
		}

		public override void SetVibration(string vibrationName, float frequency, float amplitude, float length) {
			if(!vibrators.ContainsKey(vibrationName)) {
				Debug.LogError("Error: vibration '" + vibrationName + "' not found in XboxController");
				return;
			}
			if(frequency <= 0 || amplitude <= 0) {
				Debug.LogWarning("Warning - vibration frequency and amplitude must be above zero. Use StopVibration() to stop vibrations");
				return;
			}

			Vibration newVibe = new Vibration();
			newVibe.frequency = frequency;
			newVibe.amplitude = amplitude;
			newVibe.stopTime = Time.time + length;
			newVibe.nextTime = Time.time + (1f / frequency);

			vibrators[vibrationName].Add(newVibe);

			Vibrate();
		}

		public override void StopVibration(string vibrationName) {
			if(!vibrators.ContainsKey(vibrationName)) {
				Debug.LogError("Error: vibration '" + vibrationName + "' not found in XboxController");
				return;
			}
			vibrators[vibrationName].Clear();
		}

		public override void PulseHaptic(string hapticName, float amplitude) {
			if(!haptics.ContainsKey(hapticName)) {
				Debug.LogError("Error: haptic '" + hapticName + "' not found in XboxController");
				return;
			}
			if(amplitude <= 0f) {
				Debug.LogWarning("Warning - haptic pulse amplitude must be above zero.");
				return;
			}

			SteamVR_Controller.Input(intValues["Index"]).TriggerHapticPulse((ushort)Mathf.Lerp(1f, 3999f, Mathf.Min(1f, amplitude)));
		}

		public override void Vibrate() {
			float amplitudeSum = 0f;
			foreach(KeyValuePair<string, List<Vibration>> pair in vibrators) {
				for(int i = 0; i < pair.Value.Count; i++) {
					Vibration curVibe = pair.Value[i];
					if(curVibe.nextTime <= Time.time) {
						amplitudeSum += curVibe.amplitude;
						curVibe.nextTime += 1f / curVibe.frequency;
					}
					pair.Value[i] = curVibe;

					if(curVibe.stopTime <= Time.time) {
						pair.Value.RemoveAt(i);
					}
				}

				if(amplitudeSum > 0f) {
					SteamVR_Controller.Input(intValues["Index"]).TriggerHapticPulse((ushort)Mathf.Lerp(1f, 3999f, Mathf.Min(1f, amplitudeSum)));
				}
			}
		}
	}

}