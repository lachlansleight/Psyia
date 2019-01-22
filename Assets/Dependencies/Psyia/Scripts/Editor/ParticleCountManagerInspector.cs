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
			}
		}
	}
}
