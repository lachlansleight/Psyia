using UnityEngine;
using System.Collections;

public class NumberPlaysIncrement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		if(!PlayerPrefs.HasKey("NumberPlays")) PlayerPrefs.SetInt("NumberPlays", 1);
		else PlayerPrefs.SetInt("NumberPlays", PlayerPrefs.GetInt("NumberPlays") + 1);

		//PlayerPrefs.SetInt("NumberPlays", 1);

	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.T)) PlayerPrefs.SetInt("NumberPlays", 1);
	}
}
