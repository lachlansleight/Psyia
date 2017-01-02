using UnityEngine;
using System.Collections;

namespace VRTools {

	public enum ControlState {
		Absent,
		Inactive,
		NearTouched,
		NearTouchDown,
		NearTouchUp,
		Touched,
		TouchDown,
		TouchUp,
		Pressed,
		PressUp,
		PressDown
	}

	public struct ControllerState {
		/// <summary>
		/// Whether the controller is currently being tracked by the lighthouse trackers
		/// </summary>
		public bool IsTracked;
		/// <summary>
		/// The SteamVR_Controller index of the controller
		/// </summary>
		public int Index;

		/// <summary>
		/// The Unity transform
		/// </summary>
		public Transform Transform;

		/// <summary>
		/// Position in world space
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Velocity in world space
		/// </summary>
		public Vector3 Velocity;
		/// <summary>
		/// Forward vector in world space
		/// </summary>
		public Vector3 Forward;
		/// <summary>
		/// Up vector in world space
		/// </summary>
		public Vector3 Up;
		/// <summary>
		/// Right vector in world space
		/// </summary>
		public Vector3 Right;
		/// <summary>
		/// Rotation in world space (shortcut for Quaternion.LookRotation(Forward, Up))
		/// </summary>
		public Quaternion Rotation;
		/// <summary>
		/// Angular velocity in world space
		/// </summary>
		public Vector3 AngularVelocity;

		/// <summary>
		/// Current state of the controller buttons
		/// </summary>
		public ButtonStates Buttons;

		/// <summary>
		/// Position on the touchpad
		/// </summary>
		public Vector2 TouchpadAxis;
		/// <summary>
		/// Orthogonal position on the touchpad (or center if in the center, or untouched if not touched)
		/// </summary>
		public TouchpadDir TouchpadDir;
		/// <summary>
		/// Change in horizontal touchpad position since last frame
		/// </summary>
		public float TouchpadDeltaX;
		/// <summary>
		/// Change in vertical touchpad position since last frame
		/// </summary>
		public float TouchpadDeltaY;
		/// <summary>
		/// Change in distance from the center of touchpad position since last frame
		/// </summary>
		public float TouchpadDeltaR;
		/// <summary>
		/// Change in angle of touchpad position since last frame (clockwise is positive)
		/// </summary>
		public float TouchpadDeltaT;

		public Vector2 TriggerAxis;
	}

	public struct ButtonStates {
		/// <summary>
		/// Current state of the trigger button
		/// </summary>
		public ButtonState Trigger;
		/// <summary>
		/// Current state of the grip button
		/// </summary>
		public ButtonState Grip;
		/// <summary>
		/// Current state of the touchpad button
		/// </summary>
		public ButtonState TouchpadButton;
		/// <summary>
		/// Current state of the menu button
		/// </summary>
		public ButtonState Menu;
	}

	public struct ChaperoneData {
		/// <summary>
		/// Whether the chaperone bounds have been loaded in
		/// </summary>
		public bool IsValid;
		/// <summary>
		/// Width in meters of hard bounds
		/// </summary>
		public float Width;
		/// <summary>
		/// Depth in meters of hard bounds
		/// </summary>
		public float Depth;
	}

	public enum TouchpadDir {
		Left,
		Right,
		Up,
		Down,
		Center,
		Untouched
	}

	public enum ButtonState {
		Inactive,
		Touched,
		Pressed,
		TouchDown,
		TouchUp,
		PressDown,
		PressUp
	}

	public struct Vibration {
		public float frequency;
		public float amplitude;
		public float nextTime;
		public float stopTime;
	}
}