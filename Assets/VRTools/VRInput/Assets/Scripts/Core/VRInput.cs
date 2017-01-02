using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRTools {

	public class VRInput {

		static Dictionary<string, VRInputDevice> Devices;

		/// <summary>
		/// Returns a VRInputDevice with a given name
		/// </summary>
		/// <returns>Generic VRInputDevice interface</returns>
		/// <param name="deviceName">The name of the VR Device</param>
		public static VRInputDevice GetDevice(string deviceName) {
			if(Devices == null) {
				Debug.LogError("Error - couldn't find VR Device '" + deviceName + "' (Devices is null)");
				return null;
			}

			if(Devices.ContainsKey(deviceName)) {
				return Devices[deviceName];
			} else {
				Debug.LogError("Error - couldn't find VR Device '" + deviceName + "'");
				return null;
			}
		}

		/// <summary>
		/// Determines if the specified device exists
		/// </summary>
		/// <returns><c>true</c> if device exists, otherwise <c>false</c>.</returns>
		/// <param name="deviceName">Device name</param>
		public static bool HasDevice(string deviceName) {
			if(Devices == null) return false;

			return Devices.ContainsKey(deviceName);
		}

		/// <summary>
		/// Lists all currently available VR Input Devices
		/// </summary>
		public static void ListVRDevices() {
			if(Devices == null) {
				Debug.Log("No VR devices found (Devices is null)");
			} else if(Devices.Count == 0) {
				Debug.Log("No VR devices found (Device count is 0)");
			} else {
				string tempString = "VR Devices:";
				foreach(KeyValuePair<string, VRInputDevice> pair in Devices) {
					tempString += "\n[" + pair.Key + "]";
				}
				Debug.Log(tempString);
			}
		}

		/// <summary>
		/// Initialises the HTC Vive HMD with Left and Right controllers
		/// </summary>
		public static void AddVive() {
			if(Devices == null) Devices = new Dictionary<string, VRInputDevice>();

			#if UNITY_5_3 || UNITY_5_2 || UNITY_5_1 || UNITY_5_0 || UNITY_4
			ViveHMD viveHmd = new ViveHMD();
			#else
			NativeHMD viveHmd = new NativeHMD();
			#endif
			ViveController viveLeft = new ViveController();
			ViveController viveRight = new ViveController();
			if(Devices.ContainsKey("ViveHMD")) Devices.Remove("ViveHMD");
			Devices.Add("ViveHMD", viveHmd);
			//Devices["ViveHMD"].ListFunctionality();
			if(Devices.ContainsKey("ViveLeft")) Devices.Remove("ViveLeft");
			Devices.Add("ViveLeft", viveLeft);
			//Devices["ViveLeft"].ListFunctionality();
			if(Devices.ContainsKey("ViveRight")) Devices.Remove("ViveRight");
			Devices.Add("ViveRight", viveRight);
		}

		/// <summary>
		/// Initialises an Xbox controller for traditional input
		/// </summary>
		public static void AddXbox() {
			if(Devices == null) Devices = new Dictionary<string, VRInputDevice>();

			XboxController xBox = new XboxController();
			if(Devices.ContainsKey("Xbox")) Devices.Remove("Xbox");
			Devices.Add("Xbox", xBox);
		}
	}

}