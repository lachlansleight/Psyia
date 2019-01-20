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
		PoseAction.AddOnChangeListener(OnPoseChanged, Hand);
	}

	public void OnDisable()
	{
		PoseAction.RemoveOnChangeListener(OnPoseChanged, Hand);
	}

	private void OnPoseChanged(SteamVR_Action_In actionIn)
	{
		if (!(actionIn is SteamVR_Action_Pose)) return;
		var asPose = (SteamVR_Action_Pose) actionIn;
		transform.position = asPose.GetLocalPosition(Hand);
		transform.rotation = asPose.GetLocalRotation(Hand);
	}
}
