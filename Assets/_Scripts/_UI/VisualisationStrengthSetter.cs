using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualisationStrengthSetter : MonoBehaviour
{

	public SliderAudioHook[] Visuals;
	public SliderAudioHook[] Physics;

	public void SetVisualStrength(float value)
	{
		foreach (var h in Visuals) h.VisualisationStrength = value;
	}
	
	public void SetPhysicsStrength(float value)
	{
		foreach (var h in Physics) h.VisualisationStrength = value;
	}
}
