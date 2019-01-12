using System.Collections;
using System.Collections.Generic;
using Psyia;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using XRP;

public class SystemSettings : MonoBehaviour
{
	public PsyiaSettingsApplicator SettingsApplicator;
	
	public XrpSlider CountSlider;
	public GameObject ApplyButton;
	public PsyiaEmitter StartEmitter;

	private int _storedCount;

	public void Start()
	{
		ApplyButton.SetActive(false);
		//StartCoroutine(ApplyParticleCountAfterFrame());
	}

	private IEnumerator ApplyParticleCountAfterFrame()
	{
		yield return new WaitForSeconds(2f);
		_storedCount = (int)CountSlider.CurrentValue;
		ApplyParticleCount();
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

	public void SetAntialiasing(int value)
	{
		switch (value) {
			case 0:
				SettingsApplicator.SetAntialiasing(0);
				break;
			case 1:
				SettingsApplicator.SetAntialiasing(2);
				break;
			case 2:
				SettingsApplicator.SetAntialiasing(4);
				break;
			case 3:
				SettingsApplicator.SetAntialiasing(8);
				break;
			default:
				throw new System.FormatException("Unexpected value '" + value + "' for SetAntialiasing!");
		}
	}

	public void SetBloom(bool value)
	{
		SettingsApplicator.SetBloom(value);
	}
	

}
