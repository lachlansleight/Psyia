using System.Collections;
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
		if (ApplyForceAction == null) return;
		ApplyForceAction.AddOnChangeListener(OnAxisValueChanged, Hand);
	}

	public void OnDisable()
	{
		if (ApplyForceAction == null) return;
		ApplyForceAction.RemoveOnChangeListener(OnAxisValueChanged, Hand);
	}

	public void Update()
	{
		_psyiaForce.StrengthMultiplier = Value;
	}

	private void OnAxisValueChanged(SteamVR_Action_In actionIn)
	{
		if (!(actionIn is SteamVR_Action_Single)) return;
		var asSingle = (SteamVR_Action_Single) actionIn;
		Value = asSingle.GetAxis(Hand);
	}
}
