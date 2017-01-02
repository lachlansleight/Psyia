using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRTools {

	public class XboxController : VRInputDevice {

		Dictionary<string, float> axes;
		Dictionary<string, int> buttons;

		Dictionary<string, List<Vibration>> vibrators;

		float currentLeftVibe = 0f;
		float currentRightVibe = 0f;

		public XboxController() {
			base.capabilities = new Dictionary<string, int>();
				base.capabilities.Add("Axis", 9);
				base.capabilities.Add("Button", 16);
				base.capabilities.Add("HapticVibration", 2);

			axes = new Dictionary<string, float>();
				axes.Add("LeftStickX", 0f);
				axes.Add("LeftStickY", 0f);
				axes.Add("RightStickX", 0f);
				axes.Add("RightStickY", 0f);
				axes.Add("DPadX", 0f);
				axes.Add("DPadY", 0f);
				axes.Add("LeftTrigger", 0f);
				axes.Add("RightTrigger", 0f);
				axes.Add("Triggers", 0f);
			buttons = new Dictionary<string, int>();
				buttons.Add("A", 0);
				buttons.Add("B", 0);
				buttons.Add("X", 0);
				buttons.Add("Y", 0);
				buttons.Add("DPadUp", 0);
				buttons.Add("DPadDown", 0);
				buttons.Add("DPadRight", 0);
				buttons.Add("DPadLeft", 0);
				buttons.Add("LeftStick", 0);
				buttons.Add("RightStick", 0);
				buttons.Add("Start", 0);
				buttons.Add("Back", 0);
				buttons.Add("LeftTrigger", 0);
				buttons.Add("RightTrigger", 0);
				buttons.Add("LeftShoulder", 0);
				buttons.Add("RightShoulder", 0);

			vibrators = new Dictionary<string, List<Vibration>>();
				vibrators.Add("Left", new List<Vibration>());
				vibrators.Add("Right", new List<Vibration>());
		}

		public override void ListFunctionality() {
			string functionalityString = "Functionality for XboxController:";
			functionalityString += "\n  " + axes.Count + " axes:";
			foreach(KeyValuePair<string, float> pair in axes) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			functionalityString += "\n  " + buttons.Count + " buttons:";
			foreach(KeyValuePair<string, int> pair in buttons) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			functionalityString += "\n  " + vibrators.Count + " vibrators:";
			foreach(KeyValuePair<string, List<Vibration>> pair in vibrators) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			Debug.Log(functionalityString);
		}

		public override bool GetButton(string ButtonName) {
			if(buttons.ContainsKey(ButtonName)) {
				if(buttons[ButtonName] <= 0) return false;
				else return true;
			}

			Debug.LogError("Error: button '" + ButtonName + "' not found in XboxController");
			return false;
		}
		public override bool GetButtonDown(string ButtonName) {
			if(buttons.ContainsKey(ButtonName)) {
				if(buttons[ButtonName] == 2) return true;
				else return false;
			}

			Debug.LogError("Error: button '" + ButtonName + "' not found in XboxController");
			return false;
		}
		public override bool GetButtonUp(string ButtonName) {
			if(buttons.ContainsKey(ButtonName)) {
				if(buttons[ButtonName] == -1) return true;
				else return false;
			}

			Debug.LogError("Error: button '" + ButtonName + "' not found in XboxController");
			return false;
		}

		public override float GetAxis(string AxisName) {
			if(axes.ContainsKey(AxisName)) return axes[AxisName];

			Debug.LogError("Error: axis '" + AxisName + "' not found in XboxController");
			return 0f;
		}

		public override void SetButton (string buttonName, int value)
		{
			if(buttons.ContainsKey(buttonName)) {
				buttons[buttonName] = value;
				return;
			}

			Debug.LogError("Error - button '" + buttonName + "' not found in XboxController buttons");
		}

		public override void SetAxis (string axisName, float value)
		{
			if(axes.ContainsKey(axisName)) {
				axes[axisName] = value;
				return;
			}

			Debug.LogError("Error - axis '" + axisName + "' not found in XboxController axes");
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

			length = Mathf.Max(0.1f, length);

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


		public override void Vibrate() {
			//XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, 0f, 0f);
			//return;
			float amplitudeSumLeft = 0f;
			for(int i = 0; i < vibrators["Left"].Count; i++) {
				Vibration curVibe = vibrators["Left"][i];
				if(curVibe.nextTime <= Time.time) {
					amplitudeSumLeft += curVibe.amplitude;
					curVibe.nextTime += 1f / curVibe.frequency;
					vibrators["Left"][i] = curVibe;
				} else if(curVibe.nextTime <= (Time.time + 0.1f)) {
					amplitudeSumLeft += curVibe.amplitude;
				}

				if(curVibe.stopTime < Time.time) {
					amplitudeSumLeft += curVibe.amplitude;
					vibrators["Left"].RemoveAt(i);
				}
			}

			float amplitudeSumRight = 0f;
			for(int i = 0; i < vibrators["Right"].Count; i++) {
				Vibration curVibe = vibrators["Right"][i];
				if(curVibe.nextTime <= Time.time) {
					amplitudeSumRight += curVibe.amplitude;
					curVibe.nextTime += 1f / curVibe.frequency;
					vibrators["Right"][i] = curVibe;
				} else if(curVibe.nextTime <= (Time.time + 0.1f)) {
					amplitudeSumRight += curVibe.amplitude;
				}

				if(curVibe.stopTime < Time.time) {
					amplitudeSumRight += curVibe.amplitude;
					vibrators["Right"].RemoveAt(i);
				}
			}

			amplitudeSumLeft = Mathf.Clamp01(amplitudeSumLeft);
			amplitudeSumRight = Mathf.Clamp01(amplitudeSumRight);

			XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, amplitudeSumLeft, amplitudeSumRight);

			if(amplitudeSumLeft > 0 || amplitudeSumRight > 0) {
				Debug.Log("Left: " + amplitudeSumLeft + "\tRight: " + amplitudeSumRight);
			}
		}

	}

}