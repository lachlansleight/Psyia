using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveGameInterface))]
public class SaveGameInterfaceInspector : Editor
{
	public override void OnInspectorGUI()
	{
		var sgi = (SaveGameInterface) target;
		EditorGUILayout.LabelField($"PlayCount\t{sgi.PlayCount}");
		EditorGUILayout.Space();
		EditorGUILayout.LabelField($"WhiteInfiniteMode\t{sgi.InfiniteModeWhite}");
		EditorGUILayout.LabelField($"RedInfiniteMode\t{sgi.InfiniteModeRed}");
		EditorGUILayout.LabelField($"BlueInfiniteMode\t{sgi.InfiniteModeBlue}");
		EditorGUILayout.LabelField($"GreenInfiniteMode\t{sgi.InfiniteModeGreen}");
		EditorGUILayout.LabelField($"PinkInfiniteMode\t{sgi.InfiniteModePink}");
	}
}