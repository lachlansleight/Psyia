using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatePosition : MonoBehaviour {

	public bool SetCenterOnStart = true;
	public Vector3 Center;
	public Vector3 Amplitude;
	public Vector3 Period;
	public Vector3 Phase;

	private Vector3 t;

	// Use this for initialization
	void Start () {
		if(SetCenterOnStart) Center = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		t.x += Time.deltaTime / Period.x;
		t.y += Time.deltaTime / Period.y;
		t.z += Time.deltaTime / Period.z;

		transform.position = new Vector3(
			Amplitude.x * Mathf.Sin(t.x + Phase.x) + Center.x,
			Amplitude.y * Mathf.Sin(t.y + Phase.y) + Center.y,
			Amplitude.z * Mathf.Sin(t.z + Phase.z) + Center.z
		);
	}
}
