using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Foliar.Compute {

	[CustomEditor(typeof(GpuBuffer))]
	public class GpuBufferInspector : Editor {

		public override void OnInspectorGUI() {
			GpuBuffer myTarget = (GpuBuffer)target;
			EditorGUILayout.LabelField("Buffer Info:", EditorStyles.largeLabel);
			if(myTarget.Buffer == null) {
				EditorGUILayout.LabelField("Buffer not initialized");
			} else {
				EditorGUILayout.EnumPopup("Buffer Type", myTarget.BufferType);
				if(myTarget.DataType != null) EditorGUILayout.TextField("Data Type", myTarget.DataType.Name);
				else EditorGUILayout.LabelField("No type");
				EditorGUILayout.IntField("Buffer stride", myTarget.Stride);
				EditorGUILayout.IntField("Buffer count", myTarget.Count);
			}
			
			
		}
	}

}