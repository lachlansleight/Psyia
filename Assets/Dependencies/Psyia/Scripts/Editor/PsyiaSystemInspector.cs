/*

Copyright (c) 2018 Lachlan Sleight

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

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
			myTarget.Global.SimulationSpace = (Space)EditorGUILayout.EnumPopup("Simulation Space", myTarget.Global.SimulationSpace);
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Emission Settings", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("No emission settings!");
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Physics Settings", EditorStyles.boldLabel);
			myTarget.Physics.ParticleMinimumMass = EditorGUILayout.FloatField("Particle Minimum Mass", myTarget.Physics.ParticleMinimumMass);
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