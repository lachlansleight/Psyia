using System.Collections;
using System.Collections.Generic;
using Psyia;
using UCTK;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using XRP;

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
		public TextAsset PresetJson;
		public ModeSettingsPanel Panel;
		public Transform Pillar;
	}

	public Meditation Meditation;
	public MeditationTriggers MeditationTriggers;
	
	[Header("Fader")]
	public GameObject FaderObject;
	public float FlashFadeTime = 1f;
	private Material _faderMaterial;

	public Color DefaultPsyiaColor = new Color(1f, 1f, 1f, 0.25f);

	public ComputeRenderer PsyiaRenderer;
	public DispatchQueue PsyiaDispatcher;
	
	public PsyiaSettingsApplicator SettingsApplicator;
	public PsyiaJsonUiSetter UiSettingsApplicator;

	public ModeIntroCanvas IntroCanvas;

	public Mode[] Modes;

	public PsyiaMusic Music;

	public Transform LeftController;
	public Transform RightController;
	public GameObject LeftControllerBase;
	public GameObject RightControllerBase;
	public ControllerHaptics LeftHaptics;
	public ControllerHaptics RightHaptics;

	public MenuToggler MenuToggler;

	public GameObject Room;
	public Material[] RoomMaterials;

	public MenuLoop MenuMusic;
	
	public void Start()
	{
		_faderMaterial = FaderObject.GetComponent<Renderer>().material;

		foreach (var m in Modes) m.TouchSphere.transform.localScale = Vector3.zero;
		ResetTouchSphereScales();

		ResetObjects();

		StartCoroutine(FirstOpen());
	}

	public IEnumerator FirstOpen()
	{
		yield return new WaitForSeconds(1f);
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

		StartCoroutine(RunModeRoutineNew(targetMode));
	}

	public void ResetObjects()
	{
		Music.Reset();
		PsyiaRenderer.enabled = false;
		PsyiaDispatcher.RunOnUpdate = false;
		
		IntroCanvas.CanvasGroup.alpha = 0f;
		IntroCanvas.gameObject.SetActive(false);
		
		FaderObject.SetActive(false);
		LeftController.gameObject.SetActive(false);
		RightController.gameObject.SetActive(false);
		
		
		LeftHaptics.enabled = false;
		RightHaptics.enabled = false;
		
		LeftControllerBase.SetActive(true);
		RightControllerBase.SetActive(true);
		
		MenuToggler.SetTargetValue(false);
		MenuToggler.AllowMenuToggle = false;
	}

	public IEnumerator ResetEverything()
	{
		ResetObjects();

		StartCoroutine(FadeRoom(true, Modes[0].TouchSphere.transform.position, 5f));

		ResetTouchSphereScales();

		yield break;
	}

	private IEnumerator LerpPillar(Transform target, float fromHeight, float toHeight, float duration)
	{
		var startScale = target.localScale;
		var endScale = target.localScale;
		startScale.y = fromHeight;
		endScale.y = toHeight;

		var startPosition = target.position;
		var endPosition = target.position;
		startPosition.y = fromHeight / 2f - 0.01f;
		endPosition.y = toHeight / 2f - 0.01f;
		
		for (var i = 0f; i < 1f; i += Time.deltaTime / duration) {
			target.localScale = Vector3.Lerp(startScale, endScale, i);
			target.position = Vector3.Lerp(startPosition, endPosition, i);
			yield return null;
		}

		target.localScale = endScale;
		target.position = endPosition;
	}

	private void ResetTouchSphereScales()
	{
		foreach (var m in Modes) {
			//Debug.Log(m.TouchSphere.name + " requires " + m.TouchSphere.UsageCountRequirement + " vs actual of " + SaveGameInterface.Main.PlayCount);
			if (m.TouchSphere.UsageCountRequirement == SaveGameInterface.Main.PlayCount) {
				//Debug.Log("Up slow");
				m.TouchSphere.gameObject.SetActive(true);
				m.TouchSphere.LerpToScale(1f, 2f);
				StartCoroutine(LerpPillar(m.Pillar, 0f, 0.7f, 3f));
			} else if (m.TouchSphere.UsageCountRequirement < SaveGameInterface.Main.PlayCount) {
				//Debug.Log("Up fast");
				m.TouchSphere.gameObject.SetActive(true);
				m.TouchSphere.LerpToScale(1f, 0.3f);
				
				m.Panel.gameObject.SetActive(true);
				m.Panel.LerpToScale(1f, 0.3f);

				StartCoroutine(LerpPillar(m.Pillar, 0f, 0.7f, 0.6f));
			} else {
				m.TouchSphere.gameObject.SetActive(false);
			}
		}
	}

	private void ScaleTouchSpheres(float targetValue, float duration, bool disableAfterScale = false)
	{
		foreach (var m in Modes) {
			if(m.TouchSphere.gameObject.activeSelf)
				m.TouchSphere.LerpToScale(targetValue, duration, disableAfterScale);
			
			if (m.Panel.gameObject.activeSelf) {
				m.Panel.LerpToScale(targetValue, duration, disableAfterScale);
			}
		}
	}

	private IEnumerator FadeCamera(Color color)
	{
		//fade out
		FaderObject.SetActive(true);

		_faderMaterial.SetColor("_Color", color);
		
		for (var i = 0f; i < 1f; i += Time.deltaTime / FlashFadeTime) {
			color.a = 1f - i;
			_faderMaterial.SetColor("_Color", color);
			yield return null;
		}

		FaderObject.SetActive(false);
	}

	public void EnableIntroCanvas(Mode targetMode, bool showInfiniteMode)
	{
		IntroCanvas.gameObject.SetActive(true);
		IntroCanvas.SetPositionInstantly();
		var text = targetMode.ArtistName;
		if (showInfiniteMode) {
			if (targetMode.Panel.InfiniteModeToggle.CurrentValue)
				text += "\n\nInfinite mode - hold the menu button to return";
		}

		IntroCanvas.SetTexts(targetMode.SongName, text, targetMode.Tip);
	}

	public void SetControllerEnabled(bool value)
	{
		LeftController.gameObject.SetActive(value);
		RightController.gameObject.SetActive(value);
		LeftHaptics.enabled = value;
		RightHaptics.enabled = value;
		MenuToggler.AllowMenuToggle = value;
	}

	public IEnumerator FadeControllers(bool up, float duration)
	{
		for (var i = 0f; i < 1f; i += Time.deltaTime / duration) {
			var iL = 0f;
			if (i < 0.5f) iL = 4f * i * i * i;
			iL = (i - 1f);
			iL = 4f * iL * iL * iL + 1f;
			
			if (!up) iL = 1f - iL;
			
			LeftController.localScale = new Vector3(iL, iL, 1f);
			RightController.localScale = new Vector3(iL, iL, 1f);
			MeditationTriggers.GlobalSizeMultiplier = 1f - iL;
			yield return null;
		}
		
		MeditationTriggers.GlobalSizeMultiplier = up ? 0f : 1f;
		LeftController.localScale = up ? Vector3.one : new Vector3(0f, 0f, 1f);
		RightController.localScale = up ? Vector3.one : new Vector3(0f, 0f, 1f);
	}

	public IEnumerator FadeRoom(bool fadeIn, Vector3 fadeCenterPos, float duration)
	{
		if (fadeIn) {
			Room.SetActive(true);
		}
		
		foreach (var m in RoomMaterials) {
			m.SetVector("_FadeCenter", fadeCenterPos);
		}
		
		for (var i = 0f; i < 1f; i += Time.deltaTime / duration) {
			foreach (var m in RoomMaterials) {
				m.SetFloat("_FadeDistance", Mathf.Lerp(fadeIn ? 12f : 0f, fadeIn ? 0f : 12f, i));
			}

			yield return null;
		}

		if (!fadeIn) {
			Room.SetActive(false);
		}
	}

	public IEnumerator RunModeRoutineNew(Mode targetMode)
	{
		//remove spheres
		ScaleTouchSpheres(0f, 0.3f, true);
		
		//show controllers
		SetControllerEnabled(true);
		StartCoroutine(FadeControllers(true, 0.5f));
		
		//apply settings
		SettingsApplicator.TestJson = targetMode.PresetJson;
		UiSettingsApplicator.LoadFromJson(targetMode.PresetJson.text);
		//SettingsApplicator.ApplyTestJson();
		
		//turn on system
		PsyiaRenderer.enabled = true;
		PsyiaDispatcher.RunOnUpdate = true;
		PsyiaRenderer.RenderMaterial.color = DefaultPsyiaColor;
		
		//emit particles
		targetMode.Emitter.transform.position = targetMode.TouchSphere.transform.position;
		yield return null;
		targetMode.Emitter.ResetVelocity();
		yield return null;
		targetMode.Emitter.Emit(SettingsApplicator.CurrentSettings.System.MaxParticleCount * 1024);

		//fade out menu music
		MenuMusic.FadeOut(1f);

		//fade out room
		StartCoroutine(FadeRoom(false, targetMode.TouchSphere.transform.position, 0.5f));
		
		//set music
		Music.SetClip(targetMode.Song);
		Music.AutoPlay = targetMode.Panel.InfiniteModeToggle.CurrentValue;

		//play or resume
		if (AudioData.Instance.AudioDetected) {
			Music.AutoPlay = true;
		} else {
			Music.PlayPause();
		}
		
		//increment play count
		SaveGameInterface.Main.PlayCount++;
		
		//wait for end of song or wait for the user to exit
		//note - this means that if Auto Play is on, the only way back is using the menu button!
		while (Music.TimeInTrack < targetMode.Song.length - 5f || Music.AutoPlay) {
			yield return null;
		}
		
		//disable psyia system
		PsyiaRenderer.RenderMaterial.color = new Color(0f, 0f, 0f, 0f);
		PsyiaRenderer.enabled = false;
		PsyiaDispatcher.RunOnUpdate = false;

		//disable controllers + controller models
		//also show meditation triggers
		yield return StartCoroutine(FadeControllers(false, 0.5f));

		//fade in menu music
		MenuMusic.FadeIn(1f);
		
		//return to menu
		ReturnToMenu();
	}

	public IEnumerator RunModeRoutine(Mode targetMode)
	{
		//scale down touch spheres and panels
		ScaleTouchSpheres(0f, 0.3f, true);

		//fade camera
		//yield return StartCoroutine(FadeCamera(targetMode.TouchSphere.Color));

		//fade intro canvas in and out
		EnableIntroCanvas(targetMode, true);
		IntroCanvas.Fade(1f, 1f);
		yield return new WaitForSeconds(5f);
		IntroCanvas.Fade(0f, 1f, true);

		//enable controllers + controller models
		//also get rid of meditation triggers
		SetControllerEnabled(true);
		yield return StartCoroutine(FadeControllers(true, 0.5f));

		//apply chosen settings to psyia system
		SettingsApplicator.TestJson = targetMode.PresetJson;
		UiSettingsApplicator.LoadFromJson(targetMode.PresetJson.text);
		//SettingsApplicator.ApplyTestJson();
		
		//enable psyia system
		PsyiaRenderer.enabled = true;
		PsyiaDispatcher.RunOnUpdate = true;
		PsyiaRenderer.RenderMaterial.color = DefaultPsyiaColor;
		
		//emit particles
		targetMode.Emitter.transform.position = targetMode.TouchSphere.transform.position;
		yield return null;
		targetMode.Emitter.ResetVelocity();
		yield return null;
		targetMode.Emitter.Emit(SettingsApplicator.CurrentSettings.System.MaxParticleCount * 1024);
		
		//start music
		Music.SetClip(targetMode.Song);
		Music.AutoPlay = targetMode.Panel.InfiniteModeToggle.CurrentValue;

		if (AudioData.Instance.AudioDetected) {
			Music.AutoPlay = true;
		} else {
			Debug.Log(Music.IsPlaying);
			Music.PlayPause();
			Debug.Log(Music.IsPlaying);
		}
		
		//fade in psyia system
//		for (var i = 0f; i <= 1f; i += Time.deltaTime / 2f) {
//			PsyiaRenderer.RenderMaterial.color = Color.Lerp(new Color(0f, 0f, 0f, 0f), DefaultPsyiaColor, i);
//			yield return null;
//		}

		//increment play count
		SaveGameInterface.Main.PlayCount++;
		
		//wait for end of song or wait for the user to exit
		//note - this means that if Auto Play is on, the only way back is using the menu button!
		while (Music.TimeInTrack < targetMode.Song.length - 5f || Music.AutoPlay) {
			yield return null;
		}
		
		//show intro canvas
		EnableIntroCanvas(targetMode, false);
		IntroCanvas.Fade(1f, 2f);

		//fade out psyia system
		for (var i = 0f; i <= 1f; i += Time.deltaTime / 5f) {
			PsyiaRenderer.RenderMaterial.color = Color.Lerp(DefaultPsyiaColor, new Color(0f, 0f, 0f, 0f), i);
			yield return null;
		}

		//disable psyia system
		PsyiaRenderer.RenderMaterial.color = new Color(0f, 0f, 0f, 0f);
		PsyiaRenderer.enabled = false;
		PsyiaDispatcher.RunOnUpdate = false;
		
		//wait
		yield return new WaitForSeconds(2f);
		
		//fade out canvas
		IntroCanvas.Fade(0f, 1f, true);

		//disable controllers + controller models
		//also show meditation triggers
		yield return StartCoroutine(FadeControllers(false, 0.5f));

		//return to menu
		ReturnToMenu();
	}

	[ContextMenu("StartMeditation")]
	public void StartMeditation()
	{
		Debug.Log("Starting meditation");
		StartCoroutine(RunMeditationRoutine());
	}

	public IEnumerator RunMeditationRoutine()
	{
		FaderObject.SetActive(true);

		var color = Color.black;
		_faderMaterial.SetColor("_Color", color);

		foreach (var m in Modes) {
			if(m.TouchSphere.gameObject.activeSelf)
				m.TouchSphere.LerpToScale(0f, 0.3f, true);
			
			if (m.Panel.gameObject.activeSelf) {
				m.Panel.LerpToScale(0f, 0.3f, true);
			}
		}
		
		for (var i = 0f; i < 1f; i += Time.deltaTime / FlashFadeTime) {
			color.a = 1f - i;
			_faderMaterial.SetColor("_Color", color);
			yield return null;
		}

		FaderObject.SetActive(false);

		IntroCanvas.gameObject.SetActive(true);
		IntroCanvas.SetPositionInstantly();
		var text = "Suggested duration: 20 minutes";
		text += "\n\nInfinite mode - hold the menu button to return";
		IntroCanvas.SetTexts("Meditation", text, "");
		IntroCanvas.Fade(1f, 1f);

		yield return new WaitForSeconds(5f);

		IntroCanvas.Fade(0f, 1f, true);

		MenuToggler.AllowMenuToggle = true;

		for (var i = 0f; i < 1f; i += Time.deltaTime / 0.5f) {
			var iL = 0f;
			if (i < 0.5f) iL = 4f * i * i * i;
			iL = (i - 1f);
			iL = 4f * iL * iL * iL + 1f;
			LeftController.localScale = new Vector3(iL, iL, 1f);
			RightController.localScale = new Vector3(iL, iL, 1f);
			MeditationTriggers.GlobalSizeMultiplier = 1f - i;
		}

		MeditationTriggers.GlobalSizeMultiplier = 0f;
		LeftController.localScale = Vector3.one;
		RightController.localScale = Vector3.one;

		SettingsApplicator.TestJson = Meditation.PresetJson;
		UiSettingsApplicator.LoadFromJson(Meditation.PresetJson.text);
		//SettingsApplicator.ApplyTestJson();
		PsyiaRenderer.RenderMaterial.color = new Color(0f, 0f, 0f, 0f);
		PsyiaRenderer.enabled = true;
		PsyiaDispatcher.RunOnUpdate = true;
		Meditation.Emitter.transform.position = new Vector3(0f, 1.5f, 1f);
		Meditation.Emitter.Emit(SettingsApplicator.CurrentSettings.System.MaxParticleCount * 1024);

		Meditation.BeginMeditation();
		
		for (var i = 0f; i <= 1f; i += Time.deltaTime / 5f) {
			PsyiaRenderer.RenderMaterial.color = Color.Lerp(new Color(0f, 0f, 0f, 0f), DefaultPsyiaColor, i);
			yield return null;
		}

		//note - this means that if Auto Play is on, the only way back is using the menu button!
		var duration = 0f;
		while (duration < (60f * 20f)) {
			duration += Time.deltaTime;
			yield return null;
		}
		
		IntroCanvas.gameObject.SetActive(true);
		IntroCanvas.SetPositionInstantly();
		IntroCanvas.SetTexts("Welcome back", "", "");
		IntroCanvas.Fade(1f, 2f);

		for (var i = 0f; i <= 1f; i += Time.deltaTime / 5f) {
			PsyiaRenderer.RenderMaterial.color = Color.Lerp(DefaultPsyiaColor, new Color(0f, 0f, 0f, 0f), i);
			yield return null;
		}

		PsyiaRenderer.enabled = false;
		PsyiaDispatcher.RunOnUpdate = false;
		
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
			MeditationTriggers.GlobalSizeMultiplier = i;
		}

		MeditationTriggers.GlobalSizeMultiplier = 0f;
		LeftController.localScale = Vector3.zero;
		RightController.localScale = Vector3.zero;

		ReturnToMenu();
	}

	public void ReturnToMenu()
	{
		StopAllCoroutines();
		if (Meditation.Running) Meditation.StopMeditation();
		StartCoroutine(ResetEverything());
	}
}
