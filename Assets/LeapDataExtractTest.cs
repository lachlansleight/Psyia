using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;


public class LeapDataExtractTest : MonoBehaviour {

	public HandModelBase TargetHand;

	public Transform DebugPalm;
	public Transform[] DebugFingers;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Leap.Hand hand = TargetHand.GetLeapHand();
		DebugPalm.position = hand.GetPalmPose().position;
		DebugPalm.rotation = hand.GetPalmPose().rotation;
		DebugPalm.localScale = Vector3.one * Mathf.Lerp(0.1f, 0.01f, hand.GetFistStrength());

		List<Leap.Finger> fingers = hand.Fingers;
		for(int i = 0; i < fingers.Count; i++) {
			DebugFingers[i].position = fingers[i].TipPosition.ToVector3();
			DebugFingers[i].rotation = Quaternion.LookRotation(fingers[i].Direction.ToVector3(), Vector3.up);
		}
	}
}
