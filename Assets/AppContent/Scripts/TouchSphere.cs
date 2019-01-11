using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TouchSphere : MonoBehaviour
{


	public Transform LookTarget;
	public Transform[] Interactors;
	public Color Color = Color.red;

	public float MaxSpeedMultiplier = 5f;
	public Vector2 Ranges = new Vector2(0.8f, 0.2f);
	public AnimationCurve RangeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	
	private Material _myMaterial;
	private Vector2 _defaultSpeeds;
	private Vector2 _customTime;

	public void Awake()
	{
		_myMaterial = GetComponent<Renderer>().material;

		_myMaterial.SetColor("_Color", Color.Lerp(Color, Color.black, 0.1f));
		_myMaterial.SetColor("_Emission", Color);

		_myMaterial.SetFloat("_FrequencyX", _myMaterial.GetFloat("_FrequencyX") * Random.Range(0.9f, 1.1f));
		_myMaterial.SetFloat("_FrequencyY", _myMaterial.GetFloat("_FrequencyY") * Random.Range(0.9f, 1.1f));

		_defaultSpeeds = new Vector2(_myMaterial.GetFloat("_SpeedX"), _myMaterial.GetFloat("_SpeedY"));

		_customTime = Vector2.zero;
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

	}
}
