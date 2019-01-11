using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MenuToggler : MonoBehaviour
{
	public SteamVR_Action_Boolean ToggleMenuAction;
	public SteamVR_Input_Sources Hand;

	[Range(0f, 1f)] public float LerpSpeed = 0.2f;
	public Transform MenuObject;
	public Transform MenuHeight;
	public Transform HMDObject;

	private bool _targetValue;
	private float _currentValue;
	
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
		
		MenuObject.gameObject.SetActive(_currentValue > 0.01f);
	}

	private void OnAxisValueChanged(SteamVR_Action_In actionIn)
	{
		if (!(actionIn is SteamVR_Action_Boolean)) return;
		var asBoolean = (SteamVR_Action_Boolean) actionIn;
		var pressDown = asBoolean.GetStateDown(Hand);
		
		if (!pressDown) return;
		_targetValue = !_targetValue;
		if (!_targetValue) return;
		
		MenuObject.position = new Vector3(HMDObject.position.x, 0f, HMDObject.position.z);
		var forward = HMDObject.forward;
		forward.y = 0f;
		MenuObject.rotation = Quaternion.LookRotation(forward, Vector3.up); 
		MenuHeight.localPosition = new Vector3(0f, HMDObject.position.y - 0.54f, 0f);
	}
}
