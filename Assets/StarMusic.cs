using UnityEngine;
using System.Collections;

public class StarMusic : MonoBehaviour {

	public StarLab starLab;
	public float minSpeed = 0.5f;

	AudioSource mySource;

	// Use this for initialization
	void Start () {
		mySource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(PsyiaSettings.MusicSlowsWithTime) mySource.pitch = Mathf.Lerp(minSpeed, 1f, starLab.timeScale);
		else mySource.pitch = 1f;
	}
}
