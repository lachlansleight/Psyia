using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRUI_Audio : MonoBehaviour {

	public static float uiVolume = 0.5f;
	static Dictionary<string, AudioClip> clips;
	static Object prefab;

	static void BuildDictionary() {
		clips = new Dictionary<string, AudioClip>();
		clips.Add("PressDown", Resources.Load("VRUI_PressDown") as AudioClip);
		clips.Add("PressUp", Resources.Load("VRUI_PressDown") as AudioClip);

		prefab = Resources.Load("VRUI_AudioElement");
	}

	public static void PlayPressDown(Vector3 position) {
		if(clips == null) BuildDictionary();

		GameObject newObject = Instantiate(prefab) as GameObject;
		newObject.transform.position = position;
		newObject.name = "VRUI_PressDownClip";

		newObject.GetComponent<VRUI_AudioSound>().Initialise(clips["PressDown"], uiVolume);
	}

	public static void PlayPressUp(Vector3 position) {
		if(clips == null) BuildDictionary();

		GameObject newObject = Instantiate(prefab) as GameObject;
		newObject.transform.position = position;
		newObject.name = "VRUI_PressUpClip";

		newObject.GetComponent<VRUI_AudioSound>().Initialise(clips["PressUp"], uiVolume);
	}
}
