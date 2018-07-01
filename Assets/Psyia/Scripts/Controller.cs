using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	[Range(0f, 1f)] public float value = 0f;

	public VRDevice MyDevice;

	public float minSpinRate = 10f;
	public float maxSpinRate = 1000f;

	public float minSphereSize = 0.005f;
	public float maxSphereSize = 0.04f;

	public Color minSphereCol;
	public Color maxSphereCol;

	public Transform spinner;
	public Transform sphere;
	Material sphereMat;

	float spinnerRot = 0f;

	// Use this for initialization
	void Start () {
		sphereMat = sphere.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		value = VRTK_Devices.TriggerAxis(MyDevice);

		spinnerRot += Mathf.Lerp(minSpinRate, maxSpinRate, Mathf.Pow(Mathf.Clamp01(value), 3f)) * Time.deltaTime;
		spinner.localRotation = Quaternion.Euler(spinnerRot, 90f, -90f);
		sphere.localScale = Vector3.one * Mathf.Lerp(minSphereSize, maxSphereSize, Mathf.Clamp01(value));
		sphereMat.SetColor("_EmissionColor", Color.Lerp(minSphereCol, maxSphereCol, Mathf.Clamp01(value)));
	}
}
