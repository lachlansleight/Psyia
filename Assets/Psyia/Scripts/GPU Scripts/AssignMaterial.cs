using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCTK;

[RequireComponent(typeof(ComputeRenderer))]
public class AssignMaterial : MonoBehaviour {

	ComputeRenderer MyRenderer;
	public ParticleVisuals Visuals;

	// Use this for initialization
	void Start () {
		MyRenderer = GetComponent<ComputeRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		MyRenderer.RenderMaterial = Visuals.ParticleMaterial;
	}
}
