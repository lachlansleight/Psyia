using UnityEngine;
using System.Collections;

public class SkyboxPulse : MonoBehaviour {

	public AudioData audioData;
	public Material mat;
	public Color minCol = Color.black;
	public Color maxCol = Color.white;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		mat.color = Color.Lerp(minCol, maxCol, audioData.avgVol);
	}
}
