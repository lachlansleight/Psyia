using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using Psyia;
using UnityEngine;

public class MeditationCrystal : MonoBehaviour
{

	[Header("Required")]
	public Meditation Meditation;
	public AudioSource AudioSource;
	public PsyiaForce Force;
	public PsyiaForce SineForce;
	public Material TemplateMaterial;

	[Space(10)]
	public Transform AttractorParent;
	public Transform Spinner;
	public Transform ForceSphere;
	public Renderer Pole;
	public Renderer SpawnIndicator;
	
	[Header("Properties")]
	public float IntervalTime;
	public Vector2 SpinSpeeds = new Vector2(10f, 4000f);
	public Vector2 SphereSizes = new Vector2(0.005f, 0.04f);

	[Header("Status")]
	public float Age;
	
	private Material _material;
	private bool _initialized;
	private float _rotation;
	private float _targetHeight;
	private float _growTime;
	private float _minMultiplier;
	private float _growProgress;
	
	public void Awake()
	{
		
	}

	public void Update()
	{
		if(!_initialized) return;

		var lerpFactor = Mathf.InverseLerp(_minMultiplier, 1f, Force.StrengthMultiplier);
		
		_rotation += Mathf.Lerp(SpinSpeeds.x, SpinSpeeds.y, lerpFactor) * Time.deltaTime;
		Spinner.localRotation = Quaternion.Euler(new Vector3(_rotation, 90f, 90f));

		ForceSphere.localScale = Vector3.one * Mathf.Lerp(SphereSizes.x, SphereSizes.y, lerpFactor);
		_material.SetColor("_EmissionColor", Color.Lerp(Color.black, Color.white, lerpFactor));
		
		Force.Strength = Meditation.MaxCharge * 
		                 Random.Range(1f - Meditation.ChargeVariability, 1f + Meditation.ChargeVariability) * 
		                 _growProgress;
		SineForce.Strength = Force.Strength * Meditation.SineForceMultiplier;

		Age += Time.deltaTime;
	}

	public void Initialize(Meditation meditation, AudioClip clip, float intervalTime, float height, float growTime = 30f)
	{
		Meditation = meditation;
		IntervalTime = intervalTime;
		AudioSource.clip = clip;
		_growTime = growTime;
		_targetHeight = height;

		_material = Instantiate(TemplateMaterial);
		var mats = Spinner.GetComponent<Renderer>().sharedMaterials;
		mats[1] = _material;
		Spinner.GetComponent<Renderer>().sharedMaterials = mats;
		ForceSphere.GetComponent<Renderer>().sharedMaterial = _material;

		_minMultiplier = Meditation.MinCharge / Meditation.MaxCharge;
		Force.StrengthMultiplier = _minMultiplier;
		SineForce.StrengthMultiplier = Force.StrengthMultiplier - _minMultiplier;

		StartCoroutine(GrowRoutine());

		_initialized = true;
	}

	private IEnumerator GrowRoutine()
	{
		StartCoroutine(PulseSpawnRoutine());
		
		for (var i = 0f; i < 1f; i += Time.deltaTime / _growTime) {
			var iSmooth = LerpCubic(i);
			AttractorParent.localPosition = new Vector3(0f, Mathf.Lerp(0f, _targetHeight, iSmooth));
			AttractorParent.localScale = Vector3.one * Mathf.Lerp(0f, 1f, iSmooth);
			
			var v = Pole.transform.localScale;
			v.y = Mathf.Lerp(0f, 0.5f * _targetHeight, iSmooth);
			Pole.transform.localScale = v;
			Pole.transform.localPosition = new Vector3(0f, Mathf.Lerp(0f, 0.5f * _targetHeight, iSmooth), 0f);
			Pole.material.color = Color.Lerp(new Color(0.3f, 0.3f, 0.3f), new Color(0.05f, 0.05f, 0.05f), i);

			_growProgress = i;
			yield return null;
		}

		_growProgress = 1f;

		StartCoroutine(ToneRoutine());
	}

	private IEnumerator PulseSpawnRoutine()
	{
		SpawnIndicator.gameObject.SetActive(true);
		
		var startColor = new Color(0.1f, 0.1f, 0.1f, 0f);
		var startScale = Vector3.zero;
		var endColor = new Color(0.1f, 0.1f, 0.1f, 1f);
		var endScale = new Vector3(0.15f, 0.15f, 1f);
		for (var i = 0f; i < 1f; i += Time.deltaTime / 2f) {
			SpawnIndicator.material.color = Color.Lerp(startColor, endColor, LerpCubic(i));
			SpawnIndicator.transform.localScale = Vector3.Lerp(startScale, endScale, LerpCubic(i));
			yield return null;
		}
		for (var i = 1f; i > 0f; i -= Time.deltaTime / 2f) {
			SpawnIndicator.material.color = Color.Lerp(startColor, endColor, LerpCubic(i));
			SpawnIndicator.transform.localScale = Vector3.Lerp(startScale, endScale, LerpCubic(i));
			yield return null;
		}

		SpawnIndicator.gameObject.SetActive(false);
	}

	private IEnumerator ToneRoutine() 
	{
		yield return new WaitForSeconds(IntervalTime + Meditation.DecayTime - Meditation.LeadInTime);

		for(float i = 0; i < 1f; i += Time.deltaTime / Meditation.LeadInTime) {
			Force.StrengthMultiplier = Mathf.Lerp(_minMultiplier, 1f, LerpCubic(i));
			SineForce.StrengthMultiplier = Force.StrengthMultiplier - _minMultiplier;
			yield return null;
		}
		
		AudioSource.Play();

		for(float i = 0; i < 1f; i += Time.deltaTime / Meditation.DecayTime) {
			Force.StrengthMultiplier = Mathf.Lerp(1f, _minMultiplier, LerpQuadraticOut(i));
			SineForce.StrengthMultiplier = Force.StrengthMultiplier - _minMultiplier;
			yield return null;
		}

		StartCoroutine(ToneRoutine());
	}
	
	private float LerpQuadraticOut(float t) 
	{
		return -1f * ((t - 1f) * (t - 1f)) + 1f;
	}

	private float LerpCubic(float t) 
	{
		if(t < 0.5f) return 4f * t * t * t;
		t--;
		return 4f * t * t * t + 1f;
	}
}
