using System.Collections;
using System.Collections.Generic;
using Psyia;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(PsyiaEmitter))]
public class ControllerSpawn : MonoBehaviour {

	public SteamVR_Action_Pose PoseAction;
	public SteamVR_Action_Single SpawnParticlesAction;
	public SteamVR_Input_Sources Hand;
	
	[Range(0f, 1f)] public float Value;

	private PsyiaEmitter _psyiaEmitter;

	private void Awake()
	{
		_psyiaEmitter = GetComponent<PsyiaEmitter>();
	}
	
	// Use this for initialization
	public void OnEnable ()
	{
		SpawnParticlesAction.AddOnChangeListener(OnAxisValueChanged, Hand);
		PoseAction.AddOnChangeListener(OnPoseChanged, Hand);
	}

	public void OnDisable()
	{
		SpawnParticlesAction.RemoveOnChangeListener(OnAxisValueChanged, Hand);
	}
	
	// Update is called once per frame
	public void Update ()
	{
		//LeftValue = SteamVR_Input._default.inActions.ApplyForce.GetAxis(SteamVR_Input_Sources.LeftHand);
	}

	private void OnAxisValueChanged(SteamVR_Action_In actionIn)
	{
		if (!(actionIn is SteamVR_Action_Single)) return;
		var asSingle = (SteamVR_Action_Single) actionIn;
		Value = Mathf.InverseLerp(0.2f, 1f, asSingle.GetAxis(Hand));
		_psyiaEmitter.EmissionMultiplier = Value;
		_psyiaEmitter.Settings.MinSpawnVelocity = Mathf.Lerp(0f, 0.005f, Value);
		_psyiaEmitter.Settings.MaxSpawnVelocity = Mathf.Lerp(0f, 0.001f, Value);
	}

	private void OnPoseChanged(SteamVR_Action_In actionIn)
	{
		if (!(actionIn is SteamVR_Action_Pose)) return;
		var asPose = (SteamVR_Action_Pose) actionIn;
		transform.position = asPose.GetLocalPosition(Hand);
		transform.rotation = Quaternion.Inverse(asPose.GetLocalRotation(Hand));
	}
}
