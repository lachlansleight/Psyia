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
	public Material TemplateMaterial;

	[Space(10)]
	public Transform AttractorParent;
	public Transform Spinner;
	public Transform ForceSphere;
	public Transform Pole;
	
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
	
	public void Awake()
	{
		
	}

	public void Update()
	{
		if(!_initialized) return;

		var lerpFactor = Mathf.InverseLerp(Meditation.MinCharge, Meditation.MaxCharge, Force.Strength);
		
		_rotation += Mathf.Lerp(SpinSpeeds.x, SpinSpeeds.y, lerpFactor) * Time.deltaTime;
		Spinner.localRotation = Quaternion.Euler(new Vector3(_rotation, 90f, 90f));

		ForceSphere.localScale = Vector3.one * Mathf.Lerp(SphereSizes.x, SphereSizes.y, lerpFactor);
		_material.SetColor("_EmissionColor", Color.Lerp(Color.black, Color.white, lerpFactor));

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

		StartCoroutine(GrowRoutine());

		_initialized = true;
	}

	IEnumerator GrowRoutine()
	{
		for (var i = 0f; i < 1f; i += Time.deltaTime / _growTime) {
			var iSmooth = lerpCubic(i);
			AttractorParent.localPosition = new Vector3(0f, Mathf.Lerp(0f, _targetHeight, iSmooth));
			AttractorParent.localScale = Vector3.one * Mathf.Lerp(0f, 1f, iSmooth);
			
			var v = Pole.localScale;
			v.y = Mathf.Lerp(0f, 0.5f * _targetHeight, iSmooth);
			Pole.localScale = v;
			Pole.localPosition = new Vector3(0f, Mathf.Lerp(0f, 0.5f * _targetHeight, iSmooth), 0f);
			yield return null;
		}

		StartCoroutine(ToneRoutine());
	}
	
	IEnumerator ToneRoutine() 
	{
		yield return new WaitForSeconds(IntervalTime + Meditation.DecayTime - Meditation.LeadInTime);

		for(float i = 0; i < 1f; i += Time.deltaTime / Meditation.LeadInTime) {
			Force.Strength = Mathf.Lerp(Meditation.MinCharge, Meditation.MaxCharge - (Meditation.MaxCharge - Meditation.MinCharge) * 0.7f, lerpCubic(i));
			yield return null;
		}
		
		AudioSource.Play();

		for(float i = 0; i < 1f; i += Time.deltaTime / Meditation.DecayTime) {
			Force.Strength = Mathf.Lerp(Meditation.MaxCharge, Meditation.MinCharge, lerpQuadraticOut(i));
			yield return null;
		}

		StartCoroutine(ToneRoutine());
	}
	
	private float lerpQuadraticOut(float t) 
	{
		return -1f * ((t - 1f) * (t - 1f)) + 1f;
	}

	private float lerpCubic(float t) 
	{
		if(t < 0.5f) return 4f * t * t * t;
		t--;
		return 4f * t * t * t + 1f;
	}
}
