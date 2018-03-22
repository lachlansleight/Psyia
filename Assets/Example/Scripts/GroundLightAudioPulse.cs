using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLightAudioPulse : MonoBehaviour {

	public AudioData FFT;

	public Color OnCol;
	public Color OffCol;
	Renderer MyRenderer;

	void Start () {
		MyRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		MyRenderer.material.SetColor("_EmissionColor", Color.Lerp(OffCol, OnCol, FFT.avgVol));
	}
}
