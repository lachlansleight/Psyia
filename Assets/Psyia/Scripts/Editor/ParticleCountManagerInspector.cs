using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UCTK;

[CustomEditor(typeof(ParticleCountManager))]
public class ParticleCountManagerInspector : Editor {

	public override void OnInspectorGUI() {
		ParticleCountManager myTarget = (ParticleCountManager)target;

		myTarget.ParticleCountFactor = EditorGUILayout.IntSlider("Particle Count", myTarget.ParticleCountFactor, 1, 1024);
		EditorGUILayout.LabelField("Final count: " + (myTarget.ParticleCountFactor * 1024));

		myTarget.ParticleBuffer = (BufferSetup)EditorGUILayout.ObjectField("Particle Buffer", myTarget.ParticleBuffer, typeof(BufferSetup), true);
		myTarget.DistanceBuffer = (BufferSetup)EditorGUILayout.ObjectField("Distance Buffer", myTarget.DistanceBuffer, typeof(BufferSetup), true);
		myTarget.DeadList = (BufferSetupWithDispatch)EditorGUILayout.ObjectField("Dead List", myTarget.DeadList, typeof(BufferSetupWithDispatch), true);

		if(myTarget.ParticleBuffer != null) {
			if(Application.isPlaying) {
				if((myTarget.ParticleCountFactor * 1024) != myTarget.ParticleBuffer.Count) {
					if(GUILayout.Button("Apply Particle Count")) {
						myTarget.ApplyParticleCount();
					}
				}
			} else {
				myTarget.ApplyParticleCount();
			}
		}
	}
}
