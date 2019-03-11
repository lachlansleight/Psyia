using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using eageramoeba.DeviceRating;

[CustomEditor(typeof(AutoQuality))]
public class AutoQualityEditor : Editor {
    private string[] selStrings = new string[] { '\u2196'.ToString(), '\u2191'.ToString(), '\u2197'.ToString(), '\u2190'.ToString(), "X", '\u2192'.ToString(), '\u2199'.ToString(), '\u2193'.ToString(), '\u2198'.ToString() };

    int butw;
    int sw;
    int bw;
    string[] qualityResults;
    int qualitySetting;

    float min;
    float max;

    SerializedObject m_object;
    SerializedProperty qualityBands;

    void OnEnable() {
        m_object = new SerializedObject(target);
        qualityBands = m_object.FindProperty("qualityBands");
        m_object.Update();
    }

    public override void OnInspectorGUI() {
        butw = 20;
        sw = 30;
        bw = 130;
        qualityResults = QualitySettings.names;
        qualitySetting = QualitySettings.GetQualityLevel();

        m_object.Update();
        DrawDefaultInspector();

        AutoQuality script = (AutoQuality)target;

        //startDelay.intValue = GUILayout.SelectionGrid(1, selStrings, 3, GUILayout.Width(80));

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Min", ""), EditorStyles.boldLabel, GUILayout.Width(sw));
        EditorGUILayout.LabelField(new GUIContent("Max", ""), EditorStyles.boldLabel, GUILayout.Width(sw));
        EditorGUILayout.LabelField(new GUIContent("Quality Setting ("+ qualityBands.arraySize + "/"+ qualityResults.Length + ")", ""), EditorStyles.boldLabel, GUILayout.MinWidth(bw));
        if (qualityBands.arraySize < qualityResults.Length) {
            if (GUILayout.Button(new GUIContent("+", ""), EditorStyles.miniButton, GUILayout.Width(butw))) {
                qualityBands.InsertArrayElementAtIndex(qualityBands.arraySize);
                if (qualityBands.arraySize > 1) {
                    qualityBands.GetArrayElementAtIndex(qualityBands.arraySize - 1).floatValue += qualityBands.GetArrayElementAtIndex(0).floatValue;
                } else {
                    qualityBands.GetArrayElementAtIndex(qualityBands.arraySize - 1).floatValue = 2;
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        if (qualityBands.arraySize > qualityResults.Length) {
            qualityBands.arraySize = qualityResults.Length;
        }

        min = 0;
        for (int i = 0; i < qualityBands.arraySize; i++) {
            max = qualityBands.GetArrayElementAtIndex(i).floatValue;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent(min.ToString(), ""), EditorStyles.label, GUILayout.Width(sw));
            qualityBands.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.FloatField(max, GUILayout.Width(sw));
            EditorGUILayout.LabelField(new GUIContent(qualityResults[i], ""), EditorStyles.label, GUILayout.MinWidth(bw));
            if (GUILayout.Button(new GUIContent("X", ""), EditorStyles.miniButton, GUILayout.Width(butw))) {
                qualityBands.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.EndHorizontal();
            min = max;
        }

        EditorGUILayout.Space();

        m_object.ApplyModifiedProperties();
    }
}