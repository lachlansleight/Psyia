using System.Collections;
using System.Collections.Generic;
using Psyia;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(PsyiaForce))]
public class ControllerForce : MonoBehaviour
{

	public SteamVR_Action_Pose PoseAction;
	public SteamVR_Action_Single ApplyForceAction;
	public SteamVR_Input_Sources Hand;
	
	[Range(0f, 1f)] public float Value;

	private PsyiaForce _psyiaForce;

	private void Awake()
	{
		_psyiaForce = GetComponent<PsyiaForce>();
	}
	
	// Use this for initialization
	public void OnEnable ()
	{
		ApplyForceAction.AddOnChangeListener(OnAxisValueChanged, Hand);
		PoseAction.AddOnChangeListener(OnPoseChanged, Hand);
	}

	public void OnDisable()
	{
		ApplyForceAction.RemoveOnChangeListener(OnAxisValueChanged, Hand);
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
		Value = asSingle.GetAxis(Hand);
		_psyiaForce.StrengthMultiplier = Value;
	}

	private void OnPoseChanged(SteamVR_Action_In actionIn)
	{
		if (!(actionIn is SteamVR_Action_Pose)) return;
		var asPose = (SteamVR_Action_Pose) actionIn;
		transform.position = asPose.GetLocalPosition(Hand);
		transform.rotation = Quaternion.Inverse(asPose.GetLocalRotation(Hand));
	}
}
