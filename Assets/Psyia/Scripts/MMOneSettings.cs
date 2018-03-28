using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class MMOneSettings : MonoBehaviour {

	public EmissionSettings Emission;
	public ParticleSettings Particle;
	public ParticleVisuals Visuals;

	[Space(20)]
	public Material[] Materials;
	public Texture2D[] Textures;
	private int CurrentTexture;
	private int CurrentMaterial;

	// Use this for initialization
	void Start () {
		CurrentMaterial = System.Array.IndexOf(Materials, Visuals.ParticleMaterial);
		CurrentTexture = System.Array.IndexOf(Textures, Visuals._Image);
	}
	
	// Update is called once per frame
	void Update () {
		if(MMOne.GetButtonDown("Ch1_Cue")) {
			CurrentMaterial++;
			if(CurrentMaterial >= Materials.Length) CurrentMaterial = 0;
			Visuals.ParticleMaterial = Materials[CurrentMaterial];
		}
		if(MMOne.GetButtonDown("Ch1_1")) {
			CurrentTexture--;
			if(CurrentTexture < 0) CurrentTexture = Textures.Length - 1;
			Visuals._Image = Textures[CurrentTexture];
		}
		if(MMOne.GetButtonDown("Ch1_2")) {
			CurrentTexture++;
			if(CurrentTexture >= Textures.Length) CurrentTexture = 0;
			Visuals._Image = Textures[CurrentTexture];
		}
		Visuals._PointSize = Mathf.Lerp(0f, 1f, MMOne.GetKnob("Matrix03"));
		Visuals._LineLength = Mathf.Lerp(0f, 0.2f, MMOne.GetKnob("Matrix02"));
		Visuals._Color.a = MMOne.GetSlider("Ch1_Vol");
	}
}
