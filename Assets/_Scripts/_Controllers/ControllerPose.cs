using System.Collections;
using System.Collections.Generic;
using Psyia;
using UnityEngine;
using Valve.VR;

public class ControllerPose : MonoBehaviour
{

	public SteamVR_Action_Pose PoseAction;
	public SteamVR_Input_Sources Hand;
	
	public void OnEnable ()
	{
		PoseAction.AddOnChangeListener(Hand, OnPoseChanged);
	}

	public void OnDisable()
	{
		PoseAction.RemoveOnChangeListener(Hand, OnPoseChanged);
	}

	private void OnPoseChanged(SteamVR_Action_Pose actionIn, SteamVR_Input_Sources hand)
	{
		transform.position = actionIn.GetLocalPosition(hand);
		transform.rotation = actionIn.GetLocalRotation(hand);
	}
}
