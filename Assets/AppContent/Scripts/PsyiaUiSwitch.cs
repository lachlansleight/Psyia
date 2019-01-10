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
		ShowingControllerSettings = value == 1;
	}
	
	public void Update()
	{
		_currentValue = Mathf.Lerp(_currentValue, ShowingControllerSettings ? 1f : 0f, LerpTime);
		
		//ControllerSettings.localPosition = new Vector3(0f, 0f, Mathf.Lerp(-0.4f, 0f, _currentValue));
		ControllerSettings.rotation = Quaternion.Euler(0f, Mathf.Lerp(0f, 180f, _currentValue), 0f);
		ControllerSettings.gameObject.SetActive(_currentValue > 0.01f);

		foreach (var t in ControllerSettingsPanels) {
			t.localScale = new Vector3(1f, Mathf.Lerp(0f, 1f, _currentValue), 1f);
		}

		//MixedSettings.localPosition = new Vector3(0f, 0f, Mathf.Lerp(0f, -0.4f, _currentValue));
		MixedSettings.rotation = Quaternion.Euler(0f, Mathf.Lerp(-180f, 0f, _currentValue), 0f);
		MixedSettings.gameObject.SetActive(_currentValue < 0.99f);
		
		foreach (var t in MixedSettingsPanels) {
			t.localScale = new Vector3(1f, Mathf.Lerp(1f, 0f, _currentValue), 1f);
		}
	}
}
