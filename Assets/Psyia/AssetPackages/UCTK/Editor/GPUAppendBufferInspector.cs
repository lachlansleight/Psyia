using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UCTK {

	[CustomEditor(typeof(GpuAppendBuffer))]
	public class GPUAppendBufferInspector : GpuBufferInspector {

		public int LastRecordedCount;

		public override void OnInspectorGUI() {
			GpuAppendBuffer myTarget = (GpuAppendBuffer)target;
			
			base.OnInspectorGUI();

			if(myTarget.Buffer != null) {
				EditorGUILayout.Space();
				EditorGUILayout.IntField("Current count", LastRecordedCount);
				if(GUILayout.Button("Update current count display")) {
					LastRecordedCount = myTarget.CurrentCount;
				}
			}
		}
	}

}