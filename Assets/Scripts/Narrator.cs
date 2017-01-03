using UnityEngine;
using System.Collections;

public class Narrator : MonoBehaviour {

	public AudioSource[] sources;
	public float[] waitTimes;
	public TouchToBeginSphere touchSphereA;
	public TouchToBeginSphere touchSphereB;

	// Use this for initialization
	void Start () {
		if(!PlayerPrefs.HasKey("NumberPlays")) PlayerPrefs.SetInt("NumberPlays", 1);

		else PlayerPrefs.SetInt("NumberPlays", PlayerPrefs.GetInt("NumberPlays") + 1);

		StartCoroutine(DoNarration());
	}
	
	// Update is called once per frame
	void Update () {
		//manually start tutorial - note that you have 2 seconds to press this!
		if(Input.GetKeyDown(KeyCode.T)) {
			PlayerPrefs.SetInt("NumberPlays", 1);
		}
	}

	IEnumerator DoNarration() {
		yield return new WaitForSeconds(PlayerPrefs.GetInt("NumberPlays") == 1 ? 8f : 2f);
		sources[0].Play();
		
		if(PlayerPrefs.GetInt("NumberPlays") == 1) {
			yield return new WaitForSeconds(waitTimes[0]);
			//do tutorial
			for(int i = 1; i < sources.Length - 1; i++) {
				sources[i].Play();
				yield return new WaitForSeconds(waitTimes[i]);
				if(i == sources.Length - 3) {
					touchSphereA.StartLerp();
					touchSphereB.StartLerp();
				}
			}
		} else if(PlayerPrefs.GetInt("NumberPlays") > 1) {
			touchSphereA.StartLerp();
			touchSphereB.StartLerp();

			yield return new WaitForSeconds(3f);


			

			if(PlayerPrefs.GetInt("NumberPlays") == 2) {
				sources[sources.Length - 1].Play();
				//tell them about music
				yield return new WaitForSeconds(waitTimes[waitTimes.Length - 1]);
			}

			sources[sources.Length - 2].Play();

			
			
			
		}
	}
}
