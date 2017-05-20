using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour {

	public Material mainMat;
	public Material triggerMat;
	public Transform trigger;

	public Color baseStartCol;
	public Color baseEndCol;
	public Color triggerStartCol;
	public Color triggerEndColA;
	public Color triggerEndColB;

	public Vector3 triggerRotationA = Vector3.zero;
	public Vector3 triggerRotationB = new Vector3(-15f, 0f, 0f);

	public Vector3 startPos = new Vector3(0.1266f, 1.3532f, -0.0789f);
	public Vector3 endPos = new Vector3(0.0194f, 1.3221f, -0.0021f);

	public float initialDelay = 17f;
	public float mainLerpTime = 1f;
	public float triggerLerpSpeed = 0.3f;

	public float repeatInterval = 2f;

	// Use this for initialization
	void Start () {
		mainMat.SetColor("_EmissionColor", baseStartCol);
		triggerMat.SetColor("_EmissionColor", triggerStartCol);
		transform.localScale = Vector3.zero;

		//temp
		PlayerPrefs.SetFloat("NumberPlays", 0);

		StartCoroutine(RunTutorial(initialDelay));
	}

	IEnumerator RunTutorial(float waitTime) {
		yield return new WaitForSeconds(waitTime);

		transform.localScale = Vector3.one;
		for(float i = 0; i < 1f; i += Time.deltaTime / mainLerpTime) {
			transform.position = Vector3.Lerp(startPos, endPos, lerpCubic(i));
			mainMat.SetColor("_EmissionColor", Color.Lerp(baseStartCol, baseEndCol, i));
			triggerMat.SetColor("_EmissionColor", Color.Lerp(triggerStartCol, triggerEndColA, i));
			yield return null;
		}
		for(int count = 0; count < 3; count++) {
			for(float i = 0; i < 1f; i += Time.deltaTime / (triggerLerpSpeed * 0.5f)) {
				trigger.localEulerAngles = Vector3.Lerp(triggerRotationA, triggerRotationB, lerpCubic(i));
				triggerMat.SetColor("_EmissionColor", Color.Lerp(triggerEndColA, triggerEndColB, i));
				yield return null;
			}
			for(float i = 0; i < 1f; i += Time.deltaTime / (triggerLerpSpeed * 0.5f)) {
				trigger.localEulerAngles = Vector3.Lerp(triggerRotationB, triggerRotationA, lerpCubic(i));
				triggerMat.SetColor("_EmissionColor", Color.Lerp(triggerEndColB, triggerEndColA, i));
				yield return null;
			}

			yield return new WaitForSeconds(0.5f);
		}

		for(float i = 0; i < 1f; i += Time.deltaTime / mainLerpTime) {
			mainMat.SetColor("_EmissionColor", Color.Lerp(baseEndCol, baseStartCol, i));
			triggerMat.SetColor("_EmissionColor", Color.Lerp(triggerEndColA, triggerStartCol, i));
			yield return null;
		}

		StartCoroutine(RunTutorial(repeatInterval));
	}

	float lerpCubic(float t) {
		if(t < 0.5f) return 4f * t * t * t;
		t--;
		return 4f * t * t * t + 1f;
	}
}
