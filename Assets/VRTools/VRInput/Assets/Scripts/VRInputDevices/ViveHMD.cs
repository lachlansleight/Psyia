using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRTools {

	public class ViveHMD : VRInputDevice {

		Dictionary<string, int> intValues;
		
		public ViveHMD() {
			base.capabilities = new Dictionary<string, int>();

			intValues = new Dictionary<string, int>();
			intValues.Add("Index", 0);
		}

		public override int GetInt (string variableName)
		{
			if(intValues.ContainsKey(variableName)) return intValues[variableName];

			Debug.LogError("Error - variableName '" + variableName + "' not found in ViveHMD intValues");
			return 0;
		}

		public override void SetInt (string variableName, int value)
		{
			if(intValues.ContainsKey(variableName)) {
				intValues[variableName] = value;
				return;
			}
			Debug.LogError("Error - variableName '" + variableName + "' not found in ViveHMD intValues");
		}

		public override void ListFunctionality() {
			string functionalityString = "Functionality for ViveHMD:";
			functionalityString += "\n  " + intValues.Count + " intValues:";
			foreach(KeyValuePair<string, int> pair in intValues) {
				functionalityString += "\n    \"" + pair.Key + "\"";
			}
			Debug.Log(functionalityString);
		}
	}

}