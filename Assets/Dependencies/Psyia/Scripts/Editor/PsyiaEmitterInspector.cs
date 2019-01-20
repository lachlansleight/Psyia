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
using UCTK;

namespace Psyia {

	[CustomEditor(typeof(PsyiaEmitter))]
	[CanEditMultipleObjects]
	public class PsyiaEmitterInspector : Editor {

		SerializedProperty ModeProperty;
		SerializedProperty StartEmitCountProperty;
		SerializedProperty EmitOverTimeProperty;
		SerializedProperty EmitOverDistanceProperty;
		SerializedProperty EmissionMultiplierProperty;

		SerializedProperty SettingsProperty;

		void OnEnable() {
			ModeProperty = serializedObject.FindProperty("_Mode");
			StartEmitCountProperty = serializedObject.FindProperty("_StartEmitCount");
			EmitOverTimeProperty = serializedObject.FindProperty("_EmitOverTime");
			EmitOverDistanceProperty = serializedObject.FindProperty("_EmitOverDistance");
			EmissionMultiplierProperty = serializedObject.FindProperty("_EmissionMultiplier");

			SettingsProperty = serializedObject.FindProperty("Settings");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			PsyiaEmitter myTarget = (PsyiaEmitter)target;

			EditorGUILayout.PropertyField(ModeProperty, new GUIContent("Emission Mode"));

			if(ModeProperty.enumValueIndex == 1) {
				EditorGUILayout.PropertyField(StartEmitCountProperty, new GUIContent("Start Emit Count"));
			} else if(ModeProperty.enumValueIndex == 0) {
				EditorGUILayout.PropertyField(EmitOverTimeProperty, new GUIContent("Particles per second"));
			} else if(ModeProperty.enumValueIndex == 2) {
				EditorGUILayout.PropertyField(EmitOverDistanceProperty, new GUIContent("Particles per meter"));
			} else if(ModeProperty.enumValueIndex == 3) {
				EditorGUILayout.LabelField("Call Emit() on this object to emit particles");
			}

			EditorGUILayout.PropertyField(EmissionMultiplierProperty, new GUIContent("Emission Count Multiplier"));

			EditorGUILayout.PropertyField(SettingsProperty, new GUIContent("Emitted Particle Settings"));

			/*
			
			if(myTarget.Mode == PsyiaEmitter.PsyiaEmitterMode.Start) {
				myTarget.StartEmitCount = EditorGUILayout.IntField("Start Emit Count", myTarget.StartEmitCount);
			} else if(myTarget.Mode == PsyiaEmitter.PsyiaEmitterMode.Time) {
				myTarget.EmitOverTime = EditorGUILayout.FloatField("Emit Over Time", Mathf.Max(0f, myTarget.EmitOverTime));
			} else if(myTarget.Mode == PsyiaEmitter.PsyiaEmitterMode.Distance) {
				myTarget.EmitOverDistance = EditorGUILayout.FloatField("Emit Over Distance", Mathf.Max(0f, myTarget.EmitOverDistance));
			}

			myTarget.Settings.InheritVelocity = EditorGUILayout.Slider("Inherit Velocity", myTarget.Settings.InheritVelocity, 0f, 1f);
			myTarget.Settings.EmissionRadius = EditorGUILayout.FloatField("Emission Radius", myTarget.Settings.EmissionRadius);
			myTarget.Settings.EmissionVelocity = EditorGUILayout.FloatField("Emission Velocity", myTarget.Settings.EmissionVelocity);

			*/

			serializedObject.ApplyModifiedProperties();
		}
	}

	[CustomPropertyDrawer(typeof(PsyiaEmitter.EmitterSettings))]
	public class EmitterSettingsDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			
			EditorGUILayout.LabelField("Emitted Particle Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(property.FindPropertyRelative("InheritVelocity"), new GUIContent("Inherit Velocity"));
			EditorGUILayout.PropertyField(property.FindPropertyRelative("EmissionRadius"), new GUIContent("Emission Radius"));
			EditorGUILayout.Slider(property.FindPropertyRelative("EmissionRadiusShell"), 0f, 1f, new GUIContent("Radius Shell"));
			EditorGUILayout.Slider(property.FindPropertyRelative("EmissionArcSize"), 0f, 1f, new GUIContent("Arc Size"));
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("MinSpawnVelocity"), new GUIContent("Min Emission Speed"));
				EditorGUILayout.PropertyField(property.FindPropertyRelative("MaxSpawnVelocity"), new GUIContent("Max Emission Speed"));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Slider(property.FindPropertyRelative("RandomiseDirection"), 0f, 1f, new GUIContent("Randomise Direction"));
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(property.FindPropertyRelative("MinSpawnMass"), new GUIContent("Min Spawn Mass"));
				EditorGUILayout.PropertyField(property.FindPropertyRelative("MaxSpawnMass"), new GUIContent("Max Spawn Mass"));
			EditorGUILayout.EndHorizontal();
			/*
			// Draw label
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			// Don't make child fields be indented
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			// Calculate rects
			var amountRect = new Rect(position.x, position.y, 30, position.height);
			var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
			var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

			// Draw fields - passs GUIContent.none to each so they are drawn without labels
			EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("InheritVelocity"), GUIContent.none);
			EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("EmissionRadius"), GUIContent.none);
			EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("EmissionVelocity"), GUIContent.none);

			// Set indent back to what it was
			EditorGUI.indentLevel = indent;

			*/

			EditorGUI.EndProperty();
		}
	}

}