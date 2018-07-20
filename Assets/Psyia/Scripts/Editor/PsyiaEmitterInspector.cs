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

		SerializedProperty SettingsProperty;

		void OnEnable() {
			ModeProperty = serializedObject.FindProperty("_Mode");
			StartEmitCountProperty = serializedObject.FindProperty("StartEmitCount");
			EmitOverTimeProperty = serializedObject.FindProperty("EmitOverTime");
			EmitOverDistanceProperty = serializedObject.FindProperty("EmitOverDistance");

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
	public class EmissionSettingsDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			
			EditorGUILayout.LabelField("Emitted Particle Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(property.FindPropertyRelative("InheritVelocity"), new GUIContent("Inherit Velocity"));
			EditorGUILayout.PropertyField(property.FindPropertyRelative("EmissionRadius"), new GUIContent("Emission Radius"));
			EditorGUILayout.PropertyField(property.FindPropertyRelative("EmissionVelocity"), new GUIContent("Emission Velocity"));
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