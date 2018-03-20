using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Foliar.Compute {

	[CustomEditor(typeof(DispatchQueue))]
	public class DispatchQueueInspector : Editor {

		public override void OnInspectorGUI() {
			DispatchQueue myTarget = (DispatchQueue)target;

			for(int i = 0; i < myTarget.Dispatchers.Count; i++) {
				myTarget.Dispatchers[i].Dispatcher = (ComputeDispatcher)EditorGUILayout.ObjectField("Dispatcher", myTarget.Dispatchers[i].Dispatcher, typeof(ComputeDispatcher), true);

				EditorGUILayout.BeginHorizontal();
					myTarget.Dispatchers[i].Enabled = EditorGUILayout.Toggle("Enabled", myTarget.Dispatchers[i].Enabled);
					myTarget.Dispatchers[i].DispatchInterval = Mathf.RoundToInt(Mathf.Max(1, EditorGUILayout.IntField("Dispatch Interval", myTarget.Dispatchers[i].DispatchInterval)));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				if(i > 0) {
					if(GUILayout.Button("^")) {
						myTarget.MoveItemUp(i);
					}
				}
				if(i < myTarget.Dispatchers.Count - 1) {
					if(GUILayout.Button("v")) {
						myTarget.MoveItemDown(i);
					}
				}
				if(GUILayout.Button("x")) {
					myTarget.RemoveItem(i);
				}
				EditorGUILayout.EndHorizontal();
			}
			if(GUILayout.Button("Add Item")) {
				myTarget.AddItem();
			}
		}
	}
}