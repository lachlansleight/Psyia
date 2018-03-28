using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;
using UnityEngine.UI;

public class MMOne_Canvas : MonoBehaviour {

	Dictionary<string, Text> KnobTexts;
	Dictionary<string, Text> SliderTexts;
	Dictionary<string, Image> ButtonImages;

	public Transform KnobParent;
	public Transform SliderParent;
	public Transform ButtonParent;

	private void Awake() {
		KnobTexts = new Dictionary<string, Text>();
		for(int i = 0; i < KnobParent.childCount; i++) {
			KnobTexts.Add(KnobParent.GetChild(i).name, KnobParent.GetChild(i).GetComponent<Text>());
		}

		SliderTexts = new Dictionary<string, Text>();
		for(int i = 0; i < SliderParent.childCount; i++) {
			SliderTexts.Add(SliderParent.GetChild(i).name, SliderParent.GetChild(i).GetComponent<Text>());
		}

		ButtonImages = new Dictionary<string, Image>();
		for(int i = 0; i < ButtonParent.childCount; i++) {
			ButtonImages.Add(ButtonParent.GetChild(i).name, ButtonParent.GetChild(i).GetComponent<Image>());
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach(KeyValuePair<string, Text> pair in KnobTexts) {
			pair.Value.text = Mathf.RoundToInt(MMOne.GetKnob(pair.Key) * 255f) + "";
		}
		foreach(KeyValuePair<string, Text> pair in SliderTexts) {
			pair.Value.text = Mathf.RoundToInt(MMOne.GetSlider(pair.Key) * 255f) + "";
		}
		foreach(KeyValuePair<string, Image> pair in ButtonImages) {
			bool DownThisFrame = MMOne.GetButtonDown(pair.Key);
			bool UpThisFrame = MMOne.GetButtonUp(pair.Key);
			bool Down = MMOne.GetButton(pair.Key);

			if(DownThisFrame) {
				pair.Value.color = Color.red;
			} else if(UpThisFrame) {
				pair.Value.color = Color.blue;
			} else if(Down) {
				pair.Value.color = Color.white;
			} else {
				pair.Value.color = Color.black;
			}
		}
	}
}
