using UnityEngine;
using System.Collections;

public class MeditationMenuButtons : MonoBehaviour {

	public Meditation med;
	Renderer myRenderer;

	// Use this for initialization
	void Start () {
		myRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		myRenderer.material.SetColor("_EmissionColor", Color.Lerp(Color.black, Color.white, med.menuTimer / med.maxMenuTimer));
	}
}
