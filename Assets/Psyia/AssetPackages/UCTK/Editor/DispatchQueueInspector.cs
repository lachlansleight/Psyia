using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UCTK {

	[CustomEditor(typeof(DispatchQueue))]
	public class DispatchQueueInspector : Editor {

		public override void OnInspectorGUI() {
			DispatchQueue myTarget = (DispatchQueue)target;

			myTarget.RunOnAwake = EditorGUILayout.Toggle("Run on Awake", myTarget.RunOnAwake);
			myTarget.RunOnUpdate = EditorGUILayout.Toggle("Run on Update", myTarget.RunOnUpdate);
			if(GUILayout.Button("Manual Run Queue")) {
				myTarget.RunQueue();
			}

			EditorGUILayout.LabelField("Current Queue:", EditorStyles.boldLabel);
			for(int i = 0; i < myTarget.transform.childCount; i++) {
				myTarget.transform.GetChild(i).gameObject.SetActive(EditorGUILayout.Toggle(myTarget.transform.GetChild(i).name, myTarget.transform.GetChild(i).gameObject.activeSelf));
			}

			if(GUILayout.Button("Add Dispatch Item")) {
				GameObject NewObj = new GameObject("Dispatch Item");
				NewObj.transform.parent = myTarget.transform;
				NewObj.AddComponent<DispatchQueueItem>();
			}
		}
	}
}