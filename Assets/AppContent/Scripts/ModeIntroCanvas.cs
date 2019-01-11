using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeIntroCanvas : MonoBehaviour
{

	public Transform Target;
	public float Distance = 2f;
	public float LerpSpeed = 0.1f;

	public CanvasGroup CanvasGroup;
	public Text SongTitleText;
	public Text ArtistNameText;
	public Text TipText;

	public void SetTexts(string songTitle, string artistName, string hint)
	{
		SongTitleText.text = songTitle;
		ArtistNameText.text = artistName;
		TipText.text = hint;
	}
	
	public void Update()
	{
		transform.position = Vector3.Slerp(transform.position - Target.position, Target.forward * Distance, LerpSpeed);
		transform.position += Target.position;

		transform.rotation = Quaternion.LookRotation(transform.position - Target.position, Vector3.up);
	}

	public void SetPositionInstantly()
	{
		transform.position = Vector3.Slerp(transform.position - Target.position, Target.forward * Distance, 1f);
		transform.position += Target.position;

		transform.rotation = Quaternion.LookRotation(transform.position - Target.position, Vector3.up);
	}

	public void Fade(float target, float fadeTime, bool disableAtEnd = false)
	{
		StartCoroutine(FadeRoutine(CanvasGroup.alpha, target, fadeTime, disableAtEnd));
	}

	public IEnumerator FadeRoutine(float from, float to, float duration, bool disableAtEnd = false)
	{
		for (var i = 0f; i <= 1f; i += Time.deltaTime / duration) {
			CanvasGroup.alpha = Mathf.Lerp(from, to, i);
			yield return null;
		}

		CanvasGroup.alpha = to;

		if (disableAtEnd) gameObject.SetActive(false);
	}
}
