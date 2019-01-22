using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TimeSlower : MonoBehaviour
{

	public float TimeScaleMultiplier;
	public float TimeScale;
	public PsyiaMusic Music;
	public bool SlowsWithTime;
	public UpdateTime TimeUpdater;

	public float OneDownSpeed = 0.5f;
	public float BothDownSpeed = 0.05f;

	public SteamVR_Action_Boolean SlowButton;

	private bool _leftDown;
	private bool _rightDown;

	public void OnEnable()
	{
		SlowButton.AddOnChangeListener(HandleButton, SteamVR_Input_Sources.LeftHand);
		SlowButton.AddOnChangeListener(HandleButton, SteamVR_Input_Sources.RightHand);
	}

	public void OnDisable()
	{
		SlowButton.RemoveOnChangeListener(HandleButton, SteamVR_Input_Sources.LeftHand);
		SlowButton.RemoveOnChangeListener(HandleButton, SteamVR_Input_Sources.RightHand);
	}

	public void Update()
	{
		if (_leftDown == false && _rightDown == false) TimeScale = 1f;
		else if (_leftDown == true && _rightDown == true) TimeScale = BothDownSpeed;
		else TimeScale = OneDownSpeed;

		if (SlowsWithTime) {
			Music.SetPitch(TimeScale * TimeScaleMultiplier);
		}

		TimeUpdater.TimeScale = TimeScale * TimeScaleMultiplier;
	}
	
	private void HandleButton(SteamVR_Action_In actionIn)
	{
		if (!(actionIn is SteamVR_Action_Boolean)) return;
		var asBoolean = (SteamVR_Action_Boolean) actionIn;
		
		if (asBoolean.GetStateDown(SteamVR_Input_Sources.LeftHand)) {
			_leftDown = true;
		} else if (asBoolean.GetStateUp(SteamVR_Input_Sources.LeftHand)) {
			_leftDown = false;
		}
		
		if (asBoolean.GetStateDown(SteamVR_Input_Sources.RightHand)) {
			_rightDown = true;
		} else if (asBoolean.GetStateUp(SteamVR_Input_Sources.RightHand)) {
			_rightDown = false;
		}
	}
}
