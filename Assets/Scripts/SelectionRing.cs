using UnityEngine;
using System.Collections;

public class SelectionRing : MonoBehaviour {

	public Renderer ring;
	public Renderer dot;
	Material ringMat;
	public Color outerCol;
	public Color innerCol;

	public bool isLeft = false;

	public StarLab starLab;

	public Collider inactiveZone;

	public VRTools.UI.VRUI_Input uiInput;

	// Use this for initialization
	void Start () {
		ring.enabled = dot.enabled = true;
		isLeft = transform.parent.name.Equals("LeftController") || transform.parent.name.Equals("Controller (left)");
		ringMat = ring.material;
	}
	
	// Update is called once per frame
	void Update () {
		bool leftInactive = false;
		bool rightInactive = false;

		if(starLab == null) {
			
			if(inactiveZone == null) {
				inactiveZone = GameObject.Find("FirstTimeControls").GetComponent<Collider>();
			}

			leftInactive = inactiveZone.bounds.Contains(VRTools.VRInput.GetDevice("ViveLeft").position);
			rightInactive = inactiveZone.bounds.Contains(VRTools.VRInput.GetDevice("ViveRight").position);

			if(isLeft && leftInactive) ring.enabled = dot.enabled = true;
			else if(!isLeft && rightInactive) ring.enabled = dot.enabled = true;
			else ring.enabled = dot.enabled = false;
		} else {
			if(isLeft && starLab.leftInactive) ring.enabled = dot.enabled = true;
			else if(!isLeft && starLab.rightInactive) ring.enabled = dot.enabled = true;
			else ring.enabled = dot.enabled = false;
		}

		float triggerAmount = VRTools.VRInput.GetDevice(isLeft ? "ViveLeft" : "ViveRight").GetAxis("Trigger");
		ring.gameObject.SetActive(triggerAmount > 0);
		ring.transform.localScale = Vector3.one * Mathf.Lerp(1f, 0f, triggerAmount);
		ringMat.color = Color.Lerp(outerCol, innerCol, triggerAmount);
	}
}
