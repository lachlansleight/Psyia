﻿using System.Collections;
using System.Collections.Generic;
using Psyia;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(PsyiaForce))]
public class ControllerForce : MonoBehaviour
{

	public SteamVR_Action_Single ApplyForceAction;
	public SteamVR_Input_Sources Hand;
	
	[Range(0f, 1f)] public float Value;

	private PsyiaForce _psyiaForce;

	private void Awake()
	{
		_psyiaForce = GetComponent<PsyiaForce>();
	}
	
	public void OnEnable ()
	{
		ApplyForceAction.AddOnChangeListener(OnAxisValueChanged, Hand);
	}

	public void OnDisable()
	{
		ApplyForceAction.RemoveOnChangeListener(OnAxisValueChanged, Hand);
	}

	private void OnAxisValueChanged(SteamVR_Action_In actionIn)
	{
		if (!(actionIn is SteamVR_Action_Single)) return;
		var asSingle = (SteamVR_Action_Single) actionIn;
		Value = asSingle.GetAxis(Hand);
		_psyiaForce.StrengthMultiplier = Value;
	}
}