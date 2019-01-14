using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRP;

public class SliderAudioHook : MonoBehaviour
{
	public enum AudioDataSource
	{
		None,
		Instant,
		Average
	}

	public AudioData AudioData;
	public AudioDataSource Source = AudioDataSource.None;
	[Range(0f, 1f)] public float VisualisationStrength = 0.5f;

	private XrpSlider _myXrpSlider;
	private float _centralSliderValue;
	
	public void Awake()
	{
		_myXrpSlider = GetComponent<XrpSlider>();
		_centralSliderValue = _myXrpSlider.CurrentValue;
	}

	public void OnEnable()
	{
		_myXrpSlider.OnValueChanged += HandleNewSliderValue;
	}

	public void OnDisable()
	{
		_myXrpSlider.OnValueChanged -= HandleNewSliderValue;
	}

	private void HandleNewSliderValue(float value)
	{
		_centralSliderValue = value;
	}

	public void Update()
	{
		if (Source == AudioDataSource.None) return;
		var newValue = _centralSliderValue;

		newValue = Mathf.Lerp(
			_centralSliderValue * (1f - VisualisationStrength),
			_centralSliderValue * (1f + VisualisationStrength),
			Source == AudioDataSource.Instant ? AudioData.InstantLevel : AudioData.AverageLevel
		);

		if (_myXrpSlider.CurrentState == State.Inactive) {
			_myXrpSlider.CurrentValue = newValue;
			_myXrpSlider.OnValueChangedEvent.Invoke(_myXrpSlider.CurrentValue);
		} else {
			_myXrpSlider.CurrentValue = newValue;
		}
	}

	public void SetStrength(float value)
	{
		VisualisationStrength = Mathf.Clamp01(value);
	}
}
