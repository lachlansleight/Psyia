using UnityEngine;
using System.Collections;
using VRTools.UI;

public class IntroUI : MonoBehaviour {

	public VRUI_Panel uiPanel;

	private bool changingScene = false;

	// Use this for initialization
	void Start () {

		if(!PlayerPrefs.HasKey("NumberPlays")) PlayerPrefs.SetInt("NumberPlays", 1);

		else PlayerPrefs.SetInt("NumberPlays", PlayerPrefs.GetInt("NumberPlays") + 1);

		if(Input.GetKeyDown(KeyCode.T)) PlayerPrefs.SetInt("NumberPlays", 1);

		StartCoroutine(DelayedPop());

		(uiPanel.GetControl("FirstTimeToggle") as VRUI_Toggle).Toggled = PlayerPrefs.GetInt("NumberPlays") == 1;


		uiPanel.GetControl("EnterMeditationButton").OnPressUp += EnterMeditationSelected;
		uiPanel.GetControl("EnterStarLabButton").OnPressUp += EnterStarLabSelected;
		uiPanel.GetControl("MeditationPostureSlider").OnIntChange += MeditationPostureChanged;
		uiPanel.GetControl("FirstTimeToggle").OnBooleanChange += FirstTimeChanged;

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
		yield return new WaitForSeconds(PlayerPrefs.GetInt("NumberPlays") > 1 ? 2f : 15f);
		GameObject.Find("UIMechanism").GetComponent<UIPanelMechanism>().Pop();
	}
}
