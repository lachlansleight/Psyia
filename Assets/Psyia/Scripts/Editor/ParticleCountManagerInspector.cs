using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Foliar.Compute;

[CustomEditor(typeof(ParticleCountManager))]
public class ParticleCountManagerInspector : Editor {

	public override void OnInspectorGUI() {
		ParticleCountManager myTarget = (ParticleCountManager)target;

		myTarget.ParticleCountFactor = EditorGUILayout.IntSlider("Particle Count", myTarget.ParticleCountFactor, 1, 1024);
		EditorGUILayout.LabelField("Final count: " + (myTarget.ParticleCountFactor * 1024));

		myTarget.ParticleBuffer = (BufferSetup)EditorGUILayout.ObjectField("Particle Buffer", myTarget.ParticleBuffer, typeof(BufferSetup), true);
		myTarget.DistanceBuffer = (BufferSetup)EditorGUILayout.ObjectField("Particle Buffer", myTarget.DistanceBuffer, typeof(BufferSetup), true);
		myTarget.DeadList = (BufferSetupWithDispatch)EditorGUILayout.ObjectField("Particle Buffer", myTarget.DeadList, typeof(BufferSetupWithDispatch), true);
		myTarget.Spawner = (StartSpawner)EditorGUILayout.ObjectField("Spawner", myTarget.Spawner, typeof(StartSpawner), true);

		if(myTarget.ParticleBuffer != null) {
			if((myTarget.ParticleCountFactor * 1024) != myTarget.ParticleBuffer.Count) {
				if(GUILayout.Button("Apply Particle Count")) {
					myTarget.ApplyParticleCount();
				}
			}
		}
	}
}
