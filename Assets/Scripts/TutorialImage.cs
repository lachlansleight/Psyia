using UnityEngine;
using System.Collections;

public class TutorialImage : MonoBehaviour {

	public Transform lookTarget;

	public Texture2D[] textures;
	public int index = 0;
	public float changeTime = 1f;
	public float fadeDelay = 20f;
	public float fadeTime = 2f;

	Material myMat;

	// Use this for initialization
	void Start () {
		myMat = GetComponent<Renderer>().material;

		StartCoroutine(FadeIn());
		StartCoroutine(ChangeTex());
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 lookDir = Quaternion.LookRotation(transform.position - lookTarget.position).eulerAngles;
		lookDir.x = lookDir.z = 0f;
		transform.rotation = Quaternion.Euler(lookDir);
	}

	IEnumerator FadeIn() {
		Color offCol = Color.black;
		myMat.SetColor("_EmissionColor", offCol);

		yield return new WaitForSeconds(fadeDelay);

		for(float i = 0; i < 1f; i += Time.deltaTime / fadeTime) {
			myMat.SetColor("_EmissionColor", Color.Lerp(offCol, Color.white, i));
			yield return null;
		}
	}

	IEnumerator ChangeTex() {
		yield return new WaitForSeconds(changeTime);

		myMat.SetTexture("_EmissionMap", textures[index]);
		index++;
		if(index >= textures.Length) index = 0;

		StartCoroutine(ChangeTex());
	}
}
