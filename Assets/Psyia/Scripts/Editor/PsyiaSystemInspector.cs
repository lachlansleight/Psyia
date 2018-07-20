using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UCTK;

namespace Psyia {

	[CustomEditor(typeof(PsyiaSystem))]
	public class ParticleCountManagerInspector : Editor {

		MaterialEditor matEditor;

		public override void OnInspectorGUI() {
			PsyiaSystem myTarget = (PsyiaSystem)target;

			EditorGUILayout.LabelField("Global Settings", EditorStyles.boldLabel);
			myTarget.Global.MaxParticleCount = EditorGUILayout.IntSlider("Max Particle Count (1000s)", myTarget.Global.MaxParticleCount, 1, 1000);
			//myTarget.Global.DefaultParticleSize = EditorGUILayout.FloatField("Default Particle Size", myTarget.Global.DefaultParticleSize);
			myTarget.Global.ParticleLifetime = EditorGUILayout.FloatField("Particle Lifetime", myTarget.Global.ParticleLifetime);
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Emission Settings", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("No emission settings!");
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Physics Settings", EditorStyles.boldLabel);
			myTarget.Physics.ParticleMass = EditorGUILayout.FloatField("Particle Mass", myTarget.Physics.ParticleMass);
			myTarget.Physics.ParticleDrag = EditorGUILayout.FloatField("Particle Drag", myTarget.Physics.ParticleDrag);	
			myTarget.Physics.ForceMultiplier = EditorGUILayout.FloatField("Force Multiplier", myTarget.Physics.ForceMultiplier);	
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Rendering Settings", EditorStyles.boldLabel);
			Material NewParticleMat = (Material)EditorGUILayout.ObjectField("Particle Material", myTarget.Renderer.ParticleMaterial, typeof(Material), true);
			if(NewParticleMat != myTarget.Renderer.ParticleMaterial) {
				Debug.Log("Setting particle material");
				myTarget.Renderer.ParticleMaterial = NewParticleMat;
				matEditor = (MaterialEditor)CreateEditor(myTarget.Renderer.ParticleMaterial);
			}
			myTarget.Renderer.ChromaY = EditorGUILayout.Slider("Chroma Y", myTarget.Renderer.ChromaY, 0f, 1f);
			
			if(myTarget.Renderer.ParticleMaterial != null) {
				if(matEditor == null) {
					matEditor = (MaterialEditor)CreateEditor(myTarget.Renderer.ParticleMaterial);
				}
				if(matEditor != null) {
					matEditor.DrawHeader();
					bool isDefaultMaterial = !AssetDatabase.GetAssetPath (myTarget.Renderer.ParticleMaterial).StartsWith ("Assets");
 
					using (new EditorGUI.DisabledGroupScope(isDefaultMaterial)) {
						matEditor.OnInspectorGUI (); 
					}
				}
			}
		}
	}

}