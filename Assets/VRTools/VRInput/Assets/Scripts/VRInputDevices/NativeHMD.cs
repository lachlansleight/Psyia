using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.VR;

namespace VRTools {

	public class NativeHMD : VRInputDevice {

		Dictionary<string, string> stringValues;
		Dictionary<string, float> floatValues;
		Dictionary<string, Vector3> vector3Values;
		Dictionary<string, Quaternion> quaternionValues;
		
		public NativeHMD() {
			base.capabilities = new Dictionary<string, int>();

			floatValues = new Dictionary<string, float>();
			floatValues.Add("RefreshRate", 0);

			stringValues = new Dictionary<string, string>();
			stringValues.Add("Model", "");

			vector3Values = new Dictionary<string, Vector3>();
			vector3Values.Add("Head", Vector3.zero);
			vector3Values.Add("LeftEye", Vector3.zero);
			vector3Values.Add("RightEye", Vector3.zero);
			vector3Values.Add("CenterEye", Vector3.zero);

			quaternionValues = new Dictionary<string, Quaternion>();
			quaternionValues.Add("Head", Quaternion.identity);
			quaternionValues.Add("LeftEye", Quaternion.identity);
			quaternionValues.Add("RightEye", Quaternion.identity);
			quaternionValues.Add("CenterEye", Quaternion.identity);
		}

		public override float GetFloat (string variableName)
		{
			if(floatValues.ContainsKey(variableName)) return floatValues[variableName];

			Debug.LogError("Error - variableName '" + variableName + "' not found in NativeHMD floatValues");
			return 0f;
		}
		public override void SetFloat (string variableName, float value)
		{
			if(floatValues.ContainsKey(variableName)) {
				floatValues[variableName] = value;
				return;
			}
			Debug.LogError("Error - variableName '" + variableName + "' not found in NativeHMD floatValues");
		}



		public override Vector3 GetVector3 (string variableName)
		{
			if(vector3Values.ContainsKey(variableName)) return vector3Values[variableName];

			Debug.LogError("Error - variableName '" + variableName + "' not found in NativeHMD vector3Values");
			return Vector3.zero;
		}
		public override void SetVector3 (string variableName, Vector3 value)
		{
			if(vector3Values.ContainsKey(variableName)) {
				vector3Values[variableName] = value;
				return;
			}
			Debug.LogError("Error - variableName '" + variableName + "' not found in NativeHMD vector3Values");
		}



		public override Quaternion GetQuaternion (string variableName)
		{
			if(quaternionValues.ContainsKey(variableName)) return quaternionValues[variableName];

			Debug.LogError("Error - variableName '" + variableName + "' not found in NativeHMD quaternionValues");
			return Quaternion.identity;
		}
		public override void SetQuaternion (string variableName, Quaternion value)
		{
			if(quaternionValues.ContainsKey(variableName)) {
				quaternionValues[variableName] = value;
				return;
			}
			Debug.LogError("Error - variableName '" + variableName + "' not found in NativeHMD quaternionValues");
		}



		public override string GetString (string variableName)
		{
			if(stringValues.ContainsKey(variableName)) return stringValues[variableName];

			Debug.LogError("Error - variableName '" + variableName + "' not found in NativeHMD stringValues");
			return "";
		}
		public override void SetString (string variableName, string value)
		{
			if(stringValues.ContainsKey(variableName)) {
				stringValues[variableName] = value;
				return;
			}
			Debug.LogError("Error - variableName '" + variableName + "' not found in NativeHMD stringValues");
		}



		public override void ListFunctionality() {
			string functionalityString = "Functionality for NativeHMD:";
			functionalityString += "\n  " + floatValues.Count + " floatValues:";
			foreach(KeyValuePair<string, float> pair in floatValues) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			functionalityString += "\n  " + vector3Values.Count + " vector3Values:";
			foreach(KeyValuePair<string, Vector3> pair in vector3Values) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			functionalityString += "\n  " + quaternionValues.Count + " quaternionValues:";
			foreach(KeyValuePair<string, Quaternion> pair in quaternionValues) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			functionalityString += "\n  " + stringValues.Count + " stringValues:";
			foreach(KeyValuePair<string, string> pair in stringValues) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			Debug.Log(functionalityString);
		}

		public override void Recenter() {
			InputTracking.Recenter();
		}
	}

}