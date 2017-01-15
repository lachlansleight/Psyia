using UnityEngine;
using System.Collections;

public class StarMusic : MonoBehaviour {

	public StarLab starLab;
	public float minSpeed = 0.5f;

	AudioSource mySource;

	public AudioClip[] clips;
	public int currentTrack = 2;

	public bool hasBeenStopped = false;

	public bool loop = false;

	public bool initialStart = true;

	// Use this for initialization
	void Start () {
		mySource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(PsyiaSettings.MusicSlowsWithTime) mySource.pitch = Mathf.Lerp(minSpeed, 1f, starLab.timeScale);
		else mySource.pitch = 1f;

		if(!mySource.isPlaying && !hasBeenStopped && !initialStart) {
			if(loop) mySource.Play();
			else NextTrack();
		}
	}

	public void NextTrack() {
		currentTrack++;
		if(currentTrack >= clips.Length) currentTrack = 0;
		mySource.Stop();
		mySource.clip = clips[currentTrack];
		mySource.Play();
	}

	public void PreviousTrack() {
		currentTrack--;
		if(currentTrack < 0) currentTrack = clips.Length - 1;
		mySource.Stop();
		mySource.clip = clips[currentTrack];
		mySource.Play();
	}

	public void PlayPause() {
		if(hasBeenStopped) {
			hasBeenStopped = false;
			mySource.Play();
		} else {
			hasBeenStopped = true;
			mySource.Stop();
		}
	}

	public void InitialStart() {

		mySource.clip = clips[currentTrack];
		mySource.Play();
		initialStart = false;
	}
}
