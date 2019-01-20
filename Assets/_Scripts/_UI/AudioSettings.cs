using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
	public PsyiaSettingsApplicator SettingsApplicator;
	public PsyiaMusic Music;

	public void SetLoop(bool value)
	{
		SettingsApplicator.SetLoop(value);
	}

	public void SetSlowWithTime(bool value)
	{
		SettingsApplicator.SetSlowWithTime(value);
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
		SettingsApplicator.SetVolume(value);
	}

	public void SetVisualAudioreactivityStrength(float value)
	{
		SettingsApplicator.SetVisualAudioreactivity(value);
	}
	
	public void SetPhysicsAudioreactivityStrength(float value)
	{
		SettingsApplicator.SetPhysicsAudioreactivity(value);
	}
}