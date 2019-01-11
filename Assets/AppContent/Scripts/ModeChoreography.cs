using System.Collections;
using System.Collections.Generic;
using Psyia;
using UCTK;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class ModeChoreography : MonoBehaviour
{
	[System.Serializable]
	public class Mode
	{
		public TouchSphere TouchSphere;
		public AudioClip Song;
		public string SongName;
		public string ArtistName;
		public string Tip;
		public PsyiaEmitter Emitter;
		//TODO: Implement this
		//public SystemPreset Preset;
	}

	[Header("Fader")]
	public GameObject FaderObject;
	public float FlashFadeTime = 1f;
	private Material _faderMaterial;

	public Color DefaultPsyiaColor = new Color(1f, 1f, 1f, 0.25f);

	public ComputeRenderer PsyiaRenderer;

	public ModeIntroCanvas IntroCanvas;

	public Mode[] Modes;

	public AudioSource ModeAudio;

	public Transform LeftController;
	public Transform RightController;
	public ControllerHaptics LeftHaptics;
	public ControllerHaptics RightHaptics;
	
	public SteamVR_Action_Boolean ToggleMenuAction;
	public SteamVR_Input_Sources Hand;
	
	public void Awake()
	{
		_faderMaterial = FaderObject.GetComponent<Renderer>().material;
		IntroCanvas.CanvasGroup.alpha = 0f;
		IntroCanvas.gameObject.SetActive(false);

		LeftController.gameObject.SetActive(false);
		RightController.gameObject.SetActive(false);
		

		PsyiaRenderer.enabled = false;
	}
	
	public void OnEnable ()
	{
		ToggleMenuAction.AddOnChangeListener(OnAxisValueChanged, Hand);
	}

	public void OnDisable()
	{
		ToggleMenuAction.RemoveOnChangeListener(OnAxisValueChanged, Hand);
	}

	public void Update()
	{
		
	}
	
	private void OnAxisValueChanged(SteamVR_Action_In actionIn)
	{
		if (!(actionIn is SteamVR_Action_Boolean)) return;
		var asBoolean = (SteamVR_Action_Boolean) actionIn;
		var pressDown = asBoolean.GetStateDown(Hand);
		
		if (!pressDown) return;
		StopAllCoroutines();
		StartCoroutine(ResetEverything());
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

	public IEnumerator ResetEverything()
	{
		ModeAudio.Stop();
		IntroCanvas.CanvasGroup.alpha = 0f;
		IntroCanvas.gameObject.SetActive(false);
		FaderObject.SetActive(false);
		LeftController.gameObject.SetActive(false);
		RightController.gameObject.SetActive(false);
		LeftHaptics.enabled = false;
		RightHaptics.enabled = false;
		PsyiaRenderer.enabled = false;
		
		foreach (var m in Modes) {
			m.TouchSphere.gameObject.SetActive(true);
			m.TouchSphere.LerpToScale(1f, 0.3f);
			yield return null;
		}
	}

	public IEnumerator RunModeRoutine(Mode targetMode)
	{
		FaderObject.SetActive(true);

		var color = targetMode.TouchSphere.Color;
		_faderMaterial.SetColor("_Color", color);

		foreach (var m in Modes) {
			m.TouchSphere.LerpToScale(0f, 0.3f, true);
		}
		
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

		yield return new WaitForSeconds(5f);

		IntroCanvas.Fade(0f, 1f, true);

		LeftController.gameObject.SetActive(true);
		RightController.gameObject.SetActive(true);
		LeftHaptics.enabled = true;
		RightHaptics.enabled = true;

		for (var i = 0f; i < 1f; i += Time.deltaTime / 0.5f) {
			var iL = 0f;
			if (i < 0.5f) iL = 4f * i * i * i;
			iL = (i - 1f);
			iL = 4f * iL * iL * iL + 1f;
			LeftController.localScale = new Vector3(iL, iL, 1f);
			RightController.localScale = new Vector3(iL, iL, 1f);
		}

		LeftController.localScale = Vector3.one;
		RightController.localScale = Vector3.one;
		
		//TODO: Implement this
		//SetSystemVariables(targetMode.Preset);
		//ResetSystem
		PsyiaRenderer.RenderMaterial.color = DefaultPsyiaColor;
		PsyiaRenderer.enabled = true;
		targetMode.Emitter.Emit(targetMode.Emitter.StartEmitCount);
		
		ModeAudio.clip = targetMode.Song;
		ModeAudio.Play();

		while (ModeAudio.time < ModeAudio.clip.length - 5f) {
			yield return null;
		}
		
		IntroCanvas.gameObject.SetActive(true);
		IntroCanvas.SetPositionInstantly();
		IntroCanvas.SetTexts(targetMode.SongName, targetMode.ArtistName, targetMode.Tip);
		IntroCanvas.Fade(1f, 2f);

		for (var i = 0f; i <= 1f; i += Time.deltaTime / 5f) {
			PsyiaRenderer.RenderMaterial.color = Color.Lerp(DefaultPsyiaColor, new Color(0f, 0f, 0f, 0f), i);
			yield return null;
		}

		PsyiaRenderer.enabled = false;
		
		yield return new WaitForSeconds(2f);
		
		IntroCanvas.Fade(0f, 1f, true);

		for (var i = 0f; i < 1f; i += Time.deltaTime / 0.5f) {
			var iL = 0f;
			if (i < 0.5f) iL = 4f * i * i * i;
			iL = (i - 1f);
			iL = 4f * iL * iL * iL + 1f;
			
			iL = 1f - iL;
			LeftController.localScale = new Vector3(iL, iL, 1f);
			RightController.localScale = new Vector3(iL, iL, 1f);
		}

		LeftController.localScale = Vector3.zero;
		RightController.localScale = Vector3.zero;
		LeftController.gameObject.SetActive(false);
		RightController.gameObject.SetActive(false);
		LeftHaptics.enabled = false;
		RightHaptics.enabled = false;

		foreach (var m in Modes) {
			m.TouchSphere.gameObject.SetActive(true);
			m.TouchSphere.LerpToScale(1f, 0.3f);
		}
	}
}
