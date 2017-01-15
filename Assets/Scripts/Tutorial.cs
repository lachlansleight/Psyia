using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTools;

public class Tutorial : MonoBehaviour {

	public bool firstTime;
	public bool secondTime;

	public bool[] completed = new bool[] {false, false, false, false } ;
	public bool[] active = new bool[] {false, false, false, false };

	public string[] controlNames = new string[] {"Trigger", "Touchpad", "Grip", "Menu"};
	[Tooltip("0,1,2,3 = trigger,touchpad,grip,menu")] public Renderer[] controllerRenderers = new Renderer[] {null, null, null, null };
	[Tooltip("0,1,2,3 = trigger,touchpad,grip,menu")] public Renderer[] lineRenderers = new Renderer[] {null, null, null, null };
	[Tooltip("0,1,2,3 = trigger,touchpad,grip,menu")] public Text[] texts = new Text[] {null, null, null, null };

	const int TRIGGER = 0;
	const int TOUCHPAD = 1;
	const int GRIP = 2;
	const int MENU = 3;

	public string[] DeviceName = new string[] {"ViveLeft", "ViveRight" };

	//public Renderer[] 

	// Use this for initialization
	void Start () {
		firstTime = PlayerPrefs.GetInt("NumberPlays") == 1;
		secondTime = PlayerPrefs.GetInt("NumberPlays") == 2;

		if(firstTime) {
			InstantCompleteControl(MENU);
		} else if(secondTime) {
			InstantCompleteControl(TRIGGER);
			InstantCompleteControl(TOUCHPAD);
			InstantCompleteControl(GRIP);
		} else {
			InstantCompleteControl(TRIGGER);
			InstantCompleteControl(TOUCHPAD);
			InstantCompleteControl(GRIP);
			InstantCompleteControl(MENU);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//once all complete, we can destroy the tutorial object
		if(completed[0] && completed[1] && completed[2] && completed[3]) Destroy(gameObject);

		for(int j = 0; j < DeviceName.Length; j++) {
			for(int i = 0; i < controlNames.Length; i++) {
				if(!completed[i] && !active[i] && VRInput.GetDevice(DeviceName[j]).GetButtonDown(controlNames[i])) {
					StartCoroutine(CompleteControl(i));
				}
			}
		}
	}

	IEnumerator CompleteControl(int index) {
		if(index < 0 || index > 3) yield break;

		active[index] = true;

		Renderer controllerMaterial = controllerRenderers[index];
		Renderer lineMaterial = lineRenderers[index];
		Text completedText = texts[index];

		Color StartCol = Color.white;
		Color EndCol = Color.black;
		Color EndColTransp = new Color(1f, 1f, 1f, 0f);

		bool first = true;

		for(float i = 0; i < 1f; i += Time.deltaTime / 1f) {
			controllerMaterial.material.SetColor("_EmissionColor", Color.Lerp(StartCol, EndCol, i));
			lineMaterial.material.color = Color.Lerp(StartCol, EndColTransp, i);
			completedText.color = Color.Lerp(StartCol, EndColTransp, i);

			if(first) {
				first = false;
				yield return new WaitForSeconds(0.25f);
			}

			yield return null;
		}

		controllerMaterial.material.SetColor("_EmissionColor", Color.black);
		lineMaterial.gameObject.SetActive(false);
		completedText.gameObject.SetActive(false);

		completed[index] = true;
		active[index] = false;
	}

	void InstantCompleteControl(int index) {
		Renderer controllerMaterial = controllerRenderers[index];
		Renderer lineMaterial = lineRenderers[index];
		Text completedText = texts[index];

		controllerMaterial.material.SetColor("_EmissionColor", Color.black);
		lineMaterial.gameObject.SetActive(false);
		completedText.gameObject.SetActive(false);

		completed[index] = true;
		active[index] = false;
	}
}
