using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDisplayCheck : MonoBehaviour {

	public Color StateOneA;
	public Color StateOneB;

	public Color StateTwoA;
	public Color StateTwoB;

	public bool IsStateOne = true;

	MeshRenderer MyRenderer;

	void Start () {
		MyRenderer = GetComponent<MeshRenderer>();
	}

	public void SetState(int State) {
		if(State == 0) IsStateOne = true;
		else IsStateOne = false;

		StartCoroutine(PulseSize());
		SetColor(0f);
	}

	public void SetColor(float LerpValue) {
		LerpValue = Mathf.Clamp01(LerpValue);

		if(IsStateOne) {
			MyRenderer.material.color = Color.Lerp(StateOneA, StateOneB, LerpValue);
		} else {
			MyRenderer.material.color = Color.Lerp(StateTwoA, StateTwoB, LerpValue);
		}
	}

	IEnumerator PulseSize() {
		Vector3 StartScale = transform.localScale;
		Vector3 EndScale = transform.localScale;
		EndScale.y *= 1.5f;

		for(float i = 0; i < 1f; i += Time.deltaTime / 0.5f) {
			transform.localScale = Vector3.Lerp(StartScale, EndScale, i);
			yield return null;
		}
		
		for(float i = 0; i < 1f; i += Time.deltaTime / 0.5f) {
			transform.localScale = Vector3.Lerp(EndScale, StartScale, i);
			yield return null;
		}

		transform.localScale = StartScale;
	}
}
