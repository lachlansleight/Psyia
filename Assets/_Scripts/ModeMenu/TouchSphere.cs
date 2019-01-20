using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using XRP;

public class TouchSphere : MonoBehaviour
{

	[Header("Objects")]
	public Transform LookTarget;
	public Transform[] Interactors;
	public Color Color = Color.red;
	public ParticleSystem[] ParticleSystems;

	[Header("Animation")]
	public float MaxSpeedMultiplier = 5f;
	public Vector2 Ranges = new Vector2(0.8f, 0.2f);
	public AnimationCurve RangeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	[Header("Properties")]
	public float TouchDistance;
	public UnityEvent OnTouch;
	public float CooldownTime = 1f;
	public int UsageCountRequirement = 0;
	
	private Material _myMaterial;
	private Vector2 _defaultSpeeds;
	private Vector2 _customTime;

	private float _defaultScale;

	private float _touchTime;
	
	public void Awake()
	{
		_myMaterial = GetComponent<Renderer>().material;

		_myMaterial.SetColor("_Color", Color.Lerp(Color, Color.black, 0.1f));
		_myMaterial.SetColor("_Emission", Color);

		_myMaterial.SetFloat("_FrequencyX", _myMaterial.GetFloat("_FrequencyX") * Random.Range(0.9f, 1.1f));
		_myMaterial.SetFloat("_FrequencyY", _myMaterial.GetFloat("_FrequencyY") * Random.Range(0.9f, 1.1f));

		_defaultSpeeds = new Vector2(_myMaterial.GetFloat("_SpeedX"), _myMaterial.GetFloat("_SpeedY"));

		_customTime = Vector2.zero;

		_defaultScale = transform.localScale.x;
	}
	
	public void Update()
	{
		_myMaterial.SetColor("_Color", Color.Lerp(Color, Color.black, 0.1f));
		_myMaterial.SetColor("_Emission", Color);
		
		transform.LookAt(LookTarget.position);
		transform.Rotate(0f, 90f, 0f, Space.Self);

		var proximity = Mathf.Min(
			(transform.position - Interactors[0].position).magnitude,
			(transform.position - Interactors[1].position).magnitude
		);

		var lerpValue = RangeCurve.Evaluate(Mathf.InverseLerp(Ranges.x, Ranges.y, proximity));
		_customTime.x += _defaultSpeeds.x * Mathf.Lerp(1f, MaxSpeedMultiplier, lerpValue) * Time.deltaTime;
		_customTime.y += _defaultSpeeds.y * Mathf.Lerp(1f, MaxSpeedMultiplier, lerpValue) * Time.deltaTime;
		
		_myMaterial.SetFloat("_CustomTimeX", _customTime.x);
		_myMaterial.SetFloat("_CustomTimeY", _customTime.y);

		if (proximity < TouchDistance && (Time.time - _touchTime) > CooldownTime) {
			_touchTime = Time.time;
			OnTouch.Invoke();
		}
	}

	public void LerpToScale(float targetScalePercentage, float duration, bool disableAtEnd = false)
	{
		StartCoroutine(LerpToScaleRoutine(targetScalePercentage, duration, disableAtEnd));
	}

	public IEnumerator LerpToScaleRoutine(float targetScalePercentage, float duration, bool disableAtEnd = false)
	{
		var from = transform.localScale.x;
		var to = targetScalePercentage * _defaultScale;
		
		for (var i = 0f; i < 1f; i += Time.deltaTime / duration) {
			transform.localScale = Vector3.one * Mathf.Lerp(from, to, LerpCubic(i));
			foreach (var ps in ParticleSystems) {
				var em = ps.emission;
				em.rateOverTimeMultiplier = (transform.localScale.x / _defaultScale);
			}
			yield return null;
		}

		transform.localScale = Vector3.one * to;
		foreach (var ps in ParticleSystems) {
			var em = ps.emission;
			em.rateOverTimeMultiplier = (transform.localScale.x / _defaultScale);
		}

		if (disableAtEnd) gameObject.SetActive(false);
	}

	private float LerpCubic(float t)
	{
		if (t < 0.5f) return 4f * t * t * t;
		t--;
		return 4f * t * t * t + 1f;
	}
}
