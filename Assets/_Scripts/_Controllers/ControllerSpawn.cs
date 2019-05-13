using System.Collections;
using System.Collections.Generic;
using Psyia;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(PsyiaEmitter))]
public class ControllerSpawn : MonoBehaviour {

	public SteamVR_Action_Single SpawnParticlesAction;
	public SteamVR_Input_Sources Hand;

	public AnimationCurve SpawnMultiplier = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	
	[Range(0f, 1f)] public float Value;

	private PsyiaEmitter _psyiaEmitter;

	private void Awake()
	{
		_psyiaEmitter = GetComponent<PsyiaEmitter>();
		_psyiaEmitter.SetEmission(false);
	}
	
	public void OnEnable ()
	{
		SpawnParticlesAction.AddOnChangeListener(OnAxisValueChanged, Hand);
	}

	public void OnDisable()
	{
		SpawnParticlesAction.RemoveOnChangeListener(OnAxisValueChanged, Hand);
	}

	private void OnAxisValueChanged(SteamVR_Action_Single actionIn, SteamVR_Input_Sources hand, float a, float b)
	{
		//Value = SpawnMultiplier.Evaluate(actionIn.GetState(Hand) ? 1f : 0f);
		Value = SpawnMultiplier.Evaluate(actionIn.GetAxis(Hand));
		_psyiaEmitter.EmissionMultiplier = Value;
		_psyiaEmitter.SetEmission(Value > 0f);
		//_psyiaEmitter.Settings.MinSpawnVelocity = MinSpawnVelocity.Evaluate(Value);
		//_psyiaEmitter.Settings.MaxSpawnVelocity = MaxSpawnVelocity.Evaluate(Value);
	}
}
