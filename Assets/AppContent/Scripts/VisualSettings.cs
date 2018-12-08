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
	public ComputeRenderer TargetRenderer;
	public ComputeShader ColourShader;
	
	public void Start () {
		
	}
	
	public void Update () {
		
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

	public void SetParticleColour(int colourSelection)
	{
		if (colourSelection < 0) colourSelection = 0;
		if (colourSelection > 5) colourSelection = 5;
		
		ColourShader.SetFloat("ColourMode", colourSelection);
	}

	public void SetLineLength(float lineLength)
	{
		TargetRenderer.RenderMaterial.SetFloat("_LineLength", lineLength);
	}

	public void SetParticleShape(int shapeSelection)
	{
		if (shapeSelection < 0) shapeSelection = 0;
		if (shapeSelection >= ParticleTextures.Length) shapeSelection = ParticleTextures.Length - 1;
		
		TargetRenderer.RenderMaterial.SetTexture("_Image", ParticleTextures[shapeSelection]);
	}

	public void SetParticleSize(float sizeSelection)
	{
		TargetRenderer.RenderMaterial.SetFloat("_PointSize", sizeSelection);
	}
}
