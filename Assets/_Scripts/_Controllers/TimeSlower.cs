using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TimeSlower : MonoBehaviour
{

	[SerializeField]
	private float _timeScaleMultiplier;

	public float TimeScaleMultiplier
	{
		get { return _timeScaleMultiplier; }
		set
		{
			Debug.Log("set");
			_timeScaleMultiplier = value;
		}
	}
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
	
	private void HandleButton(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources hand, bool newState)
	{
		Debug.Log($"{hand} down is {actionIn.GetStateDown(hand)}");
		if (actionIn.GetStateDown(hand)) {
			if (hand == SteamVR_Input_Sources.LeftHand) _leftDown = true;
			else if (hand == SteamVR_Input_Sources.RightHand) _rightDown = true;
		} else if (actionIn.GetStateUp(hand)) {
			if (hand == SteamVR_Input_Sources.LeftHand) _leftDown = false;
			else if (hand == SteamVR_Input_Sources.RightHand) _rightDown = false;
		}
	}
}
