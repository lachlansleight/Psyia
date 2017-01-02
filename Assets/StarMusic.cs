using UnityEngine;
using System.Collections;

public class StarMusic : MonoBehaviour {

	public Dispatcher stars;
	public float minSpeed = 0.5f;

	AudioSource mySource;

	// Use this for initialization
	void Start () {
		mySource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		mySource.pitch = Mathf.Lerp(minSpeed, 1f, stars.timeScale);
	}
}
