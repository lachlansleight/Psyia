using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMainCameraPosition : MonoBehaviour {

	public ComputeShader DistanceShader;
	Camera myCam;

	void Update () {
		myCam = Camera.main;
		if(Camera.main == null) {
			myCam = GameObject.FindObjectOfType<Camera>();
		}
		if(myCam != null) {
			DistanceShader.SetVector("CameraPosition", Camera.main.transform.position);
		}
	}
}
