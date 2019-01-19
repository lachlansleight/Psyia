using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using XRP;

public class MenuToggler : MonoBehaviour
{
	public SteamVR_Action_Boolean ToggleMenuAction;
	public SteamVR_Action_Vibration HapticAction;
	public SteamVR_Input_Sources Hand;
	public SteamVR_Input_Sources LeftHand;
	public SteamVR_Input_Sources RightHand;
	public ModeChoreography Choreography;

	public bool AllowMenuToggle = true;
	[Range(0f, 1f)] public float LerpSpeed = 0.2f;
	public Transform MenuObject;
	public XrpPanel MenuPanel;
	public Transform MenuHeight;
	public Transform HMDObject;
	public float HeightOffset = -0.39f;
	public float ReturnToMenuTime = 2f;

	public bool _targetValue;
	private float _currentValue;
	public float _holdTime;
	private bool _down;
	private float _lastHapticTime;
	
	public void OnEnable ()
	{
		ToggleMenuAction.AddOnChangeListener(OnAxisValueChanged, Hand);
	}

	public void OnDisable()
	{
		ToggleMenuAction.RemoveOnChangeListener(OnAxisValueChanged, Hand);
	}

	public void Update()
	{
		_currentValue = Mathf.Lerp(_currentValue, _targetValue ? 1f : 0f, LerpSpeed);
		MenuObject.localScale = Vector3.one * _currentValue;

		if (MenuObject.localScale.x < 0.01f) {
			MenuObject.transform.position = HMDObject.position + HMDObject.forward * -1f;
			MenuPanel.enabled = false;
		} else {
			MenuPanel.enabled = true;
		}
		
		//MenuObject.gameObject.SetActive(_currentValue > 0.01f);

		if (_down) _holdTime += Time.deltaTime;
		else _holdTime = 0f;

		if (_holdTime > ReturnToMenuTime) {
			_holdTime = 0;
			_down = false;
			Choreography.ReturnToMenu();
		}
		
		if (_holdTime > 0f && _holdTime < ReturnToMenuTime) {
			var requiredTime = Mathf.Lerp(0.2f, 0.02f, Mathf.Pow(Mathf.InverseLerp(0f, ReturnToMenuTime, _holdTime), 0.5f));
			if (Time.time - _lastHapticTime > requiredTime) {
				Debug.Log("buzz");
				HapticAction.Execute(0f, 0f, 160f, 0.95f, SteamVR_Input_Sources.LeftHand);
				HapticAction.Execute(0f, 0f, 160f, 0.95f, SteamVR_Input_Sources.RightHand);
				_lastHapticTime = Time.time;
			}
			//Debug.Log(Time.frameCount % (int)Mathf.Lerp(90, 9, Mathf.InverseLerp(0f, ReturnToMenuTime, _holdTime)));
			//if (Time.frameCount % (int)Mathf.Lerp(90, 9, Mathf.InverseLerp(0f, ReturnToMenuTime, _holdTime)) == 0) {
			//	Haptic.Execute(0f, 0f, 180f, 1f, SteamVR_Input_Sources.LeftHand);
			//	Haptic.Execute(0f, 0f, 180f, 1f, SteamVR_Input_Sources.RightHand);
			//}
		}
	}

	public void SetTargetValue(bool value)
	{
		_targetValue = value;
	}

	private void OnAxisValueChanged(SteamVR_Action_In actionIn)
	{
		
		if (!(actionIn is SteamVR_Action_Boolean)) return;
		var asBoolean = (SteamVR_Action_Boolean) actionIn;
		var pressUp = asBoolean.GetStateUp(Hand);
		var pressDown = asBoolean.GetStateDown(Hand);

		if (pressDown) _down = true;
		else _down = false;
		
		if (!pressUp) return;

		_down = false;
		
		if (!AllowMenuToggle) return;
		_targetValue = !_targetValue;
		if (!_targetValue) return;

		MenuObject.position = new Vector3(HMDObject.position.x, 0f, HMDObject.position.z);
		var forward = HMDObject.forward;
		forward.y = 0f;
		MenuObject.rotation = Quaternion.LookRotation(forward, Vector3.up);
		MenuHeight.localPosition = new Vector3(0f, HMDObject.position.y + HeightOffset, 0f);
	}
}
