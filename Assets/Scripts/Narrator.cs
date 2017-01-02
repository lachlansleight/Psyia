using UnityEngine;
using System.Collections;

public class Narrator : MonoBehaviour {

	public AudioSource[] sources;
	public float[] waitTimes;

	// Use this for initialization
	void Start () {
		if(!PlayerPrefs.HasKey("NumberPlays")) PlayerPrefs.SetInt("NumberPlays", 1);

		else PlayerPrefs.SetInt("NumberPlays", PlayerPrefs.GetInt("NumberPlays") + 1);

		PlayerPrefs.SetInt("NumberPlays", 1);

		StartCoroutine(DoNarration());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator DoNarration() {
		yield return new WaitForSeconds(8f);
		sources[0].Play();
		yield return new WaitForSeconds(waitTimes[0]);
		if(PlayerPrefs.GetInt("NumberPlays") == 1) {
			//do tutorial
			for(int i = 1; i < sources.Length - 1; i++) {
				sources[i].Play();
				yield return new WaitForSeconds(waitTimes[i]);
			}
		} else if(PlayerPrefs.GetInt("NumberPlays") == 2) {
			//tell them about music
			sources[sources.Length - 1].Play();
			yield return new WaitForSeconds(waitTimes[waitTimes.Length - 1]);
		}
	}
}
