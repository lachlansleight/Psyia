using UnityEngine;
using XInputDotNetPure; // Required in C#
using System.Collections.Generic;

namespace VRTools {

	public class XboxInput : MonoBehaviour
	{
		Dictionary<string, float> Axes;
		Dictionary<string, int> Buttons;


		bool playerIndexSet = false;
		PlayerIndex playerIndex;
		GamePadState state;
		GamePadState prevState;

		// Use this for initialization
		void Start()
		{
			VRInput.AddXbox();
		}


		// Update is called once per frame
		void Update()
		{
			VRInputDevice tempDevice = VRInput.GetDevice("Xbox");

			// Find a PlayerIndex, for a single player game
			// Will find the first controller that is connected ans use it
			if (!playerIndexSet || !prevState.IsConnected)
			{
				for (int i = 0; i < 4; ++i)
				{
					PlayerIndex testPlayerIndex = (PlayerIndex)i;
					GamePadState testState = GamePad.GetState(testPlayerIndex);
					if (testState.IsConnected)
					{
						Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
						playerIndex = testPlayerIndex;
						playerIndexSet = true;
					}
				}
			}

			prevState = state;
			state = GamePad.GetState(playerIndex);

			tempDevice.SetAxis("LeftStickX", state.ThumbSticks.Left.X);
			tempDevice.SetAxis("LeftStickY", state.ThumbSticks.Left.Y);
			tempDevice.SetAxis("RightStickX", state.ThumbSticks.Right.X);
			tempDevice.SetAxis("RightStickY", state.ThumbSticks.Right.Y);
			tempDevice.SetAxis("LeftTrigger", state.Triggers.Left);
			tempDevice.SetAxis("RightTrigger", state.Triggers.Right);
			tempDevice.SetAxis("DPadX", state.DPad.Left.Equals(XInputDotNetPure.ButtonState.Pressed) ? -1f : state.DPad.Right.Equals(XInputDotNetPure.ButtonState.Pressed) ? 1f : 0f);
			tempDevice.SetAxis("DPadY", state.DPad.Down.Equals(XInputDotNetPure.ButtonState.Pressed) ? -1f : state.DPad.Up.Equals(XInputDotNetPure.ButtonState.Pressed) ? 1f : 0f);
			tempDevice.SetAxis("Triggers", state.Triggers.Right - state.Triggers.Left);

			tempDevice.SetButton("A", getButtonInt(prevState.Buttons.A, state.Buttons.A));
			tempDevice.SetButton("B", getButtonInt(prevState.Buttons.B, state.Buttons.B));
			tempDevice.SetButton("X", getButtonInt(prevState.Buttons.X, state.Buttons.X));
			tempDevice.SetButton("Y", getButtonInt(prevState.Buttons.Y, state.Buttons.Y));
			tempDevice.SetButton("DPadUp", getButtonInt(prevState.DPad.Up, state.DPad.Up));
			tempDevice.SetButton("DPadDown", getButtonInt(prevState.DPad.Down, state.DPad.Down));
			tempDevice.SetButton("DPadLeft", getButtonInt(prevState.DPad.Left, state.DPad.Left));
			tempDevice.SetButton("DPadRight", getButtonInt(prevState.DPad.Right, state.DPad.Right));
			tempDevice.SetButton("LeftShoulder", getButtonInt(prevState.Buttons.LeftShoulder, state.Buttons.LeftShoulder));
			tempDevice.SetButton("RightShoulder", getButtonInt(prevState.Buttons.RightShoulder, state.Buttons.RightShoulder));
			tempDevice.SetButton("LeftTrigger", getTriggerButtonInt(prevState.Triggers.Left, state.Triggers.Left));
			tempDevice.SetButton("RightTrigger", getTriggerButtonInt(prevState.Triggers.Right, state.Triggers.Right));
			tempDevice.SetButton("LeftStick", getButtonInt(prevState.Buttons.LeftStick, state.Buttons.LeftStick));
			tempDevice.SetButton("RightStick", getButtonInt(prevState.Buttons.RightStick, state.Buttons.RightStick));
			tempDevice.SetButton("Back", getButtonInt(prevState.Buttons.Back, state.Buttons.Back));
			tempDevice.SetButton("Start", getButtonInt(prevState.Buttons.Start, state.Buttons.Start));

			tempDevice.Vibrate();
		}

		int getButtonInt(XInputDotNetPure.ButtonState last, XInputDotNetPure.ButtonState current) {
			XInputDotNetPure.ButtonState onState = XInputDotNetPure.ButtonState.Pressed;
			XInputDotNetPure.ButtonState offState = XInputDotNetPure.ButtonState.Pressed;

			bool lastDown = last.Equals(onState);
			bool currentDown = current.Equals(onState);

			if(lastDown && currentDown) return 1;
			if(lastDown && !currentDown) return -1;
			if(!lastDown && currentDown) return 2;
			if(!lastDown && !currentDown) return 0;

			return 0;
		}

		int getTriggerButtonInt(float lastValue, float curValue) {
			bool lastDown = lastValue > 0.9f;
			bool currentDown = curValue > 0.9f;

			if(lastDown && currentDown) return 1;
			if(lastDown && !currentDown) return -1;
			if(!lastDown && currentDown) return 2;
			if(!lastDown && !currentDown) return 0;

			return 0;
		}

		/*
		public float GetAxis(string axisName) {
			if(Axes == null) SetupAxes();

			if(Axes.ContainsKey(axisName)) return tempDevice.SetAxis(axisName];

			Debug.LogError("Error - axes '" + axisName + "' not found");
			return 0f;
		}
		public bool GetButton(string buttonName) {
			if(Buttons == null) SetupButtons();

			if(Buttons.ContainsKey(buttonName)) return (tempDevice.SetButton(buttonName] > 0);

			Debug.LogError("Error - button '" + buttonName + "'not found");
			return false;
		}
		public bool GetButtonDown(string buttonName) {
			if(Buttons == null) SetupButtons();

			if(Buttons.ContainsKey(buttonName)) return (tempDevice.SetButton(buttonName] == 2);

			Debug.LogError("Error - button '" + buttonName + "'not found");
			return false;
		}
		public bool GetButtonUp(string buttonName) {
			if(Buttons == null) SetupButtons();

			if(Buttons.ContainsKey(buttonName)) return (tempDevice.SetButton(buttonName] == -1);

			Debug.LogError("Error - button '" + buttonName + "'not found");
			return false;
		}
		*/
	}

}