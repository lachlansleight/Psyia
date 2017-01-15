using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

	Material myMat;
	Renderer myRenderer;

	void Start() {
		myRenderer = GetComponent<Renderer>();
		myMat = GetComponent<Renderer>().material;
		SetAlpha(0);
		myRenderer.enabled = false;
	}

	public void FadeOut() {
		myRenderer.enabled = true;
		myMat.color = new Color(0,0,0,0);
		StartCoroutine(FadeRoutine());
	}

	IEnumerator FadeRoutine() {
		for(float i = 0; i < 1f; i += Time.deltaTime / 2f) {
			SetAlpha(i);
			yield return null;
		}
	}

	public void SetAlpha(float alpha) {
		Color tempC = myMat.color;
		tempC.a = alpha;
		myMat.color = tempC;
	}
}
