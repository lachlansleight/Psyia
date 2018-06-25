using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ForceSource))]
public class ForceSourceInspector : Editor {
	

	void OnEnable() {
	}

	public override void OnInspectorGUI() {
		ForceSource myTarget = (ForceSource)target;

		myTarget.Force = (PsyiaForce)EditorGUILayout.ObjectField("Force", myTarget.Force, typeof(PsyiaForce), false);
		if(myTarget.Force != null) {
			myTarget.UseTransformPosition = EditorGUILayout.Toggle("Use Transform Position", myTarget.UseTransformPosition);
			myTarget.UseTransformRotation = EditorGUILayout.Toggle("Use Transform Rotation", myTarget.UseTransformRotation);
			myTarget.UseStrengthModifier = EditorGUILayout.Toggle("Use Strength Modifier", myTarget.UseStrengthModifier);
			myTarget.StrengthModifier = EditorGUILayout.FloatField("Strength Modifier", myTarget.StrengthModifier);

			EditorGUILayout.LabelField("Force Asset Settings", EditorStyles.boldLabel);
			myTarget.Force.Shape = (PsyiaForce.ForceShape)EditorGUILayout.EnumPopup("Force Shape", myTarget.Force.Shape);
			myTarget.Force.Strength = EditorGUILayout.FloatField("Strength", myTarget.Force.Strength);
			myTarget.Force.AttenuationMode = (PsyiaForce.ForceAttenuationMode)EditorGUILayout.EnumPopup("Attenuation Mode", myTarget.Force.AttenuationMode);
			if(myTarget.Force.AttenuationMode == PsyiaForce.ForceAttenuationMode.Sine) {
				myTarget.Force.AttenuationDistance = EditorGUILayout.FloatField("Attenuation Period", myTarget.Force.AttenuationDistance);
			} else if(myTarget.Force.AttenuationMode == PsyiaForce.ForceAttenuationMode.Infinite) {

			} else {
				myTarget.Force.AttenuationDistance = EditorGUILayout.FloatField("Attenuation Distance", myTarget.Force.AttenuationDistance);
			}
		}
	}
}
