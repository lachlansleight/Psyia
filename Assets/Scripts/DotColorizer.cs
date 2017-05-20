using UnityEngine;
using System.Collections;

public class DotColorizer : MonoBehaviour {

	public bool AlsoDrawLine = true;

	Color onCol = Color.white;
	Color offCol = Color.white;
	Material myMat;

	// Use this for initialization
	void Start () {
		myMat = GetComponent<Renderer>().material;

		offCol = myMat.color;
		onCol = new Color(offCol.r * 0.5f, offCol.g * 0.5f, 1f, offCol.a);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetCol(bool isOn, Vector3 pos) {
		if(myMat == null) myMat = GetComponent<Renderer>().material;

		if(isOn) {
			if(AlsoDrawLine == true) {
				VRTools.VRDebug.DrawLine(transform.position, pos, Color.white, true, 0.0005f);
			}
			myMat.color = onCol;
		} else myMat.color = offCol;
	}
}
