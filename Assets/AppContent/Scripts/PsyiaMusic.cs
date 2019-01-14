using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;

public class PsyiaMusic : MonoBehaviour {

	
	
	public float MinSpeed = 0.5f;
	public bool SlowWithTime = false;
	public string[] ClipNames;
	public AudioClip[] Clips;
	public int CurrentTrack = 2;
	public bool IsPlaying = false;

	public bool HasBeenStopped = false;

	public bool Loop = false;

	public float Volume = 1f;

	private bool _initialized = false;

	private AudioSource _mySource;

	private float _targetPitch;
	private float _currentPitch;
	public float PitchLerpTime = 0.1f;

	public void Start () {
		_mySource = GetComponent<AudioSource>();
		_mySource.clip = Clips[CurrentTrack];
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

		_currentPitch = Mathf.Lerp(_currentPitch, _targetPitch, PitchLerpTime);
		_mySource.pitch = _currentPitch;
		
		IsPlaying = _mySource.isPlaying;
	}

	public void SetPitch(float value)
	{
		_targetPitch = value;
	}

	public string GetCurrentSongName()
	{
		for (var i = 0; i < Clips.Length; i++) {
			if (Clips[i] == _mySource.clip) return ClipNames[i];
		}

		return "Unknown song";
	}

	public void NextTrack() {
		CurrentTrack++;
		if(CurrentTrack >= Clips.Length) CurrentTrack = 0;
		_mySource.Stop();
		_mySource.clip = Clips[CurrentTrack];
		_mySource.Play();
	}

	public void PreviousTrack()
	{
		if (_mySource.time > 3f) {
			_mySource.time = 0f;
			return;
		}
		
		CurrentTrack--;
		if(CurrentTrack < 0) CurrentTrack = Clips.Length - 1;
		_mySource.Stop();
		_mySource.clip = Clips[CurrentTrack];
		_mySource.Play();
	}

	public void PlayPause() {
		if (!_initialized) {
			PlayFirstClip();
			return;
		}
		if(HasBeenStopped) {
			HasBeenStopped = false;
			_mySource.UnPause();
		} else {
			HasBeenStopped = true;
			_mySource.Pause();
		}
	}

	public void PlayFirstClip() {

		_mySource.clip = Clips[CurrentTrack];
		_mySource.Play();
		_initialized = true;
	}
}
