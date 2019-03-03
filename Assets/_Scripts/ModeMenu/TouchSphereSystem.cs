using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TouchSphereSystem : TouchSphere
{
	[Header("Required")]
	public Object CubePrefab;
	
	[Header("Optional")]
	public Transform LookTarget;
	[Range(0f, 1f)] public float LookLerpRate = 0.1f;
	[Space(10)]
	public Transform[] Affectors;
	
	[Header("Properties")]
	public int CubeCount = 100;
	public Vector3 SpeedMask = new Vector3(0f, 1f, 1f);
	public float StartRotationAmount = 10f;
	[Space(10)]
	public float BreathingRate = 0.4f;
	public float BreathingAmplitude = 0.1f;
	[Space(5)]
	public float MinDistance = 1.5f;
	public float MaxDistance = 0.5f;
	[Space(5)]
	public float MinSpeed = 0f;
	public float MaxSpeed = 3f;
	public float ReturnSpeed = 1f;
	[Space(10)]
	public float RotationSpeedCenter = 0f;
	public float RotationSpeedAmplitude = 100f;
	[Range(0.01f, 5f)] public float RotationSpeedPeriod = 0.5f;
	[Space(10)]
	[Range(0f, 1f)] public float HueCenter = 0.5f;
	[Range(0f, 1f)] public float HueSpread = 0.2f;
	[Range(0f, 1f)] public float Saturation = 1f;
	[Range(0f, 1f)] public float Value = 1f;
	[Space(10)]
	[Range(0f, 1f)] public float ScaleVariation = 0f;
	[Range(-1f, 2f)] public float ScaleVariationOffset = 0f;
	[Range(0f, 1f)] public float ScaleVariationWidth = 0.1f;
	public Vector3 ScaleVariationMask = new Vector3(1f, 0f, 1f);
	public Vector3 Scale = new Vector3(1f, 0.1f, 1f);

	[Header("Events")]
	public UnityEvent OnTrigger;
	
	private Transform[] _cubes;
	private Renderer[] _cubeRenderers;
	private Vector3[] _rotationT;
	private float _breathT;
	private float _proximityIncrement;
	private bool _dying;
	
	public void OnEnable()
	{
		DestroyChildren();
		var cubeList = new List<Transform>();
		var rendererList = new List<Renderer>();
		for (var i = 0; i < CubeCount; i++) {
			var newCube = (Instantiate(CubePrefab) as GameObject).transform;
			newCube.parent = transform;
			newCube.localPosition = new Vector3(0f, Mathf.Lerp(-0.5f, 0.5f, Mathf.InverseLerp(0, CubeCount - 1, i)), 0f);
			cubeList.Add(newCube);
			rendererList.Add(newCube.GetComponent<Renderer>());
		}

		_cubes = cubeList.ToArray();
		_cubeRenderers = rendererList.ToArray();
		_rotationT = new Vector3[_cubes.Length];
		Increment(StartRotationAmount);
		_proximityIncrement = StartRotationAmount;
	}

	public void Update()
	{
		if (_dying) return;
		
		if (Input.GetKeyDown(KeyCode.F12) && gameObject.name == "WhiteTouchSphere") {
			OnTrigger.Invoke();
		}
		
		var breathingFactor = 1f;
		if (Affectors != null && Affectors.Length > 0) {
			var distances = new float[Affectors.Length];
			var maxDistance = -float.MaxValue;
			for (var i = 0; i < Affectors.Length; i++) {
				if (Affectors[i] == null) {
					distances[i] = 0f;
					continue;
				}

				var rawDistance = (transform.position - Affectors[i].position).magnitude;
				distances[i] = (rawDistance - MinDistance) / (MaxDistance - MinDistance);

				if (distances[i] > maxDistance) maxDistance = distances[i];
			}

			breathingFactor = 1f - maxDistance;

			if (maxDistance > 0f) {
				//increase increment, clamping to max increment
				var increment = Mathf.Lerp(MinSpeed, MaxSpeed, maxDistance * maxDistance);
				
				Increment(increment);
				_proximityIncrement += increment;
			} else {
				var returnSpeed = Mathf.Lerp(0f, ReturnSpeed, Mathf.InverseLerp(0f, -0.5f, maxDistance));
				if (_proximityIncrement > StartRotationAmount && MaxSpeed > 0f) {
					var pre = _proximityIncrement;
					_proximityIncrement -= returnSpeed;
					var post = _proximityIncrement;
					Increment(post - pre);
				} else if (_proximityIncrement < StartRotationAmount && MaxSpeed < 0f) {
					var pre = _proximityIncrement;
					_proximityIncrement += returnSpeed;
					var post = _proximityIncrement;
					Increment(post - pre);
				}
			}

			if (maxDistance >= 1f) {
				OnTrigger?.Invoke();
				Die();
			}
		}

		//breathe in and out when not proximal
		_breathT += Time.deltaTime / BreathingRate;
		var breathSpeed = breathingFactor * BreathingAmplitude * Mathf.Sin(_breathT * Mathf.PI * 2f);
		Increment(breathSpeed);

		//look at camera
		if (LookTarget != null) {
			transform.rotation = Quaternion.Lerp(
				transform.rotation,
				Quaternion.LookRotation(LookTarget.position - transform.position, Vector3.up),
				LookLerpRate
			);
		}
	}

	public void Die()
	{
		_dying = true;
		StartCoroutine(DieRoutine());
	}

	private IEnumerator DieRoutine()
	{
		var startScaleVariationOffset = ScaleVariationOffset;
		var endScaleVariationOffset = 1f + ScaleVariationWidth;
		var startValue = Value;
		var endValue = 0f;
		for (var i = 0f; i < 1f; i += Time.deltaTime / 1f) {
			ScaleVariationOffset = Mathf.Lerp(startScaleVariationOffset, endScaleVariationOffset, i);
			Value = Mathf.Lerp(startValue, endValue, i);
			Increment(MaxSpeed);
			yield return null;
		}

		DestroyChildren();
		ScaleVariationOffset = startScaleVariationOffset;
		Value = startValue;
	}

	private void DestroyChildren()
	{
		for (var i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
	}

	private void Increment(float multiplier)
	{
		for (var i = 0; i < _cubes.Length; i++) {
			if (_cubes[i] == null) continue;
			var iF = i / (_cubes.Length - 1f);
			var iT = Mathf.PI * 2f * iF;

			var speed = Vector3.one * (RotationSpeedCenter + RotationSpeedAmplitude * Mathf.Sin(iT / RotationSpeedPeriod));
			speed *= Time.deltaTime;
			speed *= multiplier;
			speed.Scale(SpeedMask);
			_rotationT[i] += speed;
			
			//_cubes[i].localPosition = new Vector3(0f, Mathf.Lerp(-0.5f, 0.5f, Mathf.InverseLerp(0, CubeCount - 1, i)), 0f);
			_cubes[i].localEulerAngles = _rotationT[i];
			_cubes[i].localPosition = Vector3.zero;
			var scaleMultiplier = Mathf.InverseLerp(
				ScaleVariationOffset - ScaleVariationWidth,
				ScaleVariationOffset + ScaleVariationWidth, 
				iF
			);
			scaleMultiplier = Mathf.Lerp(1f, scaleMultiplier, ScaleVariation);
			var scale = new Vector3(
				Scale.x * Mathf.Lerp(1f, scaleMultiplier, ScaleVariationMask.x),
				Scale.y * Mathf.Lerp(1f, scaleMultiplier, ScaleVariationMask.y),
				Scale.z * Mathf.Lerp(1f, scaleMultiplier, ScaleVariationMask.z)
			);
			scale.Scale(Scale);
			
			_cubes[i].localScale = scale;

			var hue = HueCenter + HueSpread * 2f * (iF - 0.5f);
			while (hue < 0f) hue += 1f;
			hue %= 1f;
			_cubeRenderers[i].material.color = Color.HSVToRGB(hue, Saturation, Value);
		}
	}
	
	[ContextMenu("Reset Rotations")]
	public void ResetRotations()
	{
		foreach (var c in _cubes) c.localRotation = Quaternion.identity;

		Increment(StartRotationAmount);
		_proximityIncrement = StartRotationAmount;
	}
}
