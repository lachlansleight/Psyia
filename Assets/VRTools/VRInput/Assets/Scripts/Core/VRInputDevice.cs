using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRTools {

	public class VRInputDevice {

		/// <summary>
		/// The transform of this VR Input Device's gameObject
		/// </summary>
		public Transform transform;
		/// <summary>
		/// The position in world space
		/// </summary>
		public Vector3 position;
		/// <summary>
		/// The rotation in world space
		/// </summary>
		public Quaternion rotation;
		/// <summary>
		/// The velocity in m/s
		/// </summary>
		public Vector3 velocity;
		/// <summary>
		/// The angular velocity euler angles in degrees per second
		/// </summary>
		public Vector3 angularVelocity;
		/// <summary>
		/// The forward vector
		/// </summary>
		public Vector3 forward;
		/// <summary>
		/// The up vector
		/// </summary>
		public Vector3 up;
		/// <summary>
		/// The right vector
		/// </summary>
		public Vector3 right;
		/// <summary>
		/// Returns true if this VR Device has been initialised correctly
		/// </summary>
		public bool isValid;
		/// <summary>
		/// Returns true if this VR Device is currently being tracked
		/// </summary>
		public bool isTracked;

		public Dictionary<string, int> capabilities;

		/// <summary>
		/// Is button currently pressed
		/// </summary>
		/// <returns><c>true</c>, if button is pressed, <c>false</c> otherwise.</returns>
		/// <param name="controlName">Button name</param>
		public virtual bool GetButton(string controlName) { return false; }
		/// <summary>
		/// Get pressed state of a button this frame
		/// </summary>
		/// <returns><c>true</c>, if button was pressed this frame, <c>false</c> otherwise.</returns>
		/// <param name="controlName">Button name</param>
		public virtual bool GetButtonDown(string controlName) { return false; }
		/// <summary>
		/// Get released state of a button this frame
		/// </summary>
		/// <returns><c>true</c>, if button was released this frame, <c>false</c> otherwise.</returns>
		/// <param name="controlName">Button name</param>
		public virtual bool GetButtonUp(string controlName) { return false; }

		/// <summary>
		/// Is button currently touched
		/// </summary>
		/// <returns><c>true</c>, if touch button is touched, <c>false</c> otherwise.</returns>
		/// <param name="controlName">Touch button name</param>
		public virtual bool GetTouch(string controlName) { return false; }
		/// <summary>
		/// Get pressed state of a touch button this frame
		/// </summary>
		/// <returns><c>true</c>, if touch button was touched this frame, <c>false</c> otherwise.</returns>
		/// <param name="controlName">Touch button name</param>
		public virtual bool GetTouchDown(string controlName) { return false; }
		/// <summary>
		/// Get released state of a touch button this frame
		/// </summary>
		/// <returns><c>true</c>, if touch button was released this frame, <c>false</c> otherwise.</returns>
		/// <param name="controlName">Touch button name</param>
		public virtual bool GetTouchUp(string controlName) { return false; }

		/// <summary>
		/// Is neartouch currently pressed
		/// </summary>
		/// <returns><c>true</c>, if neartouch button is near, <c>false</c> otherwise.</returns>
		/// <param name="controlName">Neartouch button name</param>
		public virtual bool GetNearTouch(string controlName) { return false; }
		/// <summary>
		/// Get pressed state of a neartouch button this frame
		/// </summary>
		/// <returns><c>true</c>, if neartouch button was entered this frame, <c>false</c> otherwise.</returns>
		/// <param name="controlName">Neartouch button name</param>
		public virtual bool GetNearTouchDown(string controlName) { return false; }
		/// <summary>
		/// Get released state of a neartouch button this frame
		/// </summary>
		/// <returns><c>true</c>, if neartouch button was removed this frame, <c>false</c> otherwise.</returns>
		/// <param name="controlName">Neartouch button name</param>
		public virtual bool GetNearTouchUp(string controlName) { return false; }

		/// <summary>
		/// Gets a general-purpose float with a given  name
		/// </summary>
		/// <returns>The float value</returns>
		/// <param name="variableName">Variable name.</param>
		public virtual float GetFloat(string variableName) { return 0f; }
		/// <summary>
		/// Gets a general-purpose int with a given  name
		/// </summary>
		/// <returns>The int value</returns>
		/// <param name="variableName">Variable name.</param>
		public virtual int GetInt(string variableName) { return 0; }
		/// <summary>
		/// Gets a general-purpose boolean with a given  name
		/// </summary>
		/// <returns>The boolean value</returns>
		/// <param name="variableName">Variable name.</param>
		public virtual bool GetBool(string variableName) { return false; }
		/// <summary>
		/// Gets a general-purpose Vector3 with a given  name
		/// </summary>
		/// <returns>The vector3 value</returns>
		/// <param name="variableName">Variable name.</param>
		public virtual Vector3 GetVector3(string variableName) { return Vector3.zero; }
		/// <summary>
		/// Gets a general-purpose Quaternion with a given  name
		/// </summary>
		/// <returns>The quaternion value</returns>
		/// <param name="variableName">Variable name.</param>
		public virtual Quaternion GetQuaternion(string variableName) { return Quaternion.identity; }
		/// <summary>
		/// Gets a general-purpose string with a given  name
		/// </summary>
		/// <returns>The string value</returns>
		/// <param name="variableName">Variable name.</param>
		public virtual string GetString(string variableName) { return ""; }

		/// <summary>
		/// Gets a controller axis with given name
		/// </summary>
		/// <returns>The axis value (range depends on the axis, typically -1 to 1 or 0 to 1)</returns>
		/// <param name="controlName">The axis name</param>
		public virtual float GetAxis(string controlName) { return 0f; }



		public virtual void SetButton(string controlName, int value) { }
		public virtual void SetTouchButton(string controlName, int value) { }
		public virtual void SetNearTouchButton(string controlName, int value) { }
		public virtual void SetFloat(string variableName, float value) { }
		public virtual void SetInt(string variableName, int value) { }
		public virtual void SetBool(string variableName, bool value) { }
		public virtual void SetVector3(string variableName, Vector3 value) { }
		public virtual void SetQuaternion(string variableName, Quaternion value) { }
		public virtual void SetString(string variableName, string value) { }
		public virtual void SetAxis(string variableName, float value) { }
		public virtual void Recenter() { }

		/// <summary>
		/// Sets vibration to be run indefinitely
		/// </summary>
		/// <param name="frequency">Frequency of vibration in Hz (from 0Hz (non inclusive) to 90Hz (inclusive)</param>
		/// <param name="amplitude">Amplitude of vibration from 0 to 1</param>
		public virtual void SetVibration(string vibrationName, float frequency, float amplitude) { }
		/// <summary>
		/// Starts a fixed-length vibration
		/// </summary>
		/// <param name="frequency">Frequency of vibration in Hz (from 0Hz (non inclusive) to 90Hz (inclusive)</param>
		/// <param name="amplitude">Amplitude of vibration from 0 to 1</param>
		/// <param name="length">Length of vibration in seconds</param> 
		public virtual void SetVibration(string vibrationName, float frequency, float amplitude, float length) { }
		/// <summary>
		/// Sends a single haptic pulse
		/// </summary>
		/// <param name="amplitude">Amplitude of pulse from 0 to 1</param>
		public virtual void PulseHaptic(string hapticName, float amplitude) { }
		/// <summary>
		/// Runs all current vibrations. Do not call this manually!
		/// </summary>
		public virtual void Vibrate() { }
		/// <summary>
		/// Stops all ongoing vibrations
		/// </summary>
		public virtual void StopVibration(string vibrationName) { }

		/// <summary>
		/// Lists all available functions
		/// </summary>
		public virtual void ListFunctionality() { }
	}

}