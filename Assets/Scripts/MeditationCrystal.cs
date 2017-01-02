using UnityEngine;
using System.Collections;

public class MeditationCrystal : MonoBehaviour {

	public float minCharge = 0.1f;
	public float maxCharge = 5f;
	public float intervalTime = 30f;
	public float decayTime = 5f;
	public float leadInTime = 2f;
	public float height;

	public Color minColor = new Color32(14, 14, 14, 255);
	public Color maxColor = Color.white;
	public float minSpeed = 10f;
	public float maxSpeed = 5000f;
	public float minSphereRadius = 0.005f;
	public float maxSphereRadius = 0.1f;

	public float growTime = 30f;

	public float outputCharge = 0f;

	public Transform crystal;
	public Transform crystalTop;
	public Transform spinner;
	public Transform sphere;

	Coroutine toneRoutine;

	float spinnerRot;
	AudioSource myAudioSource;
	Material glowMat;

	public bool manualInitialise;
	bool initialised = false;

	public bool deleteMe = false;

	public float age = 0f;

	public void Initialise(AudioClip sourceTone, Vector3 position) {
		myAudioSource = GetComponent<AudioSource>();
		myAudioSource.clip = sourceTone;
		glowMat = sphere.GetComponent<Renderer>().material;
		Material[] tempMat = spinner.GetComponent<Renderer>().materials;
		tempMat[1] = glowMat;
		spinner.GetComponent<Renderer>().materials = tempMat;

		height = position.y;
		transform.position = new Vector3(position.x, 0f, position.z);
		crystal.localScale = new Vector3(1f, 0f, 1f);
		crystalTop.localPosition = Vector3.zero;
		spinner.localPosition = Vector3.zero;
		spinner.localScale = Vector3.zero;
		sphere.localScale = Vector3.zero;

		initialised = true;
		StartCoroutine(LerpUp());
	}

	public void Stop() {
		StopCoroutine(toneRoutine);
		LerpDown();
	}

	void Update() {
		if(manualInitialise && !initialised) {
			initialised = true;
			Initialise(GetComponent<AudioSource>().clip, new Vector3(transform.position.x, 0.5f, transform.position.z));
		}

		if(!initialised) return;

		spinnerRot += Mathf.Lerp(minSpeed, maxSpeed, Mathf.InverseLerp(minCharge, maxCharge, outputCharge)) * Time.deltaTime;
		spinner.localRotation = Quaternion.Euler(new Vector3(0f, spinnerRot, 0f));

		sphere.localScale = Vector3.one * Mathf.Lerp(minSphereRadius, maxSphereRadius, Mathf.InverseLerp(minCharge, maxCharge, outputCharge));
		glowMat.SetColor("_EmissionColor", Color.Lerp(minColor, maxColor, Mathf.InverseLerp(minCharge, maxCharge, outputCharge)));

		age += Time.deltaTime;
	}

	IEnumerator ToneRoutine() {
		Debug.Log("Waiting " + intervalTime + decayTime);
		yield return new WaitForSeconds(intervalTime + decayTime - leadInTime);

		for(float i = 0; i < 1f; i += Time.deltaTime / leadInTime) {
			outputCharge = Mathf.Lerp(minCharge, maxCharge - (maxCharge - minCharge) * 0.7f, lerpCubic(i));
			yield return null;
		}
		
		Debug.Log("Dong");
		myAudioSource.Play();

		for(float i = 0; i < 1f; i += Time.deltaTime / decayTime) {
			outputCharge = Mathf.Lerp(maxCharge, minCharge, lerpQuadraticOut(i));
			yield return null;
		}

		toneRoutine = StartCoroutine(ToneRoutine());
	}

	IEnumerator LerpUp() {
		for(float i = 0; i < 1f; i += Time.deltaTime / growTime) {
			Vector3 tempPos = transform.position;
			tempPos.y = Mathf.Lerp(-0.017f, 0f, lerpCubic(i));
			transform.position = tempPos;
			crystal.localScale = new Vector3(1f, Mathf.Lerp(0f, height, lerpCubic(i)), 1f);
			crystalTop.localPosition = new Vector3(0f, Mathf.Lerp(0f, height, lerpCubic(i)), 0f);
			spinner.localPosition = new Vector3(0f, Mathf.Lerp(0f, height, lerpCubic(i)), 0f);
			sphere.localPosition = new Vector3(0f, Mathf.Lerp(0f, height, lerpCubic(i)) + 0.095f, 0f);
			spinner.localScale = Vector3.one * Mathf.Lerp(0f, 1f, lerpCubic(i));
			sphere.localScale = Vector3.one * Mathf.Lerp(0f, minSphereRadius, lerpCubic(i));

			yield return null;
		}

		toneRoutine = StartCoroutine(ToneRoutine());
	}

	IEnumerator LerpDown() {
		for(float i = 1f; i > 0f; i -= Time.deltaTime / growTime) {
			Vector3 tempPos = transform.position;
			tempPos.y = Mathf.Lerp(-0.017f, 0f, lerpCubic(i));
			transform.position = tempPos;
			crystal.localScale = new Vector3(1f, Mathf.Lerp(0f, height, lerpCubic(i)), 1f);
			crystalTop.localPosition = new Vector3(0f, Mathf.Lerp(0f, height, lerpCubic(i)), 0f);
			spinner.localPosition = new Vector3(0f, Mathf.Lerp(0f, height, lerpCubic(i)), 0f);
			sphere.localPosition = new Vector3(0f, Mathf.Lerp(0f, height, lerpCubic(i)) + 0.095f, 0f);
			spinner.localScale = Vector3.one * Mathf.Lerp(0f, 1f, lerpCubic(i));
			sphere.localScale = Vector3.one * Mathf.Lerp(0f, minSphereRadius, lerpCubic(i));

			yield return null;
		}

		deleteMe = true;
	}

	float lerpQuadraticOut(float t) {
		return -1f * ((t - 1f) * (t - 1f)) + 1f;
	}

	float lerpCubic(float t) {
		if(t < 0.5f) return 4f * t * t * t;
		t--;
		return 4f * t * t * t + 1f;
	}
}
