using UnityEngine;
using System.Collections;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using System.Collections.Generic;

public class AudioMaster : MonoBehaviour {

	public float main;
	public float bass;
	public float mid;
	public float treble;

	int chosen = 0;

	List<WaveInCapabilities> inputSources;
	List<WaveOutCapabilities> outputSources;

	WaveIn sourceStream = null;
	WaveOut outputStream = null;
	DirectSoundOut waveOut = null;

	// Use this for initialization
	void Start () {
		//GetSources();

		MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
		Debug.Log(enumerator.GetType().ToString());
		MMDeviceCollection coll = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.All);
		Debug.Log(coll.Count);
		foreach(MMDevice device in coll) {
			Debug.Log(device.FriendlyName);
		}
	}

	public void DataCallback(object sender, WaveInEventArgs e) {
		Debug.DrawLine(Vector3.zero, new Vector3(0, (float)e.Buffer[0] * 0.01f, 0), Color.red);
	}

	void OnDestroy() {
		if(sourceStream != null) {
			sourceStream.StopRecording();
			sourceStream.Dispose();
			sourceStream = null;
		}
	}
	/*

	void GetSources() {
		inputSources = new List<WaveInCapabilities>();
		outputSources = new List<WaveOutCapabilities>();

		for(int i = 0; i < WaveIn.DeviceCount; i++) inputSources.Add(WaveIn.GetCapabilities(i));
		for(int i = 0; i < WaveOut.DeviceCount; i++) outputSources.Add(WaveOut.GetCapabilities(i));
	}


	void SetupSound(int chosenIndex) {
		sourceStream = new WaveIn();
		sourceStream.DeviceNumber = chosenIndex;
		sourceStream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(chosenIndex).Channels);

		WaveInProvider waveIn = WaveInProvider(sourceStream);

		waveOut = new DirectSoundOut();
		waveOut.Init(waveIn);

		sourceStream.StartRecording();
		waveOut.Play();
	}

	void SetupSound(int chosenIndex) {
		outputStream = new WaveOut();
		outputStream.DeviceNumber = chosenIndex;

		WaveInProvider waveIn = WaveInProvider(sourceStream);

		waveOut = new DirectSoundOut();
		waveOut.Init(outputStream);
	}

	void Stop() {
		if(WaveOut != null) {
			waveOut.Stop();
			waveOut.Dispose();
			waveOut = null;
		}
		if(sourceStream != null) {
			sourceStream.StopRecording();
			sourceStream.Dispose();
			sourceStream = null;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/
}
