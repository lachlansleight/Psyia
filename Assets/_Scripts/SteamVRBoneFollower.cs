using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SteamVRBoneFollower : MonoBehaviour
{

	public Hand TargetHand;
	public SteamVR_Skeleton_JointIndexEnum TargetBone;
	
	public void Start()
	{
		
	}

	public void Update()
	{
		transform.position = TargetHand.GetBonePosition(TargetBone);
	}
}
