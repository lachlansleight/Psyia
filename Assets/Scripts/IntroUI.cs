﻿using UnityEngine;
using System.Collections;
using VRTools.UI;

public class IntroUI : MonoBehaviour {

	public VRUI_Panel uiPanel;

	private bool changingScene = false;

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetInt("NumberPlays") < 2) Destroy(gameObject);

		StartCoroutine(DelayedPop());

		uiPanel.GetControl("EnterMeditationButton").OnPressUp += EnterMeditationSelected;
		uiPanel.GetControl("EnterStarLabButton").OnPressUp += EnterStarLabSelected;
		uiPanel.GetControl("MeditationPostureSlider").OnIntChange += MeditationPostureChanged;

		for(int i = 0; i < uiPanel.controls.Length; i++) uiPanel.controls[i].SetDefaultValue();
	}
	
	public void EnterMeditationSelected() {
		if(changingScene) return;
		changingScene = true;
		StartCoroutine(ChangeScene("Meditation"));
	}

	public void EnterStarLabSelected() {
		if(changingScene) return;
		changingScene = true;
		StartCoroutine(ChangeScene("StarLab"));
	}

	public void MeditationPostureChanged(int newValue) {
		PsyiaSettings.MeditationPosture = newValue;
	}

	public void FirstTimeChanged(bool newValue) {
		PsyiaSettings.FirstTime = newValue;
	}

	IEnumerator ChangeScene(string sceneName) {
		GameObject.Find("PsyiaStars").GetComponent<StarIntro>().compute.SetInt("customMode", 1);
		GameObject.Find("PsyiaStars").GetComponent<StarIntro>().compute.SetFloat("burstStartTime", Time.time);
		GameObject.Find("UIMechanism").GetComponent<UIPanelMechanism>().Pop();
		yield return new WaitForSeconds(5f);
		GameObject.Find("Fader").GetComponent<Fader>().FadeOut();
		yield return new WaitForSeconds(2f);
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
	}

	IEnumerator DelayedPop() {
		yield return new WaitForSeconds(2f);
		GameObject.Find("UIMechanism").GetComponent<UIPanelMechanism>().Pop();
	}
}
