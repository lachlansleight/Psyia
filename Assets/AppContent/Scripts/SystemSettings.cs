using System.Collections;
using System.Collections.Generic;
using Psyia;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using XRP;

public class SystemSettings : MonoBehaviour
{
	public ParticleCountManager CountManager;
	public XrpSlider CountSlider;
	public GameObject ApplyButton;
	public PostProcessVolume PostVolume;
	public PsyiaEmitter StartEmitter;

	private int _storedCount;

	public void Start()
	{
		ApplyButton.SetActive(false);
		StartCoroutine(ApplyParticleCountAfterFrame());
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
	}

	public void ApplyParticleCount()
	{
		CountManager.ParticleCountFactor = _storedCount;
		CountManager.ApplyParticleCount();
		StartEmitter.Emit(Mathf.Min(StartEmitter.StartEmitCount, CountManager.ParticleCountFactor * 1024));
		ApplyButton.SetActive(false);
	}

	public void SetAntialiasing(int value)
	{
		switch (value) {
			case 0:
				QualitySettings.antiAliasing = 0;
				break;
			case 1:
				QualitySettings.antiAliasing = 2;
				break;
			case 2:
				QualitySettings.antiAliasing = 4;
				break;
			case 3:
				QualitySettings.antiAliasing = 8;
				break;
			default:
				throw new System.FormatException("Unexpected value '" + value + "' for SetAntialiasing!");
		}
	}

	public void SetBloom(bool value)
	{
		Bloom bloomLayer;
		if (PostVolume.profile.TryGetSettings(out bloomLayer)) {
			bloomLayer.enabled.value = value;
		} else {
			Debug.Log("No bloom found");
		}
	}
	

}
