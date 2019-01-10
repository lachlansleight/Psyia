using System.Collections;
using System.Collections.Generic;
using Psyia;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(PsyiaEmitter))]
public class ControllerSpawn : MonoBehaviour {

	public SteamVR_Action_Boolean SpawnParticlesAction;
	public SteamVR_Input_Sources Hand;

	public AnimationCurve SpawnMultiplier = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	public AnimationCurve MinSpawnVelocity = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	public AnimationCurve MaxSpawnVelocity = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	
	[Range(0f, 1f)] public float Value;

	private PsyiaEmitter _psyiaEmitter;

	private void Awake()
	{
		_psyiaEmitter = GetComponent<PsyiaEmitter>();
	}
	
	public void OnEnable ()
	{
		SpawnParticlesAction.AddOnChangeListener(OnAxisValueChanged, Hand);
	}

	public void OnDisable()
	{
		SpawnParticlesAction.RemoveOnChangeListener(OnAxisValueChanged, Hand);
	}

	private void OnAxisValueChanged(SteamVR_Action_In actionIn)
	{
		if (!(actionIn is SteamVR_Action_Boolean)) return;
		var asBoolean = (SteamVR_Action_Boolean) actionIn;
		Value = SpawnMultiplier.Evaluate(asBoolean.GetState(Hand) ? 1f : 0f);
		_psyiaEmitter.EmissionMultiplier = Value;
		_psyiaEmitter.Settings.MinSpawnVelocity = MinSpawnVelocity.Evaluate(Value);
		_psyiaEmitter.Settings.MaxSpawnVelocity = MaxSpawnVelocity.Evaluate(Value);
	}
}
