using System.Collections;
using System.Collections.Generic;
using UCTK;
using UnityEngine;

public class VisualSettings : MonoBehaviour
{
	public PsyiaSettingsApplicator SettingsApplicator;
	
	public GameObject LineLength;
	public GameObject LineLengthButton;
	public GameObject ParticleShape;
	public GameObject ParticleSize;
	public GameObject ParticleSizeButton;

	public void SetParticleForm(int formSelection)
	{
		if (formSelection < 0) formSelection = 0;
		if (formSelection >= SettingsApplicator.ParticleMaterials.Length) 
			formSelection = SettingsApplicator.ParticleMaterials.Length - 1;
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

		LineLengthButton.SetActive(LineLength.activeSelf);
		ParticleSizeButton.SetActive(ParticleSize.activeSelf);

		SettingsApplicator.SetParticleForm(formSelection);
	}

	public void SetParticleColor(int colorSelection)
	{
		if (colorSelection < 0) colorSelection = 0;
		if (colorSelection > 5) colorSelection = 5;

		SettingsApplicator.SetParticleColor(colorSelection);
	}

	public void SetParticleColorAmount(float value)
	{
		SettingsApplicator.SetParticleColorAmount(value);
	}

	public void SetLineLength(float lineLength)
	{
		SettingsApplicator.SetLineLength(lineLength);
	}

	public void SetParticleShape(int shapeSelection)
	{
		if (shapeSelection < 0) shapeSelection = 0;
		if (shapeSelection >= SettingsApplicator.ParticleTextures.Length) 
			shapeSelection = SettingsApplicator.ParticleTextures.Length - 1;

		SettingsApplicator.SetParticleShape(shapeSelection);
	}

	public void SetParticleSize(float sizeSelection)
	{
		SettingsApplicator.SetParticleSize(sizeSelection);
	}
}
