using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PsyiaRTA_Metric))]
public class PsyiaRTA_MetricInspector : Editor {

	public override void OnInspectorGUI() {
		PsyiaRTA_Metric myTarget = (PsyiaRTA_Metric)target;

		if(Selection.activeObject == myTarget) {			
			EditorUtility.SetDirty( target );
		}

		EditorGUILayout.LabelField("List averages", EditorStyles.boldLabel);
		string[] Names = System.Enum.GetNames(typeof(PsyiaRTA.MetricTimespan));
		for(int i = 0; i < Names.Length; i++) {
			EditorGUILayout.Slider(Names[i], myTarget.GetListByTimespan((PsyiaRTA.MetricTimespan)i).Average, -5f, 5f);
		}
	}
}
