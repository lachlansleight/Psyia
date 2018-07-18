using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UCTK;

namespace Psyia {

	[CustomEditor(typeof(PsyiaSystem))]
	public class ParticleCountManagerInspector : Editor {

		public override void OnInspectorGUI() {
			PsyiaSystem myTarget = (PsyiaSystem)target;

			EditorGUILayout.LabelField("Global Settings");
			myTarget.Global.MaxParticleCount = EditorGUILayout.IntSlider("Max Particle Count (1000s)", myTarget.Global.MaxParticleCount, 1, 1000);
			myTarget.Global.DefaultParticleSize = EditorGUILayout.FloatField("Default Particle Size", myTarget.Global.DefaultParticleSize);
		}
	}

}