using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(PsyiaRTA_MetricPopulator))]
public class PsyiaRTA_MetricPopulatorInspector : Editor {

	public override void OnInspectorGUI() {
		PsyiaRTA_MetricPopulator myTarget = (PsyiaRTA_MetricPopulator)target;

		myTarget.Metric = (PsyiaRTA_Metric)EditorGUILayout.ObjectField("Target Metric", myTarget.Metric, typeof(PsyiaRTA_Metric), false);
		EditorGUILayout.Space();

		//Get Object
		EditorGUI.BeginChangeCheck();
		myTarget.TargetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", myTarget.TargetObject, typeof(GameObject), true);
		if(EditorGUI.EndChangeCheck()) {
			//Reset everything below if changed
			myTarget.TargetComponent = null;
			myTarget.TargetField = null;
			myTarget.TargetProperty = null;
			myTarget.IsVector = false;
		}

		if(myTarget.TargetObject == null) {
			return;
		}

		//Get Component
		EditorGUI.BeginChangeCheck();
		Component[] Components = myTarget.TargetObject.GetComponents<Component>();
		

		string[] ComponentNames = new string[Components.Length];
		for(int i = 0; i < Components.Length; i++) {
			ComponentNames[i] = Components[i].GetType().Name;
		}

		int SelectedIndex = myTarget.TargetComponent == null ? 0 : System.Array.IndexOf(Components, myTarget.TargetComponent);
		if(SelectedIndex < 0) SelectedIndex = 0;

		myTarget.TargetComponent = Components[EditorGUILayout.Popup("Target Component", SelectedIndex, ComponentNames)];

		if(EditorGUI.EndChangeCheck()) {
			//Reset everything below if changed
			myTarget.TargetField = null;
			myTarget.TargetProperty = null;
			myTarget.IsVector = false;
		}

		if(myTarget.TargetComponent == null) {
			return;
		}

		FieldInfo[] Fields = myTarget.TargetComponent.GetType().GetFields();
		PropertyInfo[] Properties = myTarget.TargetComponent.GetType().GetProperties();

		List<FieldInfo> TypedFields = new List<FieldInfo>();
		for(int i = 0; i < Fields.Length; i++) {
			if(System.Array.IndexOf(PsyiaRTA.AllowedTypes, Fields[i].FieldType) != -1) {
				TypedFields.Add(Fields[i]);
			}
		}
		Fields = TypedFields.ToArray();

		List<PropertyInfo> TypedProperties = new List<PropertyInfo>();
		for(int i = 0; i < Properties.Length; i++) {
			if(System.Array.IndexOf(PsyiaRTA.AllowedTypes, Properties[i].PropertyType) != -1) {
				TypedProperties.Add(Properties[i]);
			}
		}
		Properties = TypedProperties.ToArray();

		string[] AccessorNames = new string[Fields.Length + Properties.Length];
		if(AccessorNames.Length == 0) {
			EditorGUILayout.LabelField("No public fields or properties");
			return;
		}

		for(int i = 0; i < Fields.Length; i++) {
			AccessorNames[i] = Fields[i].Name + " (" + Fields[i].FieldType + ")";
		}
		for(int i = 0; i < Properties.Length; i++) {
			AccessorNames[i+Fields.Length] = Properties[i].Name + " (" + Properties[i].PropertyType + ")";
		}

		if(myTarget.TargetField == null && myTarget.TargetProperty == null) {
			SelectedIndex = 0;
		} else if(myTarget.TargetField != null && !myTarget.UsingProperty) {
			SelectedIndex = System.Array.IndexOf(Fields, myTarget.TargetField);
		} else if(myTarget.TargetProperty != null && myTarget.UsingProperty) {
			SelectedIndex = System.Array.IndexOf(Properties, myTarget.TargetProperty) + Fields.Length;
		}

		int ChosenIndex = EditorGUILayout.Popup("Target Value", SelectedIndex, AccessorNames);
		if(ChosenIndex < Fields.Length) {
			myTarget.UsingProperty = false;
			myTarget.TargetField = Fields[ChosenIndex];
			myTarget.FieldName = Fields[ChosenIndex].Name;
			myTarget.TargetProperty = null;
		} else {
			myTarget.UsingProperty = true;
			myTarget.TargetField = null;
			myTarget.TargetProperty = Properties[ChosenIndex - Fields.Length];
			myTarget.PropertyName = Properties[ChosenIndex - Fields.Length].Name;
		}

		if(myTarget.TargetField != null) {
			if(myTarget.TargetField.FieldType != typeof(int) && myTarget.TargetField.FieldType != typeof(float) && myTarget.TargetField.FieldType != typeof(Vector2) && myTarget.TargetField.FieldType != typeof(Vector3)) {
				EditorGUILayout.LabelField("Warning: type should be a vector or numeric type", EditorStyles.helpBox);
			}
		} else if(myTarget.TargetProperty != null) {
			if(myTarget.TargetProperty.PropertyType != typeof(int) && myTarget.TargetProperty.PropertyType != typeof(float) && myTarget.TargetProperty.PropertyType != typeof(Vector2) && myTarget.TargetProperty.PropertyType != typeof(Vector3)) {
				EditorGUILayout.LabelField("Warning: type should be a vector or numeric type", EditorStyles.helpBox);
			}
		}

		if(myTarget.TargetProperty == null && myTarget.TargetField == null) {
			return;
		}

		if(myTarget.UsingProperty) {
			//Debug.Log(myTarget.TargetProperty.PropertyType);
			if(myTarget.TargetProperty.PropertyType == typeof(Vector2)) {
				myTarget.IsVector = true;
				string[] VectorIndices = new string[] {"x", "y"};
				myTarget.VectorIndex = EditorGUILayout.Popup("Vector index", myTarget.VectorIndex, VectorIndices);
			} else if(myTarget.TargetProperty.PropertyType == typeof(Vector3)) {
				myTarget.IsVector = true;
				string[] VectorIndices = new string[] {"x", "y", "z"};
				myTarget.VectorIndex = EditorGUILayout.Popup("Vector index", myTarget.VectorIndex, VectorIndices);
			} else if(myTarget.TargetProperty.PropertyType == typeof(Vector4)) {
				myTarget.IsVector = true;
				string[] VectorIndices = new string[] {"x", "y", "z", "w"};
				myTarget.VectorIndex = EditorGUILayout.Popup("Vector index", myTarget.VectorIndex, VectorIndices);
			} else {
				myTarget.IsVector = false;
			}
		} else {
			if(myTarget.TargetField.FieldType == typeof(Vector2)) {
				myTarget.IsVector = true;
				string[] VectorIndices = new string[] {"x", "y"};
				myTarget.VectorIndex = EditorGUILayout.Popup("Vector index", myTarget.VectorIndex, VectorIndices);
			} else if(myTarget.TargetField.FieldType == typeof(Vector3)) {
				myTarget.IsVector = true;
				string[] VectorIndices = new string[] {"x", "y", "z"};
				myTarget.VectorIndex = EditorGUILayout.Popup("Vector index", myTarget.VectorIndex, VectorIndices);
			} else if(myTarget.TargetField.FieldType == typeof(Vector4)) {
				myTarget.IsVector = true;
				string[] VectorIndices = new string[] {"x", "y", "z", "w"};
				myTarget.VectorIndex = EditorGUILayout.Popup("Vector index", myTarget.VectorIndex, VectorIndices);
			} else {
				myTarget.IsVector = false;
			}
		}

		if(EditorApplication.isPlaying && Selection.activeObject == myTarget.gameObject) {			
			EditorUtility.SetDirty( target );
			EditorGUILayout.LabelField("Average Values", EditorStyles.boldLabel);
			
			if(myTarget.Metric.Lists != null) {
				foreach(PsyiaRTA_MetricList list in myTarget.Metric.Lists) {
					EditorGUILayout.Slider(list.Timespan.ToString(), list.Average, -5f, 5f);
				}
			}
		}
	}
}
