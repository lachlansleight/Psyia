using UnityEngine;
using System.Collections;

public class MeditationDrone : MonoBehaviour {

	public AudioSource DroneStart;
	public AudioSource DroneA;
	public AudioSource DroneB;

	bool playingStart = true;
	bool playingA = false;
	bool fading = false;

	[Range(0f, 1f)] public float startPos;
	[Range(0f, 1f)] public float aPos;
	[Range(0f, 1f)] public float bPos;

	[Range(0f, 1f)] public float startVol;
	[Range(0f, 1f)] public float aVol;
	[Range(0f, 1f)] public float bVol;

	// Use this for initialization
	void Start () {
		DroneA.volume = 0;
		DroneB.volume = 0;
		DroneStart.volume = 1;

		DroneStart.Play();
		DroneB.Play();
		DroneA.timeSamples = DroneB.clip.samples / 2;
		DroneA.Play();

		StartCoroutine(DroneEntry());
	}

	IEnumerator DroneEntry() {
		yield return new WaitForSeconds(DroneStart.clip.length - 6f);
		for(float i = -1; i < 1; i += Time.deltaTime / 2.5f) {
			float[] crossFadeValues = CrossFade(i);
			DroneStart.volume = crossFadeValues[1];
			DroneA.volume = crossFadeValues[0];
			yield return null;
		}

		DroneStart.volume = 0;
		DroneA.volume = 1;

		playingStart = false;
		playingA = true;

		DroneStart.Stop();
	}

	IEnumerator CrossFade(bool atob) {
		fading = true;
		for(float i = -1; i < 1; i += Time.deltaTime / 2.5f) {
			float[] crossFadeValues = CrossFade(i);
			DroneA.volume = crossFadeValues[atob ? 1 : 0];
			DroneB.volume = crossFadeValues[atob ? 0 : 1];
			yield return null;
		}

		DroneA.volume = atob ? 0f: 1f;
		DroneB.volume = atob ? 1f : 0f;

		fading = false;
		playingA = !atob;
	}
	
	// Update is called once per frame
	void Update () {

		startPos = (float)DroneStart.timeSamples / (float)DroneStart.clip.samples;
		aPos = (float)DroneA.timeSamples / (float)DroneA.clip.samples;
		bPos = (float)DroneB.timeSamples / (float)DroneB.clip.samples;

		startVol = DroneStart.volume;
		aVol = DroneA.volume;
		bVol = DroneB.volume;

		if(!playingStart && !fading) { 
			if(playingA && aPos > 0.9f) {
				StartCoroutine(CrossFade(true));
			} else if(!playingA && bPos > 0.9f) {
				StartCoroutine(CrossFade(false));
			}
		}
	}

	float[] CrossFade(float t) {
		float[] returnValue = new float[2];
		returnValue[0] = Mathf.Sqrt(0.5f * (1f + t));
		returnValue[1] = Mathf.Sqrt(0.5f * (1f - t));
		return returnValue;
	}
}
