using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerHaptics : MonoBehaviour
{

	public SteamVR_Action_Vibration HapticAction;
	public SteamVR_Input_Sources Hand;
	public ControllerForce Force;

	public bool Invert;
	
	[Range(0f, 360f)] public float Frequency;
	public Vector2 DistanceAmplitude = new Vector2(0.2f, 0.7f);
	public Vector2 FrequencyAmplitude = new Vector2(0f, 0.3f);

	public Vector2 PulseDistance = new Vector2(0.05f, 0.01f);
	public Vector2 PulseFrequency = new Vector2(2f, 4f);

	private Vector3 _lastPulsePosition;
	private float _lastPulseTime;

	public void Awake()
	{
		_lastPulsePosition = transform.position;
		_lastPulseTime = Time.time;
	}

	public void Update()
	{
		var distance = (transform.position - _lastPulsePosition).magnitude;
		var time = Time.time - _lastPulseTime;

		var value = Force.Value;
		if (Invert) value *= -1;
		
		var pulseDistance = Mathf.Lerp(PulseDistance.x, PulseDistance.y, value);
		var distanceAmplitude = Mathf.Lerp(DistanceAmplitude.x, DistanceAmplitude.y, value);
		
		if (distance > pulseDistance) {
			HapticAction.Execute(0f, 0f, Frequency, distanceAmplitude, Hand);
			_lastPulsePosition = transform.position;
		}

		var pulseFrequency = 1f / Mathf.Lerp(PulseFrequency.x, PulseFrequency.y, value);
		var frequencyAmplitude = Mathf.Lerp(FrequencyAmplitude.x, FrequencyAmplitude.y, value);

		if (time > pulseFrequency) {
			HapticAction.Execute(0f, 0f, Frequency, frequencyAmplitude, Hand);
			_lastPulseTime = Time.time;
		}
	}
}
