using UnityEngine;
using System.Collections;
using VRTools;

public class StarController : MonoBehaviour {

	[Range(0f, 1f)] public float value = 0f;

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

	public string deviceName;
	public string axis;

	public StarLab starLab;

	// Use this for initialization
	void Start () {
		sphereMat = sphere.GetComponent<Renderer>().sharedMaterial;

		fancy.SetActive(PsyiaSettings.ControllerModels);
		minimal.SetActive(!PsyiaSettings.ControllerModels);
	}
	
	// Update is called once per frame
	void Update () {
		VRInputDevice device = VRInput.GetDevice(deviceName);

		value = Mathf.Clamp01(device.GetAxis(axis));
		if(PsyiaSettings.LeftTouchpadFunction == 1 && deviceName == "ViveLeft" && device.GetButton("Touchpad")) value = 0.1f;
		else if(PsyiaSettings.RightTouchpadFunction == 1 && deviceName == "ViveRight" && device.GetButton("Touchpad")) value = 0.1f;

		if(PsyiaSettings.ControllerDistance < 0.05f) {
			transform.position = device.position + device.forward * PsyiaSettings.ControllerDistance;
		} else {
			Vector3 slerpyPos = Vector3.Slerp(transform.position - device.position, device.forward * PsyiaSettings.ControllerDistance, Mathf.Lerp(0.2f, 0.02f, Mathf.InverseLerp(0.05f, 2f, PsyiaSettings.ControllerDistance)));
			transform.position = slerpyPos + device.position;
		}

		transform.rotation = device.rotation;

		//no matter whether we're on the menu or not, the base should be enabled/disabled regardless
		if(fancyBase.activeSelf != PsyiaSettings.ControllerModels) fancyBase.SetActive(PsyiaSettings.ControllerModels);

		//if we're on the menu with this controller, disable attractor
		if((starLab.leftInactive && deviceName.Equals("ViveLeft")) || (starLab.rightInactive && deviceName.Equals("ViveRight"))) {
			fancy.SetActive(false);
			minimal.SetActive(false);
		} else {
			if(PsyiaSettings.ControllerModels) {
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
