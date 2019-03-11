using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsyiaUiSwitch : MonoBehaviour
{

	[Range(0f, 1f)] public float LerpTime = 0.1f;
	public bool ShowingControllerSettings = false;

	public Transform ControllerSettings;
	public Transform[] ControllerSettingsPanels;
	public Transform MixedSettings;
	public Transform[] MixedSettingsPanels;

	private float _currentValue;

	public void SetMixedSettings(int value)
	{
		ShowingControllerSettings = value == 0;
	}
	
	public void Update()
	{
		_currentValue = Mathf.Lerp(_currentValue, ShowingControllerSettings ? 1f : 0f, LerpTime);

		foreach (var t in ControllerSettingsPanels) {
			t.gameObject.SetActive(ShowingControllerSettings);
		}

		foreach (var t in MixedSettingsPanels) {
			t.gameObject.SetActive(!ShowingControllerSettings);
		}

		return;
		
		//ControllerSettings.localPosition = new Vector3(0f, 0f, Mathf.Lerp(-0.4f, 0f, _currentValue));
		ControllerSettings.localRotation = Quaternion.Euler(0f, Mathf.Lerp(-270f, 0f, _currentValue), 0f);
		//ControllerSettings.gameObject.SetActive(_currentValue > 0.05f);
		ControllerSettings.localPosition = _currentValue < 0.05f ? new Vector3(0f, -10000f, 0f) : Vector3.zero;

		foreach (var t in ControllerSettingsPanels) {
			t.localScale = new Vector3(1f, Mathf.Lerp(0.01f, 1f, _currentValue), 1f);
		}

		//MixedSettings.localPosition = new Vector3(0f, 0f, Mathf.Lerp(0f, -0.4f, _currentValue));
		MixedSettings.localRotation = Quaternion.Euler(0f, Mathf.Lerp(0f, 270f, _currentValue), 0f);
		//MixedSettings.gameObject.SetActive(_currentValue < 0.95f);
		MixedSettings.localPosition = _currentValue > 0.95f ? new Vector3(0f, -10000f, 0f) : Vector3.zero;
		
		foreach (var t in MixedSettingsPanels) {
			t.localScale = new Vector3(1f, Mathf.Lerp(1f, 0.01f, _currentValue), 1f);
		}
	}
}
