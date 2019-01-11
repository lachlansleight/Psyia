using System.Collections;
using System.Collections.Generic;
using UCTK;
using UnityEngine;
using UnityEngine.UI;

public class ModeChoreography : MonoBehaviour
{

	public class Mode
	{
		public TouchSphere TouchSphere;
		public AudioClip Song;
		public string SongName;
		public string ArtistName;
		public string Tip;
		//TODO: Implement this
		//public SystemPreset Preset;
	}

	[Header("Fader")]
	public GameObject FaderObject;
	public float FlashFadeTime = 1f;
	private Material _faderMaterial;

	public ComputeRenderer PsyiaRenderer;

	public ModeIntroCanvas IntroCanvas;

	public Mode[] Modes;
	
	public void Awake()
	{
		_faderMaterial = FaderObject.GetComponent<Renderer>().material;
		IntroCanvas.CanvasGroup.alpha = 0f;
		IntroCanvas.gameObject.SetActive(false);

		PsyiaRenderer.enabled = false;
	}

	public void Update()
	{
		
	}

	public void SetMode(TouchSphere sourceSphere)
	{
		Mode targetMode = null;
		foreach (var m in Modes) {
			if (m.TouchSphere != sourceSphere) continue;
			
			targetMode = m;
			break;
		}

		if (targetMode == null) {
			throw new System.Exception("Didn't find sourceSphere " + sourceSphere.gameObject.name + " in mode list");
		}

		StartCoroutine(RunModeRoutine(targetMode));
	}

	public IEnumerator RunModeRoutine(Mode targetMode)
	{
		FaderObject.SetActive(true);

		var color = targetMode.TouchSphere.Color;
		_faderMaterial.SetColor("_Color", color);
		
		for (var i = 0f; i < 1f; i += Time.deltaTime / FlashFadeTime) {
			color.a = 1f - i;
			_faderMaterial.SetColor("_Color", color);
			yield return null;
		}

		FaderObject.SetActive(false);

		IntroCanvas.gameObject.SetActive(true);
		IntroCanvas.SetPositionInstantly();
		IntroCanvas.SetTexts(targetMode.SongName, targetMode.ArtistName, targetMode.Tip);
		IntroCanvas.Fade(1f, 1f);
	}
}
