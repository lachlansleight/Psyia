using UnityEngine;
using System.Collections;

public class CopyEmissionColor : MonoBehaviour {

	public Renderer target;
	Renderer myRenderer;

	// Use this for initialization
	void Start () {
		myRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		myRenderer.material.SetColor("_EmissionColor", target.material.GetColor("_EmissionColor"));
	}
}
