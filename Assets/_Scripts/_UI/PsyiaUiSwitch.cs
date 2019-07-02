using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XRP;

public class PsyiaUiSwitch : MonoBehaviour
{

	[Range(0f, 1f)] public float LerpTime = 0.1f;
	public bool ShowingControllerSettings = false;
	public bool ShowingSimpleSettings = true;

	public Transform AdvancedSettings;
	public Transform SimpleSettings;
	public XrpPanel AdvancedPanel;
	public XrpPanel SimplePanel;
	public Text SimpleAdvancedText;
	
	[Space(10)]
	
	public Transform ControllerSettings;
	public Transform[] ControllerSettingsPanels;
	public Transform MixedSettings;
	public Transform[] MixedSettingsPanels;
	public Sprite MixedIcon;
	public Sprite ControllerIcon;
	public Image ControllerMixedButton;

	private bool _lerpingSimpleAdvanced = false;
	private float _currentValue;

	public void ToggleMixedSettings()
	{
		SetMixedSettings(!ShowingControllerSettings);
	}
	
	public void SetMixedSettings(int value)
	{
		Debug.Log($"Setting mixed settings to {value}");
		ShowingControllerSettings = value == 0;
		ControllerMixedButton.sprite = ShowingControllerSettings ? MixedIcon : ControllerIcon;
	}
	
	public void SetMixedSettings(bool value)
	{
		Debug.Log($"Setting mixed settings to {value}");
		ShowingControllerSettings = value;
		ControllerMixedButton.sprite = ShowingControllerSettings ? MixedIcon : ControllerIcon;
	}

	public void ToggleSimpleSettings()
	{
		SetSimpleSettings(!ShowingSimpleSettings);
	}

	public void BackToMenu()
	{
		var modeChoreography = FindObjectOfType<ModeChoreography>();
		if (modeChoreography != null) {
			modeChoreography.ReturnToMenu();
		}
	}

	public void SetSimpleSettings(bool value)
	{
		if (_lerpingSimpleAdvanced) return;

		Debug.Log("Setting simple settings to " + value);
		
		StartCoroutine(LerpSimpleAdvanced(!value));
	}

	private IEnumerator LerpSimpleAdvanced(bool toAdvanced)
	{
		AdvancedPanel.enabled = false;
		SimplePanel.enabled = false;
		
		_lerpingSimpleAdvanced = true;
		
		for (var i = 0f; i < 1f; i += Time.deltaTime / LerpTime) {
			if (toAdvanced) {
				SimpleSettings.localScale = Vector3.one * Mathf.Lerp(1f, 0f, i);
			} else {
				AdvancedSettings.localScale = Vector3.one * Mathf.Lerp(1f, 0f, i);
			}
			yield return null;
		}
		if (toAdvanced) {
			SimpleSettings.localScale = Vector3.zero;
		} else {
			AdvancedSettings.localScale = Vector3.zero;
		}

		SimpleAdvancedText.text = toAdvanced ? "Simple" : "Advanced";
		
		for (var i = 0f; i < 1f; i += Time.deltaTime / LerpTime) {
			if (toAdvanced) {
				AdvancedSettings.localScale = Vector3.one * Mathf.Lerp(0f, 1f, i);
			} else {
				SimpleSettings.localScale = Vector3.one * Mathf.Lerp(0f, 1f, i);
			}
			yield return null;
		}
		if (toAdvanced) {
			AdvancedSettings.localScale = Vector3.one;
		} else {
			SimpleSettings.localScale = Vector3.one;
		}

		_lerpingSimpleAdvanced = false;
		ShowingSimpleSettings = !toAdvanced;
		AdvancedPanel.enabled = toAdvanced;
		SimplePanel.enabled = !toAdvanced;
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
