using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleParticles : MonoBehaviour
{

	public Material[] ParticleMats;
	public float[] ParticleMaterialSizes;
	public Color[] ParticleColors;
	
	private ParticleSystem _particles;

	private bool _oscillationX;
	private bool _oscillationY;
	private bool _oscillationZ;
	private float _tX;
	private float _tY;
	private float _tZ;
	private Vector3 _centerPosition;

	private float _oldEmissionCount;
	private Color _startCol;
	private Color _endCol;
	private Gradient _colorGradient;

	public void Awake()
	{
		_particles = GetComponent<ParticleSystem>();
		_oldEmissionCount = _particles.emission.rateOverTime.constant;
		SetEmission(false);
		_centerPosition = transform.position;
	}

	public void Update()
	{
		transform.position = _centerPosition + new Vector3(
			1f * Mathf.Sin(_tX),
			1f * Mathf.Sin(_tY),
			1f * Mathf.Sin(_tZ)
		);

		_tX += _oscillationX ? Time.deltaTime / 1.5f : 0f;
		_tY += _oscillationY ? Time.deltaTime / 1.1f : 0f;
		_tZ += _oscillationZ ? Time.deltaTime / 1.8f : 0f;
	}

	public void SetEmission(bool value)
	{
		var em = _particles.emission;
		em.rateOverTime = new ParticleSystem.MinMaxCurve(value ? _oldEmissionCount : 0);
	}

	public void Emit(int count)
	{
		_particles.Emit(count);
	}

	public void SetSpeed(float value)
	{
		var main = _particles.main;
		main.startSpeed = new ParticleSystem.MinMaxCurve(value * 0.75f, value * 1.25f);
	}

	public void SetNoiseStrength(float value)
	{
		var noise = _particles.noise;
		noise.strength = new ParticleSystem.MinMaxCurve(value);
	}

	public void SetStartColor(int index)
	{
		if (index < 0) index = 0;
		if (index >= ParticleColors.Length) index = ParticleColors.Length - 1;

		var colorOverLifetime = _particles.colorOverLifetime;
		var col = ParticleColors[index];
		col.a = 1f;
		_startCol = col;
		_colorGradient = new Gradient();
		_colorGradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(_startCol, 0f), new GradientColorKey(_endCol, 1f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0f, 1f) }
		);
		colorOverLifetime.color = new ParticleSystem.MinMaxGradient(_colorGradient);
	}
	
	public void SetEndColor(int index)
	{
		if (index < 0) index = 0;
		if (index >= ParticleColors.Length) index = ParticleColors.Length - 1;

		var colorOverLifetime = _particles.colorOverLifetime;
		var col = ParticleColors[index];
		col.a = 0f;
		_endCol = col;
		_colorGradient = new Gradient();
		_colorGradient.SetKeys(
			new [] { new GradientColorKey(_startCol, 0f), new GradientColorKey(_endCol, 1f) },
			new [] { new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0f, 1f) }
		);
		colorOverLifetime.color = new ParticleSystem.MinMaxGradient(_colorGradient);
	}

	public void SetParticleMaterial(int index)
	{
		if (index < 0) index = 0;
		if (index >= ParticleMats.Length) index = ParticleMats.Length - 1;

		var ren = _particles.GetComponent<ParticleSystemRenderer>();
		ren.sharedMaterial = ParticleMats[index];

		var sol = _particles.sizeOverLifetime;
		sol.size = new ParticleSystem.MinMaxCurve(ParticleMaterialSizes[index]);
	}

	public void SetOscillationX(bool value)
	{
		_oscillationX = value;
	}
	
	public void SetOscillationY(bool value)
	{
		_oscillationY = value;
	}
	
	public void SetOscillationZ(bool value)
	{
		_oscillationZ = value;
	}
}
