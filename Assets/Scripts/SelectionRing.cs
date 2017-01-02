using UnityEngine;
using System.Collections;

public class SelectionRing : MonoBehaviour {

	public Renderer ring;
	public Renderer dot;
	Material ringMat;
	public Color outerCol;
	public Color innerCol;

	public bool isLeft = false;

	public Dispatcher compute;

	// Use this for initialization
	void Start () {
		ring.enabled = dot.enabled = true;
		isLeft = transform.parent.name.Equals("LeftController");
		ringMat = ring.material;
	}
	
	// Update is called once per frame
	void Update () {
		if(isLeft && compute.leftInactive) ring.enabled = dot.enabled = true;
		else if(!isLeft && compute.rightInactive) ring.enabled = dot.enabled = true;
		else ring.enabled = dot.enabled = false;

		ring.transform.localScale = Vector3.one * Mathf.Lerp(1f, 0f, VRTools.VRInput.GetDevice(isLeft ? "ViveLeft" : "ViveRight").GetAxis("Trigger"));
		ringMat.color = Color.Lerp(outerCol, innerCol, VRTools.VRInput.GetDevice(isLeft ? "ViveLeft" : "ViveRight").GetAxis("Trigger"));
		
	}
}
