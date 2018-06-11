using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PsyiaRTA_StateKey))]
public class PsyiaRTA_StateKeyInspector : Editor {

	public bool IsFoldedOut = false;

	public override void OnInspectorGUI() {
		PsyiaRTA_StateKey myTarget = (PsyiaRTA_StateKey)target;

		if(EditorApplication.isPlaying && Selection.activeObject == myTarget) {
			EditorUtility.SetDirty(target);
			EditorGUILayout.Slider("Trigger Value", myTarget.TriggerValue, 0f, 1f);
			EditorGUILayout.Slider("Lerp Value", myTarget.LerpValue, 0f, 1f);
			IsFoldedOut = EditorGUILayout.Foldout(IsFoldedOut, "Show Metric Values");
			if(IsFoldedOut) {
				for(int i = 0; i < myTarget.Metrics.Length; i++) {
					EditorGUILayout.LabelField(myTarget.Metrics[i].Metric.name + "/" + myTarget.Metrics[i].Timespan);
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Slider("Trigger Value", myTarget.Metrics[i].TriggerValue, 0f, 2f);
					EditorGUILayout.Slider("Lerp Value", myTarget.Metrics[i].LerpValue, 0f, 1f);
					EditorGUILayout.EndHorizontal();
				}
			}
		}

		EditorGUILayout.LabelField("Metrics", EditorStyles.boldLabel);
		for(int i = 0; i < myTarget.Metrics.Length; i++) {
			EditorGUILayout.BeginHorizontal();
			myTarget.Metrics[i].Metric = (PsyiaRTA_Metric)EditorGUILayout.ObjectField(myTarget.Metrics[i].Metric, typeof(PsyiaRTA_Metric));
			myTarget.Metrics[i].Timespan = (PsyiaRTA.MetricTimespan)EditorGUILayout.EnumPopup(myTarget.Metrics[i].Timespan);
			if(GUILayout.Button("X")) {
				myTarget.RemoveMetric(i);
				return;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			myTarget.Metrics[i].TriggerRangeMin = EditorGUILayout.FloatField("Trigger Range", myTarget.Metrics[i].TriggerRangeMin);
			myTarget.Metrics[i].TriggerRangeMax = EditorGUILayout.FloatField(myTarget.Metrics[i].TriggerRangeMax);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			myTarget.Metrics[i].LerpRangeMin = EditorGUILayout.FloatField("Lerp Range", myTarget.Metrics[i].LerpRangeMin);
			myTarget.Metrics[i].LerpRangeMax = EditorGUILayout.FloatField(myTarget.Metrics[i].LerpRangeMax);
			EditorGUILayout.EndHorizontal();
		}
		if(GUILayout.Button("Add Metric")) {
			myTarget.AddMetric();
		}
	}
}
