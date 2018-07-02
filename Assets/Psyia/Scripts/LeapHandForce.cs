using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

[RequireComponent(typeof(ForceSource))]
public class LeapHandForce : MonoBehaviour {

	public HandModelBase TargetHand;
	public float MinFistThreshold = 0.1f;
	public float MaxFistThreshold = 0.9f;

	private ForceSource _Source;
	private ForceSource Source {
		get {
			if(_Source == null) {
				_Source = GetComponent<ForceSource>();
			}
			return _Source;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!TargetHand.IsTracked) {
			Source.StrengthModifier = 0f;
			return;
		}
		Hand hand = TargetHand.GetLeapHand();
		if(hand.Confidence == 0f) {
			Source.StrengthModifier = 0f;
			return;
		}
		transform.position = hand.GetPalmPose().position;
		transform.rotation = Quaternion.LookRotation(hand.PalmNormal.ToVector3(), Vector3.up);
		Source.StrengthModifier = Mathf.Lerp(1f, 0f, Mathf.InverseLerp(MinFistThreshold, MaxFistThreshold, hand.GetFistStrength()));
	}
}
