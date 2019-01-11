using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
	public PsyiaMusic Music;

	public void SetLoop(bool value)
	{
		Music.Loop = value;
	}

	public void SetSlowWithTime(bool value)
	{
		//TODO: Implement this
		//Music.SlowWithTime = value;
	}

	public void NextTrack()
	{
		Music.NextTrack();
	}

	public void PreviousTrack()
	{
		Music.PreviousTrack();
	}

	public void PlayPause()
	{
		Music.PlayPause();
	}

	public void SetVolume(float value)
	{
		Music.Volume = value;
	}

	public void SetVisualAudioreactivityStrength(float value)
	{
		//TODO: Implement this
		//PsyiaSystem.VisualReactivityStrength = value;
	}
	
	public void SetPhysicsAudioreactivityStrength(float value)
	{
		//TODO: Implement this
		//PsyiaSystem.PhysicsReactivityStrength = value;
	}
}