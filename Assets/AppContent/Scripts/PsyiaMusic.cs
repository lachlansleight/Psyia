using UnityEngine;
using System.Collections;

public class PsyiaMusic : MonoBehaviour {

	
	
	public float MinSpeed = 0.5f;
	public bool SlowWithTime = false;
	public string[] ClipNames;
	public AudioClip[] Clips;
	public int CurrentTrack = 2;

	public bool HasBeenStopped = false;

	public bool Loop = false;

	public float Volume = 1f;

	private bool _initialized = false;

	private AudioSource _mySource;

	public void Start () {
		_mySource = GetComponent<AudioSource>();
	}
	
	public void Update () {
		//if(PsyiaSettings.MusicSlowsWithTime) mySource.pitch = Mathf.Lerp(minSpeed, 1f, starLab.timeScale);
		//else mySource.pitch = 1f;

		_mySource.volume = Volume;
		
		//when the track ends, go to the next one (or loop this one)
		if(!_mySource.isPlaying && !HasBeenStopped && _initialized) {
			if(Loop) _mySource.Play();
			else NextTrack();
		}
	}

	public void NextTrack() {
		CurrentTrack++;
		if(CurrentTrack >= Clips.Length) CurrentTrack = 0;
		_mySource.Stop();
		_mySource.clip = Clips[CurrentTrack];
		_mySource.Play();
	}

	public void PreviousTrack() {
		CurrentTrack--;
		if(CurrentTrack < 0) CurrentTrack = Clips.Length - 1;
		_mySource.Stop();
		_mySource.clip = Clips[CurrentTrack];
		_mySource.Play();
	}

	public void PlayPause() {
		if(HasBeenStopped) {
			HasBeenStopped = false;
			_mySource.Play();
		} else {
			HasBeenStopped = true;
			_mySource.Stop();
		}
	}

	public void PlayFirstClip() {

		_mySource.clip = Clips[CurrentTrack];
		_mySource.Play();
		_initialized = true;
	}
}
