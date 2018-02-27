using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Foliar.Compute {

	[CustomEditor(typeof(GpuAppendBuffer))]
	public class GPUAppendBufferInspector : GpuBufferInspector {

		public override void OnInspectorGUI() {
			GpuAppendBuffer myTarget = (GpuAppendBuffer)target;

			base.OnInspectorGUI();

			if(myTarget.Buffer != null) {
				EditorGUILayout.Space();
				EditorGUILayout.IntField("Current count", myTarget.CurrentCount);
			}
			
		}
	}

}