using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Psyia {
	[CustomEditor(typeof(PsyiaForce))]
	[CanEditMultipleObjects]
	public class PsyiaForceInspector : Editor {
		
		SerializedProperty ShapeProperty;
		SerializedProperty StrengthProperty;
		SerializedProperty AttenuationProperty;
		SerializedProperty DistanceProperty;

		void OnEnable() {
			ShapeProperty = serializedObject.FindProperty("Shape");
			StrengthProperty = serializedObject.FindProperty("Strength");
			AttenuationProperty = serializedObject.FindProperty("AttenuationMode");
			DistanceProperty = serializedObject.FindProperty("AttenuationDistance");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			PsyiaForce myTarget = (PsyiaForce)target;

			EditorGUILayout.PropertyField(ShapeProperty, new GUIContent("Force Shape"));
			EditorGUILayout.PropertyField(StrengthProperty, new GUIContent("Force Strength (N)"));
			EditorGUILayout.PropertyField(AttenuationProperty, new GUIContent("Attenuation Mode"));
			if(AttenuationProperty.enumValueIndex == 4) {
				EditorGUILayout.PropertyField(DistanceProperty, new GUIContent("Attenuation Period"));
			} else if(AttenuationProperty.enumValueIndex != 0) {
				EditorGUILayout.PropertyField(DistanceProperty, new GUIContent("Attenuation Distance"));
			} else if(AttenuationProperty.enumValueIndex > 4) {
				EditorGUILayout.PropertyField(DistanceProperty, new GUIContent("Attenuation Softening Distance"));
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}