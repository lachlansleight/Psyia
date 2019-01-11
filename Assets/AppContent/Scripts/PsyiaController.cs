﻿using UnityEngine;
using System.Collections;

public class PsyiaController : MonoBehaviour
{

	public Transform SourceTransform;
	public ControllerForce ForceValue;

	public float minSpinRate = 10f;
	public float maxSpinRate = 1000f;

	public float minSphereSize = 0.005f;
	public float maxSphereSize = 0.04f;

	public Color minSphereCol;
	public Color maxSphereCol;

	public GameObject fancy;
	public GameObject minimal;
	public GameObject fancyBase;

	public Transform spinner;
	public Transform sphere;
	public Transform minimalSphere;
	public Material sphereMat;

	float spinnerRot = 0f;

	public bool ShowFullModel = true;
	public float ControllerDistance;
	public XRP.XrpPointer Pointer;

	// Use this for initialization
	void Start () {
		sphereMat = sphere.GetComponent<Renderer>().sharedMaterial;

		fancy.SetActive(ShowFullModel);
		minimal.SetActive(!ShowFullModel);
	}
	
	// Update is called once per frame
	public void Update () {

		if(ControllerDistance < 0.05f) {
			transform.position = SourceTransform.position + SourceTransform.forward * 0.03f;
			transform.rotation = SourceTransform.rotation;
		} else {
			var slerpyPos = Vector3.Slerp(transform.position - SourceTransform.position, SourceTransform.forward * (ControllerDistance + 0.03f), Mathf.Lerp(0.2f, 0.02f, Mathf.InverseLerp(0.05f, 2f, ControllerDistance)));
			transform.position = slerpyPos + SourceTransform.position;
			transform.rotation = SourceTransform.rotation;
		}

		//no matter whether we're on the menu or not, the base should be enabled/disabled regardless
		if(fancyBase.activeSelf != ShowFullModel) fancyBase.SetActive(ShowFullModel);

		var value = Pointer.Hovering ? 0f : ForceValue.Value;
		
		//if we're on the menu with this controller, disable attractor
		if(Pointer.Hovering) {
			fancy.SetActive(false);
			minimal.SetActive(false);
			
		} else {
			if(ShowFullModel) {
				fancy.SetActive(true);
				minimal.SetActive(false);

				//spin
				spinnerRot += Mathf.Lerp(minSpinRate, maxSpinRate, Mathf.Pow(Mathf.Clamp01(value), 3f)) * Time.deltaTime;
				spinner.localRotation = Quaternion.Euler(spinnerRot, 90f, 90f);

				//grow sphere
				sphere.localScale = Vector3.one * Mathf.Lerp(minSphereSize, maxSphereSize, Mathf.Clamp01(value));
			} else {
				fancy.SetActive(false);
				minimal.SetActive(true);
			}

			//glow
			sphereMat.SetColor("_EmissionColor", Color.Lerp(minSphereCol, maxSphereCol, Mathf.Clamp01(value)));
		}
	}
}