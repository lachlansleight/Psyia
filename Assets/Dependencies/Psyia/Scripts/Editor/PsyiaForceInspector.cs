/*

Copyright (c) 2018 Lachlan Sleight

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

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
		SerializedProperty StrengthMultiplierProperty;
		SerializedProperty AttenuationProperty;
		SerializedProperty DistanceProperty;

		void OnEnable() {
			ShapeProperty = serializedObject.FindProperty("Shape");
			StrengthProperty = serializedObject.FindProperty("Strength");
			StrengthMultiplierProperty = serializedObject.FindProperty("StrengthMultiplier");
			AttenuationProperty = serializedObject.FindProperty("AttenuationMode");
			DistanceProperty = serializedObject.FindProperty("AttenuationDistance");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			PsyiaForce myTarget = (PsyiaForce)target;

			EditorGUILayout.PropertyField(ShapeProperty, new GUIContent("Force Shape"));
			EditorGUILayout.PropertyField(StrengthProperty, new GUIContent("Force Strength (N)"));
			EditorGUILayout.PropertyField(StrengthMultiplierProperty, new GUIContent("Force Multiplier"));
			EditorGUILayout.PropertyField(AttenuationProperty, new GUIContent("Attenuation Mode"));
			if(AttenuationProperty.enumValueIndex == 4) {
				EditorGUILayout.PropertyField(DistanceProperty, new GUIContent("Attenuation Period"));
			} else if(AttenuationProperty.enumValueIndex > 4) {
				EditorGUILayout.PropertyField(DistanceProperty, new GUIContent("Attenuation Softening Distance"));
			} else if(AttenuationProperty.enumValueIndex != 0) {
				EditorGUILayout.PropertyField(DistanceProperty, new GUIContent("Attenuation Distance"));
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}