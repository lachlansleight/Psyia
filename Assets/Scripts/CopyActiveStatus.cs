using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CopyActiveStatus : MonoBehaviour {

	public GameObject sourceObj;

	Text myText;

	Color myColOpaque;
	Color myColTransp;

	// Use this for initialization
	void Start () {
		myText = GetComponent<Text>();

		myColOpaque = myText.color;
		myColTransp = myText.color;
		myColTransp.a = 0;
	}
	
	// Update is called once per frame
	void Update () {
		myText.color = sourceObj.activeInHierarchy ? myColOpaque : myColTransp;
	}
}
