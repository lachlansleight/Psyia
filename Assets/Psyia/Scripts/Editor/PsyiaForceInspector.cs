using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PsyiaForce))]
public class PsyiaForceInspector : Editor {
	

	void OnEnable() {
	}

	public override void OnInspectorGUI() {
		PsyiaForce myTarget = (PsyiaForce)target;

		myTarget.Shape = (PsyiaForce.ForceShape)EditorGUILayout.EnumPopup("Force Shape", myTarget.Shape);
		myTarget.Strength = EditorGUILayout.FloatField("Strength", myTarget.Strength);
		myTarget.AttenuationMode = (PsyiaForce.ForceAttenuationMode)EditorGUILayout.EnumPopup("Attenuation Mode", myTarget.AttenuationMode);
		if(myTarget.AttenuationMode == PsyiaForce.ForceAttenuationMode.Sine) {
			myTarget.AttenuationDistance = EditorGUILayout.FloatField("Attenuation Period", myTarget.AttenuationDistance);
		} else if(myTarget.AttenuationMode == PsyiaForce.ForceAttenuationMode.Infinite) {

		} else {
			myTarget.AttenuationDistance = EditorGUILayout.FloatField("Attenuation Distance", myTarget.AttenuationDistance);
		}
	}
}
