using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Foliar.Compute {

	[CustomEditor(typeof(GpuBuffer))]
	public class GpuBufferInspector : Editor {

		public override void OnInspectorGUI() {
			GpuBuffer myTarget = (GpuBuffer)target;

			myTarget.BufferType = (ComputeBufferType)EditorGUILayout.EnumPopup("Buffer type", myTarget.BufferType);

			EditorGUILayout.LabelField("Buffer Info:", EditorStyles.largeLabel);
			if(myTarget.DataType != null) EditorGUILayout.TextField("Data Type", myTarget.DataType.Name);
			else EditorGUILayout.LabelField("No type");
			if(myTarget.Buffer == null) EditorGUILayout.LabelField("Buffer not initialized");
			else {
				EditorGUILayout.IntField("Buffer stride", myTarget.Stride);
				EditorGUILayout.IntField("Buffer count", myTarget.Count);
			}
		}
	}

}