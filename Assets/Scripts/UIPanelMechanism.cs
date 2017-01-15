using UnityEngine;
using System.Collections;

public class UIPanelMechanism : MonoBehaviour {

	public AudioSource whirr;
	public Transform Panel;
	public Transform Pole;
	public Transform Base;

	public float BasePopTime = 0.3f;
	public float PoleScaleTime = 1.5f;
	public float PanelScaleTime = 1f;

	public float PanelTopHeight = 1.1f;

	public bool Out = false;
	public bool lerping = false;
	int lerpCount = 0;

	public bool Toggleable = true;

	void Start() {
		Panel.localScale = Pole.localScale = Base.localScale = Vector3.zero;
	}

	void Update() {
		if(Toggleable) {
			if(VRTools.VRInput.GetDevice("ViveRight").GetButtonDown("Menu") || VRTools.VRInput.GetDevice("ViveLeft").GetButtonDown("Menu")) Pop();
		}
	}

	public void Pop() {
		if(lerping) return;
		if(Out) PopIn();
		else PopOut();
	}

	public void PopOut() {
		if(lerping) return;

		Out = true;
		lerping = true;
		StartCoroutine(ScaleBase(true));
		StartCoroutine(ScalePole(true));
		StartCoroutine(ScalePanel(true));
	}

	public void PopIn() {
		if(lerping) return;

		Out = false;
		lerping = true;
		StartCoroutine(ScaleBase(false));
		StartCoroutine(ScalePole(false));
		StartCoroutine(ScalePanel(false));
	}

	IEnumerator ScaleBase(bool outwards) {
		lerpCount++;

		if(!outwards) yield return new WaitForSeconds(PoleScaleTime);

		Base.gameObject.SetActive(true);

		float Start = outwards ? 0f : 1f;
		float Stop = outwards ? 1f : 0f;

		for(float i = 0; i < 1f; i += Time.deltaTime / BasePopTime) {
			Base.localScale = Vector3.one * Mathf.Lerp(Start, Stop, lerpCubic(i));
			yield return null;
		}

		Base.localScale = Vector3.one * Stop;

		if(!outwards) Base.gameObject.SetActive(false);

		lerpCount--;
		if(lerpCount == 0) lerping = false;
	}

	IEnumerator ScalePole(bool outwards) {
		lerpCount++;

		//if(outwards) yield return new WaitForSeconds(BasePopTime);

		Pole.gameObject.SetActive(true);

		float Start = outwards ? 0f : PanelTopHeight - 0.1f;
		float Stop = outwards ? PanelTopHeight - 0.1f : 0f;

		for(float i = 0; i < 1f; i += Time.deltaTime / PoleScaleTime) {
			Pole.localScale = new Vector3(1f, Mathf.Lerp(Start, Stop, i), 1f);
			yield return null;
		}

		Pole.localScale = new Vector3(1f, Stop, 1f);

		if(!outwards) Pole.gameObject.SetActive(false);

		lerpCount--;
		if(lerpCount == 0) lerping = false;
	}

	IEnumerator ScalePanel(bool outwards) {
		lerpCount++;

		//if(outwards) yield return new WaitForSeconds((PoleScaleTime - PanelScaleTime));

		Panel.gameObject.SetActive(true);
		whirr.Play();

		float Start = outwards ? 0f : 1f;
		float Stop = outwards ? 1f : 0f;

		for(float i = 0; i < 1f; i += Time.deltaTime / PanelScaleTime) {
			Panel.localScale = Vector3.one * Mathf.Lerp(Start, Stop, i);
			Panel.localPosition = new Vector3(0f, Mathf.Lerp(Start, Stop, i) * PanelTopHeight, 0f);
			//Panel.localRotation = Quaternion.Euler(0f, Mathf.Lerp(0f, 360f, lerpCubic(i)), 0f);
			yield return null;
		}

		Panel.localScale = Vector3.one * Stop;
		Panel.localPosition = new Vector3(0f, Stop * PanelTopHeight, 0f);
		//Panel.localRotation = Quaternion.Euler(0f, 0f, 0f);

		if(!outwards) Panel.gameObject.SetActive(false);

		lerpCount--;
		if(lerpCount == 0) lerping = false;
	}

	float lerpCubic(float t) {
		if(t < 0.5f) return 4f * t * t * t;
		t--;
		return 4f * t * t * t + 1f;
	}
}
