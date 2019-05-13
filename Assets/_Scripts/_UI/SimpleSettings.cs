using System.Collections;
using System.Collections.Generic;
using Psyia;
using UnityEngine;

public class SimpleSettings : MonoBehaviour
{
	public PsyiaSettingsApplicator SettingsApplicator;
	public PsyiaJsonUiSetter UiSettingsApplicator;
	public PsyiaMusic Music;
	
	public GameObject ApplyButton;
	public PsyiaEmitter StartEmitter;

	[Space(10)]
	public AnimationCurve PowerToStrength = AnimationCurve.Linear(0f, 0f, 1f, 0.5f);
	public AnimationCurve ChaosToDamping = AnimationCurve.Linear(0f, 1f, 0f, 0.25f);
	public AnimationCurve ChaosToMass = AnimationCurve.Linear(0f, 0.5f, 1f, 0.05f);

	[Space(10)]
	public TextAsset[] Presets;
	public AudioClip[] PresetAudio;
	
	private int _storedCount;

	public void Start()
	{
		ApplyButton.SetActive(false);
	}
	
	public void SetParticleCount(float value)
	{
		_storedCount = Mathf.FloorToInt(value);
		ApplyButton.SetActive(true);
		SettingsApplicator.SetMaxParticleCount((int)value);
	}

	public void ApplyParticleCount()
	{
		SettingsApplicator.ApplyParticleCount();
		StartEmitter.Emit(Mathf.Min(StartEmitter.StartEmitCount, _storedCount * 1024));
		ApplyButton.SetActive(false);
	}

	public void SetAudioReactivity(float value)
	{
		SettingsApplicator.SetPhysicsAudioreactivity(value);
		SettingsApplicator.SetVisualAudioreactivity(value);
	}

	public void SetPower(float value)
	{
		SettingsApplicator.SetRightForceStrength(PowerToStrength.Evaluate(value));
		SettingsApplicator.SetLeftForceStrength(PowerToStrength.Evaluate(value));
	}

	public void SetChaos(float value)
	{
		SettingsApplicator.SetParticleDamping(ChaosToDamping.Evaluate(value));
		SettingsApplicator.SetParticleMass(ChaosToMass.Evaluate(value));
	}

	public void SetPreset(int value)
	{
		SettingsApplicator.TestJson = Presets[value];
		UiSettingsApplicator.LoadFromJson(Presets[value].text);
		SettingsApplicator.ApplyTestJson();
		Music.SetClip(PresetAudio[value]);
		
		SettingsApplicator.SetMaxParticleCount(SettingsApplicator.CurrentSettings.System.MaxParticleCount);
		SettingsApplicator.ApplyParticleCount();
		StartEmitter.Emit(Mathf.Min(StartEmitter.StartEmitCount, _storedCount * 1024));
	}
}
