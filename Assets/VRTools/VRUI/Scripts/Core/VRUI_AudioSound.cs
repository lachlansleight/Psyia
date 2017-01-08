using UnityEngine;
using System.Collections;

public class VRUI_AudioSound : MonoBehaviour {

	AudioSource mySource;
	bool initialised = false;

	public void Initialise(AudioClip sourceClip, float volume) {
		mySource = GetComponent<AudioSource>();
		mySource.clip = sourceClip;
		mySource.volume = volume;
		mySource.pitch = Random.Range(0.95f, 1.05f);
		mySource.Play();
		initialised = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!mySource.isPlaying && initialised) {
			Destroy(gameObject);
		}
	}
}
