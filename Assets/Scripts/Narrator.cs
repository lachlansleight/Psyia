using UnityEngine;
using System.Collections;

public class Narrator : MonoBehaviour {

	public AudioSource[] sources;
	public float[] waitTimes;

	public bool starLab = true;

	// Use this for initialization
	void Start () {
		//PlayerPrefs.SetInt("NumberPlays", 2);
		//PsyiaSettings.FirstTime = true;

		if(starLab) StartCoroutine(DoNarration());
		else StartCoroutine(MeditationNarration());
	}
	
	// Update is called once per frame
	void Update () {
		//manually start tutorial - note that you have 2 seconds to press this!
	}

	IEnumerator DoNarration() {
		yield return new WaitForSeconds(2f);
		sources[0].Play();
		yield return new WaitForSeconds(waitTimes[0]);
		
		if(PlayerPrefs.GetInt("NumberPlays") == 1) {
			//do tutorial
			for(int i = 4; i < sources.Length; i++) {
				sources[i].Play();
				yield return new WaitForSeconds(waitTimes[i]);
			}

			sources[3].Play();
			yield return new WaitForSeconds(waitTimes[3]);

		}  else if(PlayerPrefs.GetInt("NumberPlays") < 5) {
			sources[1].Play();
			yield return new WaitForSeconds(waitTimes[1]);
			sources[2].Play();
			yield return new WaitForSeconds(waitTimes[2]);
			sources[3].Play();
			yield return new WaitForSeconds(waitTimes[3]);
		} else {
			yield return new WaitForSeconds(7f);
			sources[3].Play();
		}
	}

	IEnumerator MeditationNarration() {
		yield return new WaitForSeconds(2f);
		sources[0].Play();
		yield return new WaitForSeconds(waitTimes[0]);
		sources[1].Play();
		yield return new WaitForSeconds(waitTimes[1]);
	}
}
