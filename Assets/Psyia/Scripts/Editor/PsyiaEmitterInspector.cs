using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UCTK;

namespace Psyia {

	[CustomEditor(typeof(PsyiaEmitter))]
	public class PsyiaEmitterInspector : Editor {

		public override void OnInspectorGUI() {
			PsyiaEmitter myTarget = (PsyiaEmitter)target;

			myTarget.Mode = (PsyiaEmitter.PsyiaEmitterMode)EditorGUILayout.EnumPopup("Emission Mode", myTarget.Mode);
			if(myTarget.Mode == PsyiaEmitter.PsyiaEmitterMode.Start) {
				myTarget.StartEmitCount = EditorGUILayout.IntField("Start Emit Count", myTarget.StartEmitCount);
			} else if(myTarget.Mode == PsyiaEmitter.PsyiaEmitterMode.Time) {
				myTarget.EmitOverTime = EditorGUILayout.FloatField("Emit Over Time", Mathf.Max(0f, myTarget.EmitOverTime));
			} else if(myTarget.Mode == PsyiaEmitter.PsyiaEmitterMode.Distance) {
				myTarget.EmitOverDistance = EditorGUILayout.FloatField("Emit Over Distance", Mathf.Max(0f, myTarget.EmitOverDistance));
			}

			myTarget.InheritVelocity = EditorGUILayout.Slider("Inherit Velocity", myTarget.InheritVelocity, 0f, 1f);
		}
	}

}