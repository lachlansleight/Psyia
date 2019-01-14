using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioreactivityTest : MonoBehaviour
{

	public AudioData AudioData;
	public Transform MainTransform;
	public Transform[] MiniTransforms;

	public void Update()
	{
		MainTransform.localScale = Vector3.one * (0.1f + 0.9f * AudioData.AverageLevel);
		foreach (var t in MiniTransforms) {
			var scale = t.localScale;
			scale.x = scale.z = 0.1f + 0.9f * AudioData.InstantLevel;
			t.localScale = scale;
		}
	}
}
