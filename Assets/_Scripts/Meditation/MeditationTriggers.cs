using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeditationTriggers : MonoBehaviour
{

	[Header("Required")]
	public Transform[] Controllers;
	[Space(10)]
	public Transform LeftHolder;
	public Transform RightHolder;

	[Space(10)]
	public Renderer LeftIndicator;
	public Renderer RightIndicator;

	[Header("Properties")]
	public float DistanceRequirement;
	public float Acceleration = 0.2f;
	public float MaxVelocity = 0.2f;
	[Range(0f, 1f)] public float GlobalSizeMultiplier = 1f;

	[Header("Status")]
	public bool Triggered;
	public Transform LeftActive;
	public Transform RightActive;

	public UnityEvent OnTrigger;

	private float _leftValue;
	private float _rightValue;
	private float _leftVelocity;
	private float _rightVelocity;
	private float _leftTargetValue;
	private float _rightTargetValue;
	private float _leftSum;
	private float _rightSum;

	public void Update()
	{
		if (GlobalSizeMultiplier <= 0f) {
			LeftIndicator.transform.localScale = Vector3.zero;
			RightIndicator.transform.localScale = Vector3.zero;
			LeftHolder.localScale = Vector3.zero;
			RightHolder.localScale = Vector3.zero;
			return;
		}
		
		if (LeftActive == null) {
			if ((LeftHolder.position - Controllers[0].position).sqrMagnitude < DistanceRequirement * DistanceRequirement) {
				LeftActive = Controllers[0];
				_leftTargetValue = 1f;
			}
			if ((LeftHolder.position - Controllers[1].position).sqrMagnitude < DistanceRequirement * DistanceRequirement) {
				LeftActive = Controllers[1];
				_leftTargetValue = 1f;
			}
		} else {
			if ((LeftHolder.position - LeftActive.position).sqrMagnitude > DistanceRequirement * DistanceRequirement) {
				LeftActive = null;
				_leftTargetValue = 0f;
				Triggered = false;
				_leftSum = 0f;
			}
		}
		
		if (RightActive == null) {
			if ((RightHolder.position - Controllers[0].position).sqrMagnitude < DistanceRequirement * DistanceRequirement) {
				RightActive = Controllers[0];
				_rightTargetValue = 1f;
			}
			if ((RightHolder.position - Controllers[1].position).sqrMagnitude < DistanceRequirement * DistanceRequirement) {
				RightActive = Controllers[1];
				_rightTargetValue = 1f;
			}
		} else {
			if ((RightHolder.position - RightActive.position).sqrMagnitude > DistanceRequirement * DistanceRequirement) {
				RightActive = null;
				_rightTargetValue = 0f;
				Triggered = false;
				_rightSum = 0f;
			}
		}

		if (_leftValue < _leftTargetValue) _leftVelocity += Acceleration;
		else _leftVelocity -= Acceleration;

		if (_rightValue < _rightTargetValue) _rightVelocity += Acceleration;
		else _rightVelocity -= Acceleration;

		_leftVelocity = Mathf.Clamp(_leftVelocity, -MaxVelocity, MaxVelocity);
		_rightVelocity = Mathf.Clamp(_rightVelocity, -MaxVelocity, MaxVelocity);

		_leftValue += Time.deltaTime * _leftVelocity;
		_rightValue += Time.deltaTime * _rightVelocity;
		
		LeftIndicator.material.color = new Color(1f, 1f, 1f, Mathf.Clamp01(_leftValue));
		RightIndicator.material.color = new Color(1f, 1f, 1f, Mathf.Clamp01(_rightValue));
		LeftIndicator.transform.localScale = new Vector3(_leftValue * 0.2f, _leftValue * 0.2f, 1f) * GlobalSizeMultiplier;
		RightIndicator.transform.localScale = new Vector3(_rightValue * 0.2f, _rightValue * 0.2f, 1f) * GlobalSizeMultiplier;
		
		LeftHolder.localScale = new Vector3(0.2f, 0.2f, 1f) * GlobalSizeMultiplier;
		RightHolder.localScale = new Vector3(0.2f, 0.2f, 1f) * GlobalSizeMultiplier;

		if (_leftTargetValue >= 1f) _leftSum += Time.deltaTime;
		if (_rightTargetValue >= 1f) _rightSum += Time.deltaTime;

		if (_leftSum < 1f || _rightSum < 1f || Triggered) return;
		
		Triggered = true;
		OnTrigger?.Invoke();
	}
}
