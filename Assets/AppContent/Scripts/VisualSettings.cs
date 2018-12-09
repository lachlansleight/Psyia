using System.Collections;
using System.Collections.Generic;
using UCTK;
using UnityEngine;

public class VisualSettings : MonoBehaviour
{

	public GameObject LineLength;
	public GameObject ParticleShape;
	public GameObject ParticleSize;

	public Material[] ParticleMaterials;
	public Texture2D[] ParticleTextures;
	public Texture2D[] ColorTextures;
	public ComputeRenderer TargetRenderer;
	public ComputeShader ColorShader;

	public Color[] _debugColors;
	[Range(0f, 1f)] public float DebugValue;
	
	public void Start () {
		
	}
	
	public void Update ()
	{
		ColorShader.SetFloat("DebugValue", DebugValue);
	}

	public void SetParticleForm(int formSelection)
	{
		if (formSelection < 0) formSelection = 0;
		if (formSelection >= ParticleMaterials.Length) formSelection = ParticleMaterials.Length - 1;
		switch (formSelection) {
			case 0:
				LineLength.SetActive(false);
				ParticleShape.SetActive(false);
				ParticleSize.SetActive(false);
				break;
			case 1:
				LineLength.SetActive(true);
				ParticleShape.SetActive(false);
				ParticleSize.SetActive(false);
				break;
			case 2:
				LineLength.SetActive(false);
				ParticleShape.SetActive(true);
				ParticleSize.SetActive(true);
				break;
			case 3:
				LineLength.SetActive(true);
				ParticleShape.SetActive(false);
				ParticleSize.SetActive(true);
				break;
			default:
				throw new System.FormatException("Unexpected value for formSelection : " + formSelection);
		}

		TargetRenderer.RenderMaterial = ParticleMaterials[formSelection];
	}

	public void SetParticleColor(int colorSelection)
	{
		if (colorSelection < 0) colorSelection = 0;
		if (colorSelection > 5) colorSelection = 5;

		if (colorSelection > 1) {
			var xColorMin = ColorTextures[colorSelection].GetPixel(32, 32);
			var xColorMax = ColorTextures[colorSelection].GetPixel(96, 64);
			var yColorMin = ColorTextures[colorSelection].GetPixel(32, 64);
			var yColorMax = ColorTextures[colorSelection].GetPixel(96, 96);
			var zColorMin = ColorTextures[colorSelection].GetPixel(32, 96);
			var zColorMax = ColorTextures[colorSelection].GetPixel(96, 32);

			_debugColors = new []
			{
				xColorMin,
				xColorMax,
				yColorMin,
				yColorMax,
				zColorMin,
				zColorMax
			};

			ColorShader.SetVector("xColorMin", xColorMin);
			ColorShader.SetVector("xColorMax", xColorMax);
			ColorShader.SetVector("yColorMin", yColorMin);
			ColorShader.SetVector("yColorMax", yColorMax);
			ColorShader.SetVector("zColorMin", zColorMin);
			ColorShader.SetVector("zColorMax", zColorMax);
		}
		
		ColorShader.SetInt("ColorMode", colorSelection);
	}

	public void SetLineLength(float lineLength)
	{
		foreach(var m in ParticleMaterials) {
			m.SetFloat("_LineLength", lineLength);
		}
	}

	public void SetParticleShape(int shapeSelection)
	{
		if (shapeSelection < 0) shapeSelection = 0;
		if (shapeSelection >= ParticleTextures.Length) shapeSelection = ParticleTextures.Length - 1;
		
		foreach(var m in ParticleMaterials) {
			m.SetTexture("_Image", ParticleTextures[shapeSelection]);
		}
	}

	public void SetParticleSize(float sizeSelection)
	{
		foreach(var m in ParticleMaterials) {
			m.SetFloat("_PointSize", sizeSelection);
		}
	}
}
