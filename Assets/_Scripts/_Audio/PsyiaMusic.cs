using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;

public class PsyiaMusic : MonoBehaviour {

	[Header("Required")]
	public string[] ClipNames;
	public AudioClip[] Clips;
	
	[Header("Properties")]
	public float Volume = 1f;
	public int DefaultTrack = 2;
	public float PitchLerpTime = 0.1f;
	public bool Loop = false;
	public bool AutoPlay = true;
	
	
	[Header("Status")]
	public int CurrentTrack = 2;
	public float TimeInTrack;
	public bool IsPlaying = false;
	public bool HasBeenStopped = false;
	public bool PlayingOneOff = false;
	
	private AudioSource _mySource;
	private bool _initialized = false;
	private float _targetPitch;
	private float _currentPitch;

	public void Awake () {
		_mySource = GetComponent<AudioSource>();
		_mySource.clip = Clips[DefaultTrack];
	}
	
	public void Update () {
		//if(PsyiaSettings.MusicSlowsWithTime) mySource.pitch = Mathf.Lerp(minSpeed, 1f, starLab.timeScale);
		//else mySource.pitch = 1f;

		_mySource.volume = Volume;
		
		//when the track ends, go to the next one (or loop this one)
		if(!_mySource.isPlaying && !HasBeenStopped && _initialized && AutoPlay) {
			if(Loop) _mySource.Play();
			else NextTrack();
		}

		_currentPitch = Mathf.Lerp(_currentPitch, _targetPitch, PitchLerpTime);
		_mySource.pitch = _currentPitch;

		TimeInTrack = _mySource.time;
		
		IsPlaying = _mySource.isPlaying;
	}

	public void SetClip(AudioClip clip)
	{
		_mySource.clip = clip;
		PlayingOneOff = System.Array.IndexOf(Clips, clip) == -1;
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
		if (PlayingOneOff) {
			CurrentTrack = Random.Range(0, Clips.Length);
			PlayingOneOff = false;
		}
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
			PlaySetClip();
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

	public void Pause()
	{
		HasBeenStopped = true;
		_mySource.Pause();
	}

	public void Reset()
	{
		_mySource.clip = Clips[DefaultTrack];
		_mySource.Stop();
		HasBeenStopped = false;
		_initialized = false;
		PlayingOneOff = false;
	}

	public void PlayFirstClip() {

		_mySource.clip = Clips[CurrentTrack];
		_mySource.Play();
		_initialized = true;
	}

	public void PlaySetClip()
	{
		_mySource.Play();
		_initialized = true;
	}
}
