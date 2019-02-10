using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Psyia;
using UnityEngine;

public class Meditation : MonoBehaviour
{

	[Header("Required")]
	public AudioClip[] Tones;
	public Object CrystalPrefab;
	public TextAsset PresetJson;
	public PsyiaEmitter Emitter;

	[Header("Properties")]
	public int MaxCrystalCount = 20;

	public float LowestInterval = 10f;
	public float HighestInterval = 15f;
	public float SpawnInterval = 30f;
	public float MinCharge = 0.1f;
	public float MaxCharge = 5f;
	[Space(5)]
	public float DecayTime = 5f;
	public float LeadInTime = 2f;

	[Header("Status")]
	public List<MeditationCrystal> Crystals;

	private int[] _toneCounts;
	
	public void Start()
	{
		Crystals = new List<MeditationCrystal>();
		_toneCounts = new int[Tones.Length];
	}

	public void BeginMeditation()
	{
		SpawnCrystal(5f);
		PlayDrone();
		StartCoroutine(MeditationRoutine());
	}

	public void PlayDrone()
	{
		//TODO: add meditation drone
	}

	IEnumerator MeditationRoutine()
	{
		yield return new WaitForSeconds(SpawnInterval);
		if (Crystals.Count < MaxCrystalCount) {
			SpawnCrystal(30f);
		}

		StartCoroutine(MeditationRoutine());
	}

	public void SpawnCrystal(float growTime)
	{
		var newObj = Instantiate(CrystalPrefab, transform) as GameObject;
		var crystal = newObj.GetComponent<MeditationCrystal>();
		var position = GetCrystalPosition();
		crystal.transform.position = new Vector3(position.x, 0f, position.z);
		var index = 0;
		var tone = GetMeditationTone(out index);
		crystal.Initialize(this, tone, Mathf.Lerp(LowestInterval, HighestInterval, Mathf.InverseLerp(0, Tones.Length, index)), position.y, growTime);
		Crystals.Add(crystal);
	}

	private AudioClip GetMeditationTone(out int index)
	{
		var minCount = int.MaxValue;
		var choice = -1;
		index = Random.Range(0, Tones.Length);
		for (var i = index; i < Tones.Length + index; i++) {
			var j = i % Tones.Length;
			if (_toneCounts[j] < minCount) {
				minCount = _toneCounts[j];
				choice = j;
			} else if (_toneCounts[j] == minCount && Random.Range(0f, 1f) < 0.5f) {
				choice = j;
			}
		}

		index = choice;
		return Tones[choice];
	}

	private Vector3 GetCrystalPosition()
	{
		var pos = CylindricalPosition();
		var attempts = 0;

		while (attempts < 1000) {
			var requiredDistance = 0.4f * 0.4f;
			var minDistance = float.MaxValue;
			for (var i = 0; i < Crystals.Count(); i++) {
				var posA = new Vector2(pos.x, pos.z);
				var posB = new Vector2(Crystals[i].transform.position.x, Crystals[i].transform.position.z);
				var distance = (posA - posB).sqrMagnitude;
				if (distance < minDistance) minDistance = distance;
			}

			if (minDistance > requiredDistance) break;

			pos = CylindricalPosition();
			attempts++;
		}

		if (attempts >= 1000) {
			Debug.LogWarning("Failed to find appropriate Meditation Crystal location in 1000 attempts");
		}

		return pos;
	}

	private Vector3 CylindricalPosition()
	{
		var radius = Random.Range(0.6f, 5f);
		var angle = Random.Range(-1f, 1f) * Mathf.PI * 0.5f;
		var height = Random.Range(0.25f, 2.5f);

		angle += Mathf.PI / 2f;

		return new Vector3(radius * Mathf.Cos(angle), height, radius * Mathf.Sin(angle));
	}
	
	public void Update()
	{
		
	}
}
