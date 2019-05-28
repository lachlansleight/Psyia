using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoop : MonoBehaviour
{

	public AudioSource Audio;

	private bool _fading;

	public void Start()
	{
		FadeIn(5f);
	}

	public void FadeIn(float duration = 2f)
	{
		if (_fading) return;
		
		StartCoroutine(Fade(Audio.volume, 1f, duration));
	}

	public void FadeOut(float duration = 2f)
	{
		if (_fading) return;
		
		StartCoroutine(Fade(Audio.volume, 0f, duration));
	}

	public void FadeTo(float target, float duration = 2f)
	{
		if (_fading) return;

		StartCoroutine(Fade(Audio.volume, target, duration));
	}

	public IEnumerator Fade(float from, float to, float duration)
	{
		_fading = true;
		
		for (var i = 0f; i < 1f; i += Time.deltaTime / duration) {
			Audio.volume = Mathf.Lerp(from, to, i);
			yield return null;
		}

		Audio.volume = to;

		_fading = false;
	}
}
